using System;
using StockManagement.DataAccessLayer;
using StockManagement.Model;
using System.Collections.Generic;
using System.Linq;

namespace StockManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StockContext context = new StockContext();
            context.InitStockContext();

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

            context.AddStockItem(stockItem);
            context.AddStockItem(stockItem2);

            try
            {
                StockItem stockItem3 = new StockItem();
                stockItem3.Name = "Meta";
                stockItem3.ISIN = "US30303M1027";
                stockItem3.Price = 66.15m;
                stockItem3.Quantity = 10;

                context.AddStockItem(stockItem3);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("try to add existing ISIN");
            }

            var stockItemList = context.GetAllStockItems();

            stockItemList[0].Price = 200.00m;

            context.UpdateStockItem(stockItemList[0]);

            var stockItemList2 = context.GetAllStockItems();

            //search
            var threshold = 150.00m;
            var stockItemNewList = stockItemList2.Where(x => x.Price < threshold).ToList();

            //Search by name
            var stockItemNewList2 = stockItemList2.Where(x => x.Name.Contains("Apple")).ToList();
        }
    }
}
