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
            var product = Product.Create("Test Name", "TestCategory", 100);

            // act            
            var sut = Inquiry.Create(product);

            // assert
            Assert.AreEqual(expected: 1, sut.OrderLines.Count);
            Assert.AreEqual(expected: product.Id, sut.OrderLines[0].Item.Id);
        }

        [TestMethod]
        public void ShouldReplaceOrderLine_WhenItemDoesExist_IncrementByOne()
        {
            // arrange
            var product = Product.Create("Test Name", "TestCategory", 100);
            var sut = Inquiry.Create(product);

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
            var product = Product.Create("Test Name", "TestCategory", 100);
            var sut = Inquiry.Create(product);
            var customer = Customer.Create("tranq@twitch.tv");

            // act
            sut.DesignateCustomer(customer);

            // assert
            Assert.IsNotNull(sut);
            Assert.AreEqual(expected: "tranq@twitch.tv", actual: sut.Customer.Email);
        }

        [TestMethod]
        public void ShouldReturnTheTotalOfTheOrder()
        {
            // arrange
            var product = Product.Create("Product One", "TestCategory", 100);
            var sut = Inquiry.Create(product);
            sut.AddProduct(product, 7);

            var productExpensive = Product.Create("My expensive product!", "TestCategory", 200000);
            sut.AddProduct(productExpensive);

            // act
            var result = sut.GetTotal();

            // assert
            Assert.AreEqual(expected: 200800, actual: result);
        }
    }
}
