using StockManagement.DataAccessLayer.Interfaces;
using StockManagement.Model;
using System;
using System.Linq;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class StockService : IStockService
    {
        private readonly IStockContext _stockContext;

        public StockService(IStockContext stockContext)
        {
            _stockContext = stockContext;
        }

        public void MakeSomeTestWork()
        {
            AddTestData();

            try
            {
                StockItem stockItem4 = new StockItem();
                stockItem4.Name = "Meta";
                stockItem4.ISIN = "US30303M1027";
                stockItem4.Price = 66.15m;
                stockItem4.Quantity = 10;

                _stockContext.AddStockItem(stockItem4);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("try to add existing ISIN");
            }

            var stockItemList = _stockContext.GetAllStockItems();
            stockItemList[0].Price = 200.00m;

            _stockContext.UpdateStockItem(stockItemList[0]);

            var stockItemList2 = _stockContext.GetAllStockItems();

            //LINQ operations
            //Querying stock items below a certain quantity.
            var testQuantity = 50;
            var stockItemNewList0 = stockItemList2.Where(x => x.Quantity < testQuantity).ToList();

            //Query Below Threshold: Show any stock with Quantity < threshold.
            var threshold = 150.00m;
            var stockItemNewList = stockItemList2.Where(x => x.Price < threshold).ToList();

            //Search by name
            var stockItemNewList2 = stockItemList2.Where(x => x.Name.Contains("Apple")).ToList();
        }

        private void AddTestData()
        {
            StockItem stockItem = new StockItem();
            stockItem.Name = "Apple Inc.";
            stockItem.ISIN = "US0378331005";
            stockItem.Price = 123.45m;
            stockItem.Quantity = 100;

            StockItem stockItem2 = new StockItem();
            stockItem2.Name = "Meta Platforms Inc.";
            stockItem2.ISIN = "US30303M1027";
            stockItem2.Price = 23.45m;
            stockItem2.Quantity = 40;

            StockItem stockItem3 = new StockItem();
            stockItem3.Name = "Amazon.com, Inc";
            stockItem3.ISIN = "US0231351067";
            stockItem3.Price = 180.45m;
            stockItem3.Quantity = 10;

            _stockContext.AddStockItem(stockItem);
            _stockContext.AddStockItem(stockItem2);
            _stockContext.AddStockItem(stockItem3);
        }
    }
}
