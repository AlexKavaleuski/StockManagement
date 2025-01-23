# Stock Management System

This document provides instructions on how to use the `StockContext` and `StockService` classes in the Stock Management System.

## StockContext

The `StockContext` class is responsible for managing the stock database. It provides methods to initialize the database, add, update, and retrieve stock items.

### Methods

#### `void InitStockContext()`

Initializes the stock database. If the database file does not exist, it creates a new one and sets up the necessary tables.

#### `void InitTestStockContext()`

Initializes a test stock database with a unique name based on the current date and time. This is useful for testing purposes.

#### `void AddStockItem(StockItem item)`

Adds a new stock item to the database. Throws an `ArgumentException` if a stock item with the same ISIN already exists.

**Parameters:**
- `StockItem item`: The stock item to add.

#### `void UpdateStockItem(StockItem item)`

Updates an existing stock item in the database. Throws an `ArgumentException` if the stock item does not exist.

**Parameters:**
- `StockItem item`: The stock item to update.

#### `List<StockItem> GetAllStockItems()`

Retrieves all stock items from the database.

**Returns:**
- `List<StockItem>`: A list of all stock items.

#### `StockItem GetStockItem(string ISIN)`

Retrieves a stock item by its ISIN.

**Parameters:**
- `string ISIN`: The ISIN of the stock item to retrieve.

**Returns:**
- `StockItem`: The stock item with the specified ISIN.

## StockService

The `StockService` class is responsible for managing stock items. It provides methods to add test data, retrieve stock items based on certain criteria, and display stock items in the console.

## Public Methods

### `void MakeSomeTestWork()`
This method performs several operations to demonstrate the functionality of the `StockService`:
- Adds test data to the stock context.
- Retrieves and displays all stock items.
- Retrieves and displays stock items below a specified quantity.
- Retrieves and displays stock items below a specified price threshold.
- Retrieves and displays stock items that contain a specified name.

### `IEnumerable<StockItem> SearchStockItemsByName(IEnumerable<StockItem> stockItemList, string searchStockItemName)`
This method filters the stock items by name.
- `stockItemList`: The list of stock items to search.
- `searchStockItemName`: The name to search for in the stock items.

### `IEnumerable<StockItem> GetStockItemsBelowThreshold(IEnumerable<StockItem> stockItemList, decimal maxThreshold)`
This method filters the stock items by price threshold.
- `stockItemList`: The list of stock items to search.
- `maxThreshold`: The maximum price threshold.

### `IEnumerable<StockItem> GetStockItemsBelowQuantity(IEnumerable<StockItem> stockItemList, int maxQuantity)`
This method filters the stock items by quantity.
- `stockItemList`: The list of stock items to search.
- `maxQuantity`: The maximum quantity threshold.