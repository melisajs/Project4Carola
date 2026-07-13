# Carola — Car Rental & Reservation Platform

Carola; araç listeleme, gelişmiş filtreleme, uygunluk kontrolü, OCR destekli rezervasyon ve yönetim paneli özelliklerini bir araya getiren, katmanlı mimariyle geliştirilmiş bir **ASP.NET Core MVC** araç kiralama projesidir.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-5C2D91?style=for-the-badge&logo=dotnet)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-10.0-6DB33F?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL_Server-Database-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![SignalR](https://img.shields.io/badge/SignalR-Realtime-00A4EF?style=for-the-badge)

## Proje Özellikleri

### Müşteri Arayüzü

- Modern ve responsive ana sayfa
- Marka, kategori, şube, koltuk sayısı ve fiyat filtreleri
- Araç özellikleri ve günlük kiralama fiyatları
- Tarih aralığına göre anlık müsaitlik kontrolü
- Ayrı ve adım adım ilerleyen rezervasyon oluşturma sayfası
- Kimlik veya ehliyet görselinden **Tesseract.js ile OCR** doğrulaması
- Çakışan tarihleri engelleyen rezervasyon kontrolü
- `CAR-000000` formatında rezervasyon takip kodu
- Rezervasyon oluşturma ve detaylı onay ekranı
- Türkçe ve İngilizce dil desteği
- Hakkımızda ve iletişim sayfaları
- SignalR tabanlı canlı destek bileşeni

### Yönetim Paneli

- Özgün **Aurora Carbon** koyu yönetim teması
- Araç, marka, kategori ve şube yönetimi
- Slider ve site içeriği yönetimi
- Rezervasyon listeleme, onaylama ve iptal işlemleri
- Rezervasyon ve gelir istatistiklerini gösteren dashboard
- Son rezervasyonlar ve araç müsaitlik özetleri
- Bozuk dış görseller için güvenli yerel yedek görsel sistemi
- Mobil ve masaüstü ekranlara uyumlu yönetim arayüzü

### Rezervasyon ve E-posta Akışı

1. Müşteri araç ve tarih aralığını seçer.
2. Sistem aracın seçilen tarihlerdeki uygunluğunu kontrol eder.
3. Kimlik/ehliyet bilgileri OCR ile okunur ve doğrulanır.
4. Müşteri ile rezervasyon bilgileri SQL Server'a kaydedilir.
5. Rezervasyon takip kodu oluşturulur.
6. Yönetici rezervasyonu onaylayabilir veya iptal edebilir.
7. Onay sonrasında MailKit ile HTML e-posta gönderilir.

SMTP ayarları yapılmamışsa e-posta içeriği geliştirme ortamında HTML önizleme olarak `wwwroot/sent-emails` klasörüne kaydedilir. Bu klasör Git tarafından takip edilmez.

## Kullanılan Teknolojiler

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core 10
- Microsoft SQL Server
- Katmanlı mimari
- Repository ve Service katmanları
- Razor Views
- MailKit
- SignalR
- Tesseract.js OCR
- JavaScript / AJAX
- Bootstrap ve özel CSS tasarımı

## Proje Mimarisi

```text
Project4Carola/
├── CarolaEnditiyLayer/       # Veri modelleri ve entity sınıfları
├── Carola.DataAccessLayer/   # EF Core context ve repository implementasyonları
├── Carola.BusinessLayer/     # Servisler, iş kuralları ve doğrulamalar
├── Carola.WebUI/             # MVC controller, view, view model ve statik dosyalar
└── Carola.slnx               # Solution dosyası
```

Bağımlılık yönü:

```text
WebUI → BusinessLayer → DataAccessLayer → EntityLayer
```

## Kurulum

### Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server veya SQL Server Express
- Visual Studio 2026, Rider ya da VS Code

### Çalıştırma

```bash
git clone https://github.com/melisajs/Project4Carola.git
cd Project4Carola
dotnet restore
dotnet run --project Carola.WebUI
```

Uygulama ilk çalıştırmada veritabanını oluşturur ve başlangıç verilerini ekler. Varsayılan bağlantı SQL Server Express kullanır:

```text
Server=.\SQLEXPRESS;Database=CarolaRentDb;Trusted_Connection=True;TrustServerCertificate=True;
```

Farklı bir SQL Server kullanıyorsanız `Carola.DataAccessLayer/Concrete/CaraloContext.cs` içindeki bağlantı bilgisini kendi ortamınıza göre düzenleyin.

## SMTP Yapılandırması

Gerçek rezervasyon onay e-postaları için `Carola.WebUI/appsettings.json` dosyasındaki alanları doldurun:

```json
{
  "Smtp": {
    "Host": "smtp.example.com",
    "Port": 587,
    "User": "your-email@example.com",
    "Password": "your-app-password"
  }
}
```

> Güvenlik için gerçek parolaları repoya göndermeyin. User Secrets veya ortam değişkenleri kullanın.

## Temel Sayfalar

| Sayfa | Adres |
|---|---|
| Ana sayfa | `/` |
| Araçlar | `/Car/CarList` |
| Rezervasyon | `/Booking?carId=2` |
| Rezervasyon takip | `/Booking/Track` |
| Hakkımızda | `/Home/About` |
| İletişim | `/Home/Contact` |
| Yönetim paneli | `/AdminDashboard` |
| Rezervasyon yönetimi | `/AdminReservation` |

## Dil Desteği

Arayüz Türkçe ve İngilizce olarak kullanılabilir. Dil seçimi; sabit metinlerin yanı sıra kategori, yakıt türü, vites türü, konum ve rezervasyon durumu gibi dinamik verileri de kapsar.

## Geliştirici

**Melisa** — [GitHub](https://github.com/melisajs)

