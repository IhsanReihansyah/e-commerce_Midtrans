using EshopMidtrans.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);


builder.Services.AddControllersWithViews();

// Agar MidtransService bisa digunakan lewat Dependency Injection
builder.Services.AddScoped<EshopMidtrans.Services.MidtransService>();

// Session dipakai untuk menyimpan Cart (Keranjang Belanja)
builder.Services.AddSession();

var app = builder.Build();

// ERROR HANDLING UNTUK MODE PRODUCTION
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Aktifkan HSTS untuk keamanan HTTPS
}

// Redirect HTTP → HTTPS
app.UseHttpsRedirection();

// Routing untuk controller/action
app.UseRouting();

// Authorization middleware (kalau pakai login)
app.UseAuthorization();

// Aktifkan akses file statis (CSS, JS, Images)
app.MapStaticAssets();
app.UseStaticFiles();

// Aktifkan session (HARUS sebelum MapControllerRoute)
app.UseSession();

// Setting routing utama aplikasi → Home/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// JALANKAN APLIKASI
app.Run();
