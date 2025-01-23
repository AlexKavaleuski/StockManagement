using System;
using System.Collections.Generic;
using System.Data.SQLite;
using StockManagement.DataAccessLayer.Interfaces;
using StockManagement.Model;

namespace StockManagement.DataAccessLayer
{
    public class StockContext : IStockContext
    {
        private string _databaseName = "StockDb9.db";
        private string _connectionString;

        public StockContext()
        {
            _connectionString = String.Format("Data Source={0};Version=3;", _databaseName);
        }

        public void InitStockContext()
        {
            if (!System.IO.File.Exists(_databaseName))
            {
                SQLiteConnection.CreateFile(_databaseName);
            }

            string createStockItemTable = @"
                        CREATE TABLE IF NOT EXISTS StockItem (
                            Name TEXT NOT NULL,
                            ISIN TEXT PRIMARY KEY NOT NULL,
                            Price REAL NOT NULL,
                            Quantity INTEGER NOT NULL
                        );";

            SQLiteConnection sqlConnection = null;

            try
            {
                using (sqlConnection = new SQLiteConnection(_connectionString))
                {
                    using (SQLiteCommand sqlCommand = new SQLiteCommand(createStockItemTable, sqlConnection))
                    {
                        sqlConnection.Open();

                        sqlCommand.ExecuteNonQuery();

                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void InitTestStockContext()
        {
            _databaseName = String.Format("testDataBase_{0}.db", DateTime.Now.ToString("yyyyMMddTHHmmss"));
            _connectionString = String.Format("Data Source={0};Version=3;", _databaseName);

            InitStockContext();
        }

        public void AddStockItem(StockItem item)
        {
            //Prevent duplicates by ISIN
            string checkDuplicateQuery = @"
                        SELECT COUNT(1)
                        FROM StockItem
                        WHERE ISIN = @ISIN;";

            string insertStockItemQuery = @"
                        INSERT INTO StockItem (Name, ISIN, Price, Quantity)
                        VALUES (@Name, @ISIN, @Price, @Quantity);";

            try
            {
                using (SQLiteConnection sqlConnection = new SQLiteConnection(_connectionString))
                {
                    sqlConnection.Open();

                    using (SQLiteCommand checkDuplicateCommand = new SQLiteCommand(checkDuplicateQuery, sqlConnection))
                    {
                        checkDuplicateCommand.Parameters.AddWithValue("@ISIN", item.ISIN);

                        int count = Convert.ToInt32(checkDuplicateCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            throw new ArgumentException("Stock item with the same ISIN already exists.");
                        }
                    }

                    using (SQLiteCommand sqlCommand = new SQLiteCommand(insertStockItemQuery, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Name", item.Name);
                        sqlCommand.Parameters.AddWithValue("@ISIN", item.ISIN);
                        sqlCommand.Parameters.AddWithValue("@Price", item.Price);
                        sqlCommand.Parameters.AddWithValue("@Quantity", item.Quantity);

                        sqlCommand.ExecuteNonQuery();
                    }

                    sqlConnection.Close();
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void UpdateStockItem(StockItem item)
        {
            //Handle scenarios where the stock does not exist.
            var stockItem = GetStockItem(item.ISIN);
            
            if (stockItem == null || stockItem.ISIN == null)
            {
                throw new ArgumentException("Stock item does not exist.");
            }

            string updateStockItemQuery = @"
                        UPDATE StockItem
                        SET Price = @Price, Quantity = @Quantity
                        WHERE ISIN = @ISIN; ";

            try
            {
                using (SQLiteConnection sqlConnection = new SQLiteConnection(_connectionString))
                {
                    using (SQLiteCommand sqlCommand = new SQLiteCommand(updateStockItemQuery, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ISIN", item.ISIN);
                        sqlCommand.Parameters.AddWithValue("@Price", item.Price);
                        sqlCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                        
                        sqlConnection.Open();
                        
                        sqlCommand.ExecuteNonQuery();
                        
                        sqlConnection.Close();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<StockItem> GetAllStockItems()
        {
            string getAllStockItemQuery = @"
                        SELECT 
                            Name, 
                            ISIN, 
                            Price, 
                            Quantity
                        FROM StockItem;";

            List<StockItem> stockItemList = new List<StockItem>();

            try
            {
                using (SQLiteConnection sqlConnection = new SQLiteConnection(_connectionString))
                {
                    using (SQLiteCommand sqlCommand = new SQLiteCommand(getAllStockItemQuery, sqlConnection))
                    {
                        sqlConnection.Open();

                        using (var reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StockItem stockItem = new StockItem
                                {
                                    Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString(),
                                    ISIN = reader["ISIN"] == DBNull.Value ? string.Empty : reader["ISIN"].ToString(),
                                    Price = reader["Price"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Price"]),
                                    Quantity = reader["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Quantity"])
                                };

                                stockItemList.Add(stockItem);
                            }
                        }

                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return stockItemList;
        }

        public StockItem GetStockItem(string ISIN)
        {
            string getAllStockItemQuery = @"
                        SELECT 
                            Name, 
                            ISIN, 
                            Price, 
                            Quantity
                        FROM StockItem
                        WHERE ISIN = @ISIN; ";

            StockItem stockItem = new StockItem();

            try
            {
                using (SQLiteConnection sqlConnection = new SQLiteConnection(_connectionString))
                {
                    using (SQLiteCommand sqlCommand = new SQLiteCommand(getAllStockItemQuery, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ISIN", ISIN);

                        sqlConnection.Open();

                        using (var reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                stockItem.Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString();
                                stockItem.ISIN = reader["ISIN"] == DBNull.Value ? string.Empty : reader["ISIN"].ToString();
                                stockItem.Price = reader["Price"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Price"]);
                                stockItem.Quantity = reader["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Quantity"]);
                            }
                        }

                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return stockItem;
        }
    }
}
