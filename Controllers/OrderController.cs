using Microsoft.AspNetCore.Mvc;
using EshopMidtrans.Data;
using System.Linq;

namespace EshopMidtrans.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Order
        public IActionResult Index(string status)
        {
            var orders = _context.Orders.AsQueryable();

            // Jika ada parameter status, filter berdasarkan itu
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                orders = orders.Where(o => o.PaymentStatus == status);
            }

            ViewData["SelectedStatus"] = status ?? "All";
            return View(orders.ToList());
        }
        
        // GET: /Order/Checkout
        public IActionResult Checkout()
        {
            return View();
        }

        // POST: /Order/Checkout
        [HttpPost]
        public IActionResult Checkout(string name, string email, string address, string phone, decimal amount)
        {
            // Simpan data order ke database
            var order = new EshopMidtrans.Models.Order
            {
                OrderId = Guid.NewGuid().ToString("N").Substring(0, 8),
                CustomerName = name,
                CustomerEmail = email,
                CustomerAddress = address,
                CustomerPhone = phone,
                Amount = amount,
                PaymentStatus = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Redirect ke halaman pembayaran Midtrans (misalnya Snap)
            return RedirectToAction("Pay", "Payment", new { orderId = order.OrderId });
        }

    }
}
