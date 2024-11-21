namespace OrderManagementApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
