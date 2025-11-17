using EshopMidtrans.Data;
using EshopMidtrans.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using EshopMidtrans.Services;

namespace EshopMidtrans.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly MidtransService _midtransService; // Service untuk integrasi Midtrans
        private const string CART_KEY = "CART_SESSION";   // Key session untuk menyimpan keranjang

        // Constructor DI untuk mengambil database + Midtrans Service
        public CartController(AppDbContext context, MidtransService midtransService)
        {
            _context = context;
            _midtransService = midtransService;
        }

        // GET: /Cart
        // Menampilkan isi keranjang
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // GET: /Cart/Checkout
        // Menampilkan halaman checkout
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                return RedirectToAction("Index");
            }

            // Kirim data ke View melalui ViewBag
            ViewBag.Cart = cart;
            ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);

            return View();
        }

        // POST: /Cart/Checkout
        // Proses checkout dan buat transaksi Midtrans
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var total = (int)cart.Sum(x => x.Price * x.Quantity);
                var orderId = Guid.NewGuid().ToString(); // ID unik order

                // Simpan ke database
                order.OrderId = orderId;
                order.Amount = total;
                order.PaymentStatus = "Pending";
                order.CreatedAt = DateTime.Now;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // 🔗 Buat transaksi Midtrans
                var paymentUrl = await _midtransService.CreateTransaction(orderId, total);

                // Hapus keranjang dari session
                HttpContext.Session.Remove(CART_KEY);

                // Redirect ke Midtrans Payment Page
                return Redirect(paymentUrl);
            }

            return View(order);
        }

        // Tambah produk ke cart dengan ukuran
        [HttpPost]
        public IActionResult AddToCartWithSize(int productId, string size, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                return NotFound();

            var cart = GetCart();

            // Cek jika item yang sama + ukuran sama sudah ada
            var existing = cart.FirstOrDefault(c =>
                c.ProductId == productId &&
                c.Size == size
            );

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    Size = size,
                    ImageUrl = product.ImageUrl
                });
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // Checkout langsung dari halaman produk
        [HttpPost]
        public IActionResult CheckoutNow(int ProductId, string Size, int Quantity)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == ProductId);
            if (product == null) return NotFound();

            // Buat cart 1 item langsung untuk checkout
            var cart = new List<CartItem>();

            cart.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = Quantity,
                Size = Size
            });

            SaveCart(cart);

            return RedirectToAction("Checkout", "Cart");
        }

        // Proses checkout manual (tanpa model order dari form)
        [HttpPost]
        public async Task<IActionResult> ProcessCheckout(string CustomerName, string CustomerEmail, string CustomerAddress, string CustomerPhone)
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                return RedirectToAction("Index");
            }

            var total = (int)cart.Sum(x => x.Price * x.Quantity);
            var orderId = Guid.NewGuid().ToString();

            // Simpan order ke database
            var order = new Order
            {
                OrderId = orderId,
                Amount = total,
                PaymentStatus = "pending",
                CustomerName = CustomerName,
                CustomerEmail = CustomerEmail,
                CustomerAddress = CustomerAddress,
                CustomerPhone = CustomerPhone
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 🔗 Buat transaksi Midtrans
            var paymentUrl = await _midtransService.CreateTransaction(orderId, total);

            // Hapus isi keranjang setelah order
            SaveCart(new List<CartItem>());

            return Redirect(paymentUrl);
        }

        // Halaman setelah pembayaran
        public IActionResult Success() => View();
        public IActionResult Pending() => View();
        public IActionResult Failed() => View();

        // Tambah produk ke cart (versi biasa)
        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name ?? string.Empty,
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl ?? string.Empty
                });
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // Tambah produk ke cart via AJAX
        [HttpPost]
        public IActionResult AddToCartAjax(int id, int qty = 1)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (existingItem != null)
                existingItem.Quantity += qty;
            else
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name ?? string.Empty,
                    Price = product.Price,
                    Quantity = qty,
                    ImageUrl = product.ImageUrl ?? string.Empty
                });

            SaveCart(cart);

            // Kirim respon JSON
            return Json(new
            {
                success = true,
                totalItems = cart.Sum(c => c.Quantity),
                message = $"{product.Name} berhasil ditambahkan ke keranjang."
            });
        }

        // Hapus item dari cart
        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        // Helper untuk mengambil cart dari session
        private List<CartItem> GetCart()
        {
            var sessionData = HttpContext.Session?.GetString(CART_KEY);
            if (string.IsNullOrEmpty(sessionData))
                return new List<CartItem>();

            try
            {
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<CartItem>>(sessionData, opts) ?? new List<CartItem>();
            }
            catch
            {
                return new List<CartItem>();
            }
        }

        // Helper untuk menyimpan cart ke session
        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            HttpContext.Session?.SetString(CART_KEY, json);
        }
    }
}
