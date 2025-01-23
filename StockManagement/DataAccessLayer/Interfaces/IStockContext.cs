using StockManagement.Model;
using System.Collections.Generic;

namespace StockManagement.DataAccessLayer.Interfaces
{
    public interface IStockContext
    {
        void InitStockContext();

        void InitTestStockContext();

        void AddStockItem(StockItem stockItem);

        void UpdateStockItem(StockItem stockItem);

        List<StockItem> GetAllStockItems();

        StockItem GetStockItem(string ISIN);
    }
}