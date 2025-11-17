using EshopMidtrans.Models;
using Microsoft.EntityFrameworkCore;

namespace EshopMidtrans.Data
{
    /// Kelas AppDbContext sebagai penghubung antara aplikasi dan database
    /// menggunakan Entity Framework Core.
    public class AppDbContext : DbContext
    {
        /// Konstruktor DbContext yang menerima konfigurasi dari dependency injection.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet mewakili tabel di database
        public DbSet<Product> Products { get; set; }  // Tabel untuk produk
        public DbSet<Order> Orders { get; set; }      // Tabel untuk order / transaksi

        /// Mengonfigurasi model sebelum database dibuat.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.DeletedAt == null);
        }
    }
}
