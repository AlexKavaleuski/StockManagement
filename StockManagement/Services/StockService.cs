﻿using StockManagement.DataAccessLayer.Interfaces;
using StockManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class StockService : IStockService
    {
        private readonly IStockContext _stockContext;
        private const int MaxQuantity = 50;
        private const decimal MaxThreshold = 150m;
        private const string SearchStockItemName = "Apple";

        public StockService(IStockContext stockContext)
        {
            _stockContext = stockContext;

            _stockContext.InitTestStockContext();
        }

        public void MakeSomeTestWork()
        {
            AddTestData();

            var stockItemList = _stockContext.GetAllStockItems();

            Console.WriteLine("\n all stock items: ");
            ShowResultInConsole(stockItemList);

            var stockItemsBelowQuantity = GetStockItemsBelowQuantity(stockItemList, MaxQuantity);

            Console.WriteLine(String.Format("\n stock items below {0} quantity: ", MaxQuantity));
            ShowResultInConsole(stockItemsBelowQuantity);

            var stockItemsBelowThreshold = GetStockItemsBelowThreshold(stockItemList, MaxThreshold);

            Console.WriteLine(String.Format("\n stock items below {0} threshold: ", MaxThreshold));
            ShowResultInConsole(stockItemsBelowThreshold);

            var stockItemsNameFilter = SearchStockItemsByName(stockItemList, SearchStockItemName);

            Console.WriteLine(String.Format("\n stock items contain '{0}': ", SearchStockItemName));
            ShowResultInConsole(stockItemsNameFilter);

            Console.ReadLine();
        }

        public IEnumerable<StockItem> SearchStockItemsByName(IEnumerable<StockItem> stockItemList, string searchStockItemName)
        {
            return stockItemList.Where(x => x.Name.IndexOf(searchStockItemName, StringComparison.OrdinalIgnoreCase) >=0 ).ToList();
        }

        public IEnumerable<StockItem> GetStockItemsBelowThreshold(IEnumerable<StockItem> stockItemList, decimal maxThreshold)
        {
            return stockItemList.Where(x => x.Price < maxThreshold).ToList();
        }

        public IEnumerable<StockItem> GetStockItemsBelowQuantity(IEnumerable<StockItem> stockItemList, int maxQuantity)
        {
            return stockItemList.Where(x => x.Quantity < maxQuantity).ToList();
        }

        private static void ShowResultInConsole(IEnumerable<StockItem> stockItemsBelowQuantity)
        {
            foreach (var stockItem in stockItemsBelowQuantity)
            {
                Console.WriteLine(String.Format("{0} {1} Quantity: {2} Price: {3}", stockItem.Name, stockItem.ISIN, stockItem.Quantity, stockItem.Price));
            }
        }

        private void AddTestData()
        {
            StockItem stockItem = new StockItem();
            stockItem.Name = "Apple Inc.";
            stockItem.ISIN = "US0378331005";
            stockItem.Price = 200m;
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
