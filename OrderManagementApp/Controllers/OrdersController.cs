using Microsoft.AspNetCore.Mvc;
using OrderManagementApp.Models;
using OrderManagementApp.Services;

namespace OrderManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var orders = _orderService.GetOrders(null, null);
            var model = new OrderListModel
            {
                Orders = orders,
                Keyword = null,
                OrderDate = null
            };
            return View(model); // Mengirimkan model yang benar ke View
        }


        [HttpGet("list")]
        public IActionResult GetOrders([FromQuery] string keyword, [FromQuery] DateTime? orderDate)
        {
            var orders = _orderService.GetOrders(keyword, orderDate);

            var model = new OrderListModel
            {
                Keyword = keyword,
                OrderDate = orderDate,
                Orders = orders // Set daftar pesanan ke dalam Orders
            };

            return Ok(model);
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            _orderService.AddOrder(order);
            return CreatedAtAction(nameof(GetOrders), new { id = order.OrderId }, order);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            _orderService.DeleteOrder(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] Order order)
        {
            if (id != order.OrderId) return BadRequest();

            _orderService.UpdateOrder(order);
            return NoContent();
        }

        [HttpGet("export")]
        public IActionResult ExportOrders([FromQuery] string keyword, [FromQuery] DateTime? orderDate)
        {
            var fileContent = _orderService.ExportOrders(keyword, orderDate);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders.xlsx");
        }

    }
}
