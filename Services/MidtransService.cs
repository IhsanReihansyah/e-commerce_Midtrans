using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace EshopMidtrans.Services
{
    /// <summary>
    /// Service untuk menghubungkan aplikasi dengan Midtrans Snap API.
    /// Bertanggung jawab membuat transaksi dan menerima redirect_url.
    /// </summary>
    public class MidtransService
    {
        private readonly IConfiguration _configuration; // Untuk mengambil konfigurasi dari appsettings.json
        private readonly HttpClient _httpClient;        // Untuk request API ke Midtrans

        public MidtransService(IConfiguration configuration)
        {
            // Menyimpan konfigurasi Midtrans (server key, environment, dll)
            _configuration = configuration;

            // Inisialisasi HttpClient
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Membuat transaksi Snap Midtrans.
        /// Mengembalikan redirect_url agar user bisa diarahkan ke halaman pembayaran Midtrans.
        /// </summary>
        public async Task<string> CreateTransaction(string orderId, decimal grossAmount)
        {
            // Mengecek apakah mode production atau sandbox (diambil dari appsettings.json)
            var isProduction = bool.Parse(_configuration["Midtrans:IsProduction"] ?? "false");

            // Mengambil ServerKey Midtrans dari konfigurasi
            var serverKey = _configuration["Midtrans:ServerKey"];

            // Menentukan endpoint API berdasarkan environment
            var url = isProduction
                ? "https://app.midtrans.com/snap/v1/transactions"          // Endpoint Production
                : "https://app.sandbox.midtrans.com/snap/v1/transactions"; // Endpoint Sandbox

            // Payload data transaksi yang dikirim ke Midtrans
            var payload = new
            {
                transaction_details = new
                {
                    order_id = orderId,           // ID unik transaksi
                    gross_amount = grossAmount    // Total pembayaran
                },
                credit_card = new
                {
                    secure = true                 // Mengaktifkan 3DS untuk keamanan transaksi
                },
                customer_details = new
                {
                    first_name = "Customer",      // Nama customer (bisa diganti dinamis)
                    email = "customer@example.com"
                },
                callbacks = new
                {
                    // URL callback setelah pembayaran selesai
                    finish = "https://localhost:5123/Cart/Success"
                }
            };

            // Serialisasi payload menjadi JSON
            var json = JsonSerializer.Serialize(payload);

            // Membuat request HTTP POST ke Midtrans
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            // Menambahkan Authorization Header (Basic Auth menggunakan Server Key)
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(serverKey + ":"))
                );

            // Menambahkan JSON payload ke request
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            // Mengirim request ke Midtrans
            var response = await _httpClient.SendAsync(request);

            // Membaca response sebagai string
            var content = await response.Content.ReadAsStringAsync();

            // Debug: Menampilkan response ke console untuk memastikan berhasil
            Console.WriteLine("MIDTRANS RESPONSE:");
            Console.WriteLine(content);

            // Parsing JSON response untuk mengambil redirect_url
            var doc = JsonDocument.Parse(content);

            // Jika berhasil mendapatkan redirect_url
            if (doc.RootElement.TryGetProperty("redirect_url", out var redirect))
            {
                return redirect.GetString() ?? "";
            }
            else
            {
                // Error jika Midtrans tidak memberikan redirect_url
                throw new Exception("Midtrans error: " + content);
            }
        }

    }
}
