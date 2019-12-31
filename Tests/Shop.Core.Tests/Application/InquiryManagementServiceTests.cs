﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Application;
using Tranquiliza.Shop.Core.Model;
using Tranquiliza.Shop.Core.Tests.Helpers;

namespace Tranquiliza.Shop.Core.Tests.Application
{
    [TestClass]
    public class InquiryManagementServiceTests
    {
        private class TestContext : IApplicationContext
        {
            public Guid UserId { get; private set; }

            public Guid ClientId { get; private set; }

            public bool IsAnonymous { get; private set; }

            public User User { get; private set; }

            public TestContext(Guid userId, Guid clientId, bool isAnonymous)
            {
                UserId = userId;
                ClientId = clientId;
                IsAnonymous = isAnonymous;
                if (!IsAnonymous)
                    User = User.CreateUserForTest("Test@Example.com", userId);
            }
        }

        [TestMethod]
        public async Task CreateInquiry()
        {
            // arrange
            var userId = Guid.Parse("9866b9fc-1636-4b21-a77c-b9f0a1229505");
            var clientId = Guid.Parse("c64906a7-7b92-4dc5-aabf-b1a695b35e4b");
            var isAnon = false;
            var productId = Guid.Parse("c4e17288-f276-46a6-9520-8d5a2c1b0427");
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(Product.Create("Test", "Test", 100, ""));

            var inquiryRepository = new Mock<IInquiryRepository>();
            var sut = new InquiryManagementServiceFactory()
                .With(productRepository.Object)
                .With(inquiryRepository.Object)
                .Build();
            var applicationContext = new TestContext(userId, clientId, isAnon);

            // act
            var result = await sut.CreateInquiry(productId, applicationContext).ConfigureAwait(false);

            // assert
            Assert.AreEqual(ResultState.Success, result.State);
            var resultData = result.Data;
            Assert.AreEqual(resultData.UserId, userId);
            Assert.AreEqual(resultData.CreatedByClient, clientId);
            Assert.AreEqual(resultData.State, InquiryState.AddingToCart);

            inquiryRepository.Verify(x => x.Save(It.Is<Inquiry>(y =>
                    y.CreatedByClient == clientId
                    && y.UserId == userId
                )
            ));
        }

        [TestMethod]
        public async Task CreateInquiry_Anonymous()
        {
            // arrange
            var clientId = Guid.Parse("c64906a7-7b92-4dc5-aabf-b1a695b35e4b");
            var isAnon = true;
            var productId = Guid.Parse("c4e17288-f276-46a6-9520-8d5a2c1b0427");
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(Product.Create("Test", "Test", 100, ""));

            var inquiryRepository = new Mock<IInquiryRepository>();
            var sut = new InquiryManagementServiceFactory()
                .With(productRepository.Object)
                .With(inquiryRepository.Object)
                .Build();
            var applicationContext = new TestContext(default, clientId, isAnon);

            // act
            var result = await sut.CreateInquiry(productId, applicationContext).ConfigureAwait(false);

            // assert
            Assert.AreEqual(ResultState.Success, result.State);
            var resultData = result.Data;
            Assert.AreEqual(resultData.CreatedByClient, clientId);
            Assert.AreEqual(resultData.UserId, default);
            Assert.AreEqual(resultData.State, InquiryState.AddingToCart);

            inquiryRepository.Verify(x => x.Save(It.Is<Inquiry>(y =>
                    y.CreatedByClient == clientId
                    && y.UserId == default
                )
            ));
        }

        [TestMethod]
        public async Task CreateInquiry_FailBecauseNoproduct()
        {
            // arrange
            var userId = Guid.Parse("9866b9fc-1636-4b21-a77c-b9f0a1229505");
            var clientId = Guid.Parse("c64906a7-7b92-4dc5-aabf-b1a695b35e4b");
            var isAnon = false;
            var productId = Guid.Parse("c4e17288-f276-46a6-9520-8d5a2c1b0427");
            var productRepository = new Mock<IProductRepository>();
            var sut = new InquiryManagementServiceFactory()
                .With(productRepository.Object)
                .Build();
            var applicationContext = new TestContext(userId, clientId, isAnon);

            // act
            var result = await sut.CreateInquiry(productId, applicationContext).ConfigureAwait(false);

            // assert
            Assert.AreEqual(ResultState.Failure, result.State);
            Assert.AreEqual("Product was not found", result.FailureReason);
        }

        [TestMethod]
        public async Task AddProductToInquiry()
        {
            // arrange
            var userId = Guid.Parse("9866b9fc-1636-4b21-a77c-b9f0a1229505");
            var clientId = Guid.Parse("c64906a7-7b92-4dc5-aabf-b1a695b35e4b");
            var isAnon = false;
            var productId = Guid.Parse("c4e17288-f276-46a6-9520-8d5a2c1b0427");
            var testProduct = Product.Create("Test", "Test", 100, "");
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(testProduct);

            var inquiry = Inquiry.Create(testProduct, userId, clientId);
            var inquiryRepository = new Mock<IInquiryRepository>();
            inquiryRepository.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(inquiry);

            var sut = new InquiryManagementServiceFactory()
                .With(productRepository.Object)
                .With(inquiryRepository.Object)
                .Build();
            var applicationContext = new TestContext(userId, clientId, isAnon);

            // act
            var result = await sut.AddProductToInquiry(inquiry.Id, productId, 3, applicationContext).ConfigureAwait(false);

            // assert
            Assert.AreEqual(ResultState.Success, result.State);
            var data = result.Data;
            Assert.AreEqual(1, data.OrderLines.Count);
            Assert.AreEqual(4, data.OrderLines[0].Amount);

            inquiryRepository.Verify(x => x.Save(It.Is<Inquiry>(y =>
                    y.State == InquiryState.AddingToCart
                    && y.OrderLines.Count == 1
                    && y.OrderLines[0].Amount == 4
                )
            ));
        }

        [TestMethod]
        public async Task ContextDoesNotHaveAccessToInquiry()
        {
            // arrange
            var userId = default(Guid);
            var clientId = Guid.Parse("c64906a7-7b92-4dc5-aabf-b1a695b35e4b");
            var isAnon = true;
            var productId = Guid.Parse("c4e17288-f276-46a6-9520-8d5a2c1b0427");
            var testProduct = Product.Create("Test", "Test", 100, "");
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(testProduct);

            var inquiry = Inquiry.Create(testProduct, userId, clientId);
            var inquiryRepository = new Mock<IInquiryRepository>();
            inquiryRepository.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(inquiry);

            var sut = new InquiryManagementServiceFactory()
                .With(productRepository.Object)
                .With(inquiryRepository.Object)
                .Build();
            var anonUserId = default(Guid);
            var incorrectClientId = Guid.NewGuid();
            var applicationContext = new TestContext(anonUserId, incorrectClientId, isAnon);

            // act
            var result = await sut.AddProductToInquiry(inquiry.Id, productId, 3, applicationContext).ConfigureAwait(false);

            // assert
            Assert.AreEqual(ResultState.AccessDenied, result.State);
        }

        [TestMethod]
        public async Task ContextIsNotAnonButClientHasAnInquiry()
        {
            // arrange
            var userId = default(Guid);
            var clientId = Guid.Parse("c64906a7-7b92-4dc5-aabf-b1a695b35e4b");
            var isAnon = false;
            var productId = Guid.Parse("c4e17288-f276-46a6-9520-8d5a2c1b0427");
            var testProduct = Product.Create("Test", "Test", 100, "");
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(testProduct);

            var inquiry = Inquiry.Create(testProduct, userId, clientId);
            var inquiryRepository = new Mock<IInquiryRepository>();
            inquiryRepository.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(inquiry);

            var sut = new InquiryManagementServiceFactory()
                .With(productRepository.Object)
                .With(inquiryRepository.Object)
                .Build();

            var incorrectUserId = Guid.NewGuid();
            var applicationContext = new TestContext(incorrectUserId, clientId, isAnon);

            // act
            var result = await sut.AddProductToInquiry(inquiry.Id, productId, 3, applicationContext).ConfigureAwait(false);

            // assert
            Assert.AreEqual(ResultState.Success, result.State);
        }

        [TestMethod]
        public async Task CustomerInformationAddedToInquiry_UpdateInquiryOwner_ToUserId()
        {
            // arrange
            var clientId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var inquiry = CreateInquiryFromAnon();
            var email = "email@somewhere.com";
            var firstName = "Daniel";
            var surName = "Tranquiliza";
            var phoneNumber = "0011223344";

            var country = "Denmark";
            var zipCode = "0000";
            var city = "Odense";
            var streetNumber = "Somewhere 1";

            var applicationContext = new TestContext(userId, clientId, false);

            var productRepository = new Mock<IProductRepository>();
            var customerRepository = new Mock<ICustomerInformationRepository>();
            var inquiryRepository = new Mock<IInquiryRepository>();
            inquiryRepository.Setup(x => x.Get(inquiry.Id))
                .ReturnsAsync(inquiry);

            var sut = new InquiryManagementServiceFactory()
                .With(productRepository.Object)
                .With(inquiryRepository.Object)
                .With(customerRepository.Object)
                .Build();

            // act
            var result = await sut.AddCustomerToInquiry(inquiry.Id, email, firstName, surName, phoneNumber, country, zipCode, city, streetNumber, applicationContext).ConfigureAwait(false);

            // assert
            Assert.AreEqual(ResultState.Success, result.State);

            customerRepository.Verify(x => x.Save(It.Is<CustomerInformation>(y =>
                y.Email == email
                && y.FirstName == firstName
                && y.Surname == surName
                && y.PhoneNumber == phoneNumber
                && y.Country == country
                && y.ZipCode == zipCode
                && y.City == city
                && y.StreetNumber == streetNumber
            )));

            inquiryRepository.Verify(x => x.Save(It.Is<Inquiry>(y =>
                    y.UserId == userId
                )));

            Inquiry CreateInquiryFromAnon()
            {
                var product = Product.Create("Test", "Test", 100, "");
                return Inquiry.Create(product, default, clientId);
            }
        }

        [TestMethod]
        public async Task CustomerInformationAddedToInquiry_CustomerInformationDoesNotExist_Create()
        {
            // arrange
            var clientId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var inquiry = CreateInquiryFromAnon();
            var email = "email@somewhere.com";
            var firstName = "Daniel";
            var surName = "Tranquiliza";
            var address = "twitch.tv/tranquiliza";
            var phoneNumber = "0011223344";

            var country = "Denmark";
            var zipCode = "0000";
            var city = "Odense";
            var streetNumber = "Somewhere 1";

            var applicationContext = new TestContext(userId, clientId, false);

            var productRepository = new Mock<IProductRepository>();
            var customerRepository = new Mock<ICustomerInformationRepository>();
            var inquiryRepository = new Mock<IInquiryRepository>();
            inquiryRepository.Setup(x => x.Get(inquiry.Id))
                .ReturnsAsync(inquiry);

            var sut = new InquiryManagementServiceFactory()
                .With(productRepository.Object)
                .With(inquiryRepository.Object)
                .With(customerRepository.Object)
                .Build();

            // act
            var result = await sut.AddCustomerToInquiry(inquiry.Id, email, firstName, surName, phoneNumber, country, zipCode, city, streetNumber, applicationContext).ConfigureAwait(false);

            // assert
            Assert.AreEqual(ResultState.Success, result.State, message: result.FailureReason);

            customerRepository.Verify(x => x.Save(It.Is<CustomerInformation>(y =>
                y.Email == email
                && y.FirstName == firstName
                && y.Surname == surName
                && y.PhoneNumber == phoneNumber
                && y.Country == country
                && y.ZipCode == zipCode
                && y.City == city
                && y.StreetNumber == streetNumber
            )));

            inquiryRepository.Verify(x => x.Save(It.Is<Inquiry>(y =>
                    y.UserId == userId
                )));

            Inquiry CreateInquiryFromAnon()
            {
                var product = Product.Create("Test", "Test", 100, "");
                return Inquiry.Create(product, default, clientId);
            }
        }
    }
}
