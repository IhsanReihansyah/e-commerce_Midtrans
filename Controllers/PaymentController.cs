using Microsoft.AspNetCore.Mvc;
using EshopMidtrans.Data;
using EshopMidtrans.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EshopMidtrans.Controllers
{
    [Route("payment")]
    public class PaymentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(AppDbContext context, ILogger<PaymentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("notification")]
        public async Task<IActionResult> Notification([FromBody] JsonElement payload)
        {
            try
            {
                var orderId = payload.GetProperty("order_id").GetString();
                var transactionStatus = payload.GetProperty("transaction_status").GetString();
                var fraudStatus = payload.GetProperty("fraud_status").GetString();

                _logger.LogInformation($"🔔 Notifikasi diterima: Order {orderId}, Status {transactionStatus}, Fraud {fraudStatus}");

                var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
                if (order == null)
                {
                    _logger.LogWarning($"Order {orderId} tidak ditemukan di database");
                    return NotFound();
                }

                if (transactionStatus == "capture" || transactionStatus == "settlement")
                {
                    order.PaymentStatus = "paid";
                }
                else if (transactionStatus == "pending")
                {
                    order.PaymentStatus = "pending";
                }
                else
                {
                    order.PaymentStatus = "failed";
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saat memproses notifikasi Midtrans");
                return BadRequest();
            }
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            return Content("Pembayaran berhasil! Terima kasih sudah berbelanja.");
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            return Content("Pembayaran gagal atau dibatalkan.");
        }
    }
}
