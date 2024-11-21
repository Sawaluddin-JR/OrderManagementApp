using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OrderManagementApp.Models;

namespace OrderManagementApp.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetOrders(string keyword, DateTime? orderDate)
        {
            var query = _context.Orders.Include(o => o.Items).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(o => o.OrderNumber.Contains(keyword));

            if (orderDate.HasValue)
                query = query.Where(o => o.OrderDate.Date == orderDate.Value.Date);

            return query.ToList();
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(int orderId)
        {
            var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        public void UpdateOrder(Order order)
        {
            var existingOrder = _context.Orders.Include(o => o.Items)
                                               .FirstOrDefault(o => o.OrderId == order.OrderId);

            if (existingOrder != null)
            {
                existingOrder.OrderNumber = order.OrderNumber;
                existingOrder.OrderDate = order.OrderDate;

                // Update items
                _context.Items.RemoveRange(existingOrder.Items);
                _context.Items.AddRange(order.Items);

                _context.SaveChanges();
            }
        }

        public byte[] ExportOrders(string keyword, DateTime? orderDate)
        {
            var orders = GetOrders(keyword, orderDate);

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Orders");

            // Header
            worksheet.Cells[1, 1].Value = "Order Number";
            worksheet.Cells[1, 2].Value = "Order Date";

            int row = 2;
            foreach (var order in orders)
            {
                worksheet.Cells[row, 1].Value = order.OrderNumber;
                worksheet.Cells[row, 2].Value = order.OrderDate.ToShortDateString();
                row++;
            }

            return package.GetAsByteArray();
        }

    }
}
