using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
            var owner = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var product = Product.Create("Test Name", "TestCategory", 100, null);

            // act            
            var sut = Inquiry.Create(product, owner, clientId);

            // assert
            Assert.AreEqual(expected: 1, sut.OrderLines.Count);
            Assert.AreEqual(expected: product.Id, sut.OrderLines[0].Item.Id);
        }

        [TestMethod]
        public void ShouldReplaceOrderLine_WhenItemDoesExist_IncrementByOne()
        {
            // arrange
            var owner = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var product = Product.Create("Test Name", "TestCategory", 100, null);
            var sut = Inquiry.Create(product, owner, clientId);

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
            var owner = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var product = Product.Create("Test Name", "TestCategory", 100, null);
            var sut = Inquiry.Create(product, owner, clientId);
            var customer = CustomerInformation.Create("tranq@twitch.tv", "tranq", "uiliza", "twitch", "0011223344");

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
            var owner = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var product = Product.Create("Product One", "TestCategory", 100, null);
            var sut = Inquiry.Create(product, owner, clientId);
            sut.AddProduct(product, 7);

            var productExpensive = Product.Create("My expensive product!", "TestCategory", 200000, null);
            sut.AddProduct(productExpensive);

            // act
            var result = sut.GetTotal();

            // assert
            Assert.AreEqual(expected: 2008, actual: result);
        }
    }
}
