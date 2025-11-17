E-Commerce Midtrans
Project ini adalah website e-commerce sederhana yang saya buat menggunakan ASP.NET Core MVC.
Di dalamnya sudah ada fitur keranjang belanja, checkout, dan integrasi pembayaran memakai Midtrans Snap. Database yang digunakan adalah MySQL lewat phpMyAdmin (XAMPP).

A. Fitur Utama
- Menampilkan produk dari Database
- Menambah produk ke keranjang (pakai session)
- Checkout + redirect ke Midtrans Snap
- Upload gambar produk/logo
- Penyimpanan order ke MySQL

B. Cara Menjalankannya
- Siapkan Database
- Jalankan XAMPP
- hidupkan MySQL.
- Masuk phpMyAdmin.
- Import file database: eshop.sql (sudah ada di repo ini).
- periksa koneksi database ada pada file appsettings.json ("DefaultConnection": "Server=127.0.0.1;Port=3306;Database=eshop;User=root;Password=;")
- Buka terminal di folder project lalu:
  1. dotnet restore
  2. dotnet run
  3. akses melalui browser sesuai dengan alamat localhost kamu +/Product (Contoh: http://localhost:5123/Product)
  <img width="691" height="189" alt="image" src="https://github.com/user-attachments/assets/cbe12ace-39bd-40fa-a62e-424b1f6104e5" />
