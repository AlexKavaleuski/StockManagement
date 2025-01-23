using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using StockManagement.DataAccessLayer;
using StockManagement.Model;
using System.Security.Cryptography;
using StockManagement.DataAccessLayer.Interfaces;

namespace StockManagement.Test
{
    [TestClass]
    public class StockContextTests
    {
        private IStockContext _stockContext;

        [TestInitialize]
        public void Setup()
        {
            _stockContext = new StockContext();
            _stockContext.InitTestStockContext();
        }

        [TestMethod]
        public void AddStockItem_ShouldAddItem_Succeeds()
        {
            // Arrange
            var stockItem = new StockItem
            {
                Name = "Apple Inc.",
                ISIN = "US0378331005",
                Price = 150.25m,
                Quantity = 15
            };

            // Act
            _stockContext.AddStockItem(stockItem);
            var retrievedItem = _stockContext.GetAllStockItems().FirstOrDefault(x=>x.ISIN == stockItem.ISIN);

            // Assert
            Assert.IsNotNull(retrievedItem);
            Assert.AreEqual(stockItem.Name, retrievedItem.Name);
            Assert.AreEqual(stockItem.ISIN, retrievedItem.ISIN);
            Assert.AreEqual(stockItem.Price, retrievedItem.Price);
            Assert.AreEqual(stockItem.Quantity, retrievedItem.Quantity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddStockItem_PreventDuplicatesByIsin_ArgumentException()
        {
            // Arrange
            StockItem stockItem = new StockItem();
            stockItem.Name = "Meta Platforms Inc.";
            stockItem.ISIN = "US30303M1027";
            stockItem.Price = 23.45m;
            stockItem.Quantity = 40;

            // Act
            _stockContext.AddStockItem(stockItem);
            _stockContext.AddStockItem(stockItem);

            // Assert expect ArgumentException
        }

        [TestMethod]
        public void UpdateStockItem_ShouldUpdateItem_Succeeds()
        {
            // Arrange
            var stockItemToAdd = new StockItem
            {
                Name = "Microsoft Corporation",
                ISIN = "US5949181045",
                Price = 150.25m,
                Quantity = 15
            };

            _stockContext.AddStockItem(stockItemToAdd);

            var stockItem = new StockItem
            {
                ISIN = "US5949181045",
                Price = 22.25m,
                Quantity = 3
            };

            // Act
            _stockContext.UpdateStockItem(stockItem);
            var retrievedItem = _stockContext.GetStockItem(stockItem.ISIN);

            // Assert
            Assert.IsNotNull(retrievedItem);
            Assert.AreEqual(stockItem.ISIN, retrievedItem.ISIN);
            Assert.AreEqual(stockItem.Price, retrievedItem.Price);
            Assert.AreEqual(stockItem.Quantity, retrievedItem.Quantity);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateStockItem_StockNotExist_ArgumentException()
        {
            // Arrange
            StockItem stockItem = new StockItem();
            stockItem.Name = "test";
            stockItem.ISIN = "test";
            stockItem.Price = 333.55m;
            stockItem.Quantity = 1;

            // Act
            _stockContext.UpdateStockItem(stockItem);

            // Assert expect ArgumentException
        }

        [TestMethod]
        public void GetAllStockItems_CountShouldBeMoreThanZero_Succeeds()
        {
            // Arrange
            // Act
            var stockItemList = _stockContext.GetAllStockItems();

            // Assert
            Assert.AreEqual(true, stockItemList.Count > 0);
        }
    }
}
