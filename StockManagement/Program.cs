using System;
using StockManagement.DataAccessLayer;
using StockManagement.Model;
using System.Collections.Generic;
using System.Linq;
using StockManagement.Services;

namespace StockManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StockService stockService = new StockService(new StockContext());

            stockService.MakeSomeTestWork();
        }
    }
}
