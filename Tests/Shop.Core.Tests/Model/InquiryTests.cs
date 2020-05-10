using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Tests.Model
{
    [TestClass]
    public class InquiryTests
    {
        [TestMethod]
        public void ShouldAddNewOrderLine_WhenItemDoesNotExist()
        {
            // arrange
            var createdOn = DateTime.Parse("2020-01-12 03:46:55");
            var owner = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var product = Product.Create("Test Name", "TestCategory", 100, null);

            // act            
            var sut = Inquiry.Create(product, owner, clientId, createdOn);

            // assert
            Assert.AreEqual(expected: 1, sut.OrderLines.Count);
            Assert.AreEqual(expected: product.Id, sut.OrderLines[0].Item.Id);
        }

        [TestMethod]
        public void ShouldReplaceOrderLine_WhenItemDoesExist_IncrementByOne()
        {
            // arrange
            var createdOn = DateTime.Parse("2020-01-12 03:46:55");
            var owner = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var product = Product.Create("Test Name", "TestCategory", 100, null);
            var sut = Inquiry.Create(product, owner, clientId, createdOn);

            // act            
            sut.AddProduct(product);

            // assert
            Assert.AreEqual(expected: 1, sut.OrderLines.Count);
            Assert.AreEqual(expected: product.Id, sut.OrderLines[0].Item.Id);
            Assert.AreEqual(expected: 2, sut.OrderLines[0].Amount);
        }

        [TestMethod]
        public void ShouldDesignateCustomer()
        {
            // arrange
            var createdOn = DateTime.Parse("2020-01-12 03:46:55");
            var owner = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var product = Product.Create("Test Name", "TestCategory", 100, null);
            var sut = Inquiry.Create(product, owner, clientId, createdOn);
            var customer = CustomerInformation.Create("tranq@twitch.tv", "tranq", "uiliza", "0011223344", "Denmark", "0000", "Odense", "Odensevej 02");
            // act
            sut.SetCustomerInformation(customer);

            // assert
            Assert.IsNotNull(sut);
            Assert.AreEqual(expected: "tranq@twitch.tv", actual: sut.CustomerInformation.Email);
        }

        [TestMethod]
        public void ShouldReturnTheTotalOfTheOrder()
        {
            // arrange
            var createdOn = DateTime.Parse("2020-01-12 03:46:55");
            var owner = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var product = Product.Create("Product One", "TestCategory", 100, null);
            var sut = Inquiry.Create(product, owner, clientId, createdOn);
            sut.AddProduct(product, 7);

            var productExpensive = Product.Create("My expensive product!", "TestCategory", 200000, null);
            sut.AddProduct(productExpensive);

            // act
            var result = sut.GetTotal();

            // assert
            Assert.AreEqual(expected: 2008, actual: result);
        }

        [TestMethod]
        [DataRow(InquiryState.AddingToCart, InquiryState.Placed, true)]
        [DataRow(InquiryState.Placed, InquiryState.Placed, true)]
        [DataRow(InquiryState.Placed, InquiryState.AddingToCart, true)]
        [DataRow(InquiryState.PaymentExpected, InquiryState.Placed, false)]
        [DataRow(InquiryState.PaymentExpected, InquiryState.PaymentExpected, false)]
        [DataRow(InquiryState.PaymentExpected, InquiryState.PaymentReceived, true)]
        [DataRow(InquiryState.PaymentReceived, InquiryState.Dispatched, true)]
        [DataRow(InquiryState.PaymentReceived, InquiryState.PaymentExpected, false)]
        public void UpdateState(InquiryState currentState, InquiryState newState, bool shouldSucceed)
        {
            // arrange
            var createdOn = DateTime.Parse("2020-01-12 03:46:55");
            var owner = Guid.NewGuid();
            var client = Guid.NewGuid();
            var product = Product.Create("Test", "Test", 100, "Desc");
            var inquiry = ForceState(Inquiry.Create(product, owner, client, createdOn), currentState);

            // act
            var result = inquiry.TryUpdateState(newState, null);

            // assert
            Assert.AreEqual(expected: shouldSucceed, actual: result, message: "Unexpected result");
        }

        private Inquiry ForceState(Inquiry inquiry, InquiryState desiredState)
        {
            var type = typeof(Inquiry);
            var property = type.GetProperty(nameof(inquiry.State));
            property.SetValue(inquiry, desiredState);

            return inquiry;
        }
    }
}
