namespace StockManagement.Model
{
    public class StockItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ISIN { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
