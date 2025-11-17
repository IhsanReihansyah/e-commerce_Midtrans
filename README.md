E-Commerce Midtrans
Project ini adalah website e-commerce sederhana yang saya buat menggunakan ASP.NET Core MVC.
Di dalamnya sudah ada fitur keranjang belanja, checkout, dan integrasi pembayaran memakai Midtrans Snap. Database yang digunakan adalah MySQL lewat phpMyAdmin (XAMPP).

A. Fitur Utama
- Menampilkan produk dari Database
<img width="1349" height="623" alt="image" src="https://github.com/user-attachments/assets/18840d53-c86a-4a1e-b66a-5ab2a882d88b" />
- Menambah produk ke keranjang (pakai session)
<img width="1343" height="613" alt="image" src="https://github.com/user-attachments/assets/919f6d42-f86b-4379-8909-4ce38ab9b7cf" />
<img width="1345" height="612" alt="image" src="https://github.com/user-attachments/assets/9c7d6b58-4512-4409-ac6d-34617e078957" />
- Checkout + redirect ke Midtrans Snap
<img width="1327" height="609" alt="image" src="https://github.com/user-attachments/assets/95caa5b5-fc26-40be-88c5-e000f80612d4" />
<img width="1340" height="584" alt="image" src="https://github.com/user-attachments/assets/43acdaf7-bdf0-4d87-a624-5742054b1c57" />
- Upload gambar produk/logo
<img width="1343" height="617" alt="image" src="https://github.com/user-attachments/assets/086f11c2-fe98-4ff2-b8ca-46e3f6fafb7f" />
- Penyimpanan order ke MySQL
<img width="1350" height="613" alt="image" src="https://github.com/user-attachments/assets/0791d66e-3936-4273-a0e1-b05a1418f2f2" />


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
  3. akses melalui browser sesuai dengan alamat localhost kamu, lalu tambah /Product (Contoh: http://localhost:5123/Product)
  <img width="691" height="189" alt="image" src="https://github.com/user-attachments/assets/cbe12ace-39bd-40fa-a62e-424b1f6104e5" />
