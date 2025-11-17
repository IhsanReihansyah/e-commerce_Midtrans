# Tahap 1: Build (Menggunakan SDK untuk kompilasi)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
# Salin semua file proyek ke /src
COPY . .
# Ganti working directory ke /src, tempat EshopMidtrans.csproj berada
WORKDIR "/src" 

# Publikasi aplikasi
RUN dotnet publish "EshopMidtrans.csproj" -c Release -o /app/publish

# Tahap 2: Final (Menggunakan Runtime yang lebih kecil untuk menjalankan aplikasi)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8071 
# Salin hasil publish dari tahap build
COPY --from=build /app/publish .
# Perintah untuk menjalankan aplikasi
ENTRYPOINT ["dotnet", "EshopMidtrans.dll"]