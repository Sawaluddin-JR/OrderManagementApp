namespace OrderManagementApp.Models
{
    public class OrderListModel
    {
        public string Keyword { get; set; }
        public DateTime? OrderDate { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
