namespace peasysdemo.Models
{
    public class ProductsViewModel
    {
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Stock { get; set; }
        public long EAN { get; set; }
        public int QuantityAsked { get; set; }
        public int Sales { get; set; }
        public string Supplier { get; set; }
    }
}