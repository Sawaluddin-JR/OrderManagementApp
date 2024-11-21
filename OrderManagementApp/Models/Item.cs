namespace OrderManagementApp.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Order Order { get; set; }
    }
}
