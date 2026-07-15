using System;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Carola.WebUI.Services
{
    public class EmailService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public EmailService(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        public async Task<bool> SendReservationApprovalEmailAsync(
            string recipientEmail, 
            string customerName, 
            string carModel, 
            string pickupLocation, 
            string returnLocation, 
            DateTime pickupDate, 
            DateTime returnDate,
            decimal totalPrice,
            string reservationCode)
        {
            var couponSvg = "<svg xmlns='http://www.w3.org/2000/svg' width='900' height='330'><defs><linearGradient id='g' x1='0' x2='1'><stop stop-color='#0c1729'/><stop offset='1' stop-color='#ff2e54'/></linearGradient></defs><rect width='900' height='330' rx='28' fill='url(#g)'/><text x='55' y='90' fill='white' font-family='Arial' font-size='34' font-weight='700'>CAROLA ÖZEL TEKLİF</text><text x='55' y='205' fill='#ffb75d' font-family='Arial' font-size='100' font-weight='800'>%30 İNDİRİM</text><text x='590' y='250' fill='white' font-family='Arial' font-size='30' font-weight='700'>KOD: CAR30</text></svg>";
            var couponDataUri = "data:image/svg+xml;base64," + Convert.ToBase64String(Encoding.UTF8.GetBytes(couponSvg));

            // Prepare beautiful HTML template matching the screenshot exactly
            string htmlContent = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8' />
                <style>
                    body {{
                        font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;
                        background-color: #f6f9fc;
                        margin: 0;
                        padding: 20px;
                        color: #333333;
                    }}
                    .container {{
                        max-width: 600px;
                        background-color: #ffffff;
                        margin: 0 auto;
                        border-radius: 8px;
                        box-shadow: 0 4px 12px rgba(0,0,0,0.08);
                        overflow: hidden;
                        border: 1px solid #e6ebf1;
                    }}
                    .header {{
                        background-color: #0c1729;
                        padding: 30px;
                        text-align: center;
                        color: #ffffff;
                    }}
                    .header h1 {{
                        margin: 0;
                        font-size: 24px;
                        font-weight: 700;
                    }}
                    .header p {{
                        margin: 10px 0 0 0;
                        font-size: 14px;
                        color: #ffb75d;
                        letter-spacing: 1px;
                        text-transform: uppercase;
                    }}
                    .content {{
                        padding: 30px;
                    }}
                    .greeting {{
                        font-size: 16px;
                        font-weight: bold;
                        margin-bottom: 15px;
                    }}
                    .intro {{
                        font-size: 14px;
                        line-height: 1.6;
                        color: #525f7f;
                        margin-bottom: 25px;
                    }}
                    .details-box {{
                        background-color: #f8fafc;
                        border: 1px solid #e2e8f0;
                        border-radius: 8px;
                        padding: 20px;
                        margin-bottom: 25px;
                    }}
                    .details-title {{
                        font-weight: bold;
                        font-size: 15px;
                        margin-bottom: 12px;
                        color: #0c1729;
                        border-bottom: 1px solid #e2e8f0;
                        padding-bottom: 6px;
                    }}
                    .detail-row {{
                        display: flex;
                        justify-content: space-between;
                        margin-bottom: 8px;
                        font-size: 14px;
                    }}
                    .detail-label {{
                        color: #64748b;
                        font-weight: 500;
                    }}
                    .detail-value {{
                        color: #0f172a;
                        font-weight: 600;
                    }}
                    .promo-section {{
                        text-align: center;
                        margin-top: 30px;
                        margin-bottom: 30px;
                    }}
                    .promo-image {{
                        max-width: 100%;
                        border-radius: 8px;
                        box-shadow: 0 4px 8px rgba(0,0,0,0.05);
                    }}
                    .coupon-box {{
                        background-color: #0c1729;
                        color: #ffffff;
                        padding: 20px;
                        border-radius: 8px;
                        text-align: center;
                        margin-bottom: 25px;
                    }}
                    .coupon-title {{
                        font-size: 18px;
                        font-weight: 700;
                        margin-bottom: 5px;
                        color: #ffb75d;
                    }}
                    .coupon-text {{
                        font-size: 13px;
                        color: #94a3b8;
                        margin-bottom: 15px;
                    }}
                    .coupon-code {{
                        display: inline-block;
                        background-color: #ffffff;
                        color: #0c1729;
                        font-weight: bold;
                        font-size: 16px;
                        padding: 8px 24px;
                        border-radius: 4px;
                        border: 1px dashed #ffb75d;
                        letter-spacing: 1px;
                    }}
                    .footer {{
                        text-align: center;
                        font-size: 12px;
                        color: #94a3b8;
                        padding: 20px;
                        border-top: 1px solid #f1f5f9;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>🚗 Rezervasyonunuz Onaylandı</h1>
                        <p>Özel Teklif Sizin İçin Hazır!</p>
                    </div>
                    <div class='content'>
                        <div class='greeting'>Sayın {customerName},</div>
                        <div class='intro'>
                            Rezervasyonunuz başarıyla onaylandı. Kiralama işleminize ait detayları aşağıda bulabilirsiniz:
                        </div>
                        
                        <div class='details-box'>
                            <div class='details-title'>🚘 Araç Bilgileri</div>
                            <div class='detail-row'>
                                <span class='detail-label'>Araç:</span>
                                <span class='detail-value'>{carModel}</span>
                            </div>
                            <div class='detail-row'>
                                <span class='detail-label'>Alış Şubesi:</span>
                                <span class='detail-value'>{pickupLocation}</span>
                            </div>
                            <div class='detail-row'>
                                <span class='detail-label'>İade Şubesi:</span>
                                <span class='detail-value'>{returnLocation}</span>
                            </div>
                            <div class='detail-row'>
                                <span class='detail-label'>Tarih Aralığı:</span>
                                <span class='detail-value'>{pickupDate:dd.MM.yyyy} - {returnDate:dd.MM.yyyy}</span>
                            </div>
                            <div class='detail-row'>
                                <span class='detail-label'>Toplam Tutar:</span>
                                <span class='detail-value'>₺{totalPrice:N2}</span>
                            </div>
                            <div class='detail-row'>
                                <span class='detail-label'>Rezervasyon Kodu:</span>
                                <span class='detail-value'>{reservationCode}</span>
                            </div>
                        </div>

                        <div class='promo-section'>
                            <!-- In production, this can point to a public server image. We point to our local asset layout -->
                            <img src='{couponDataUri}' class='promo-image' alt='Carola yüzde 30 indirim kuponu' />
                        </div>

                        <div class='coupon-box'>
                            <div class='coupon-title'>%30 İNDİRİM</div>
                            <div class='coupon-text'>Bir sonraki kiralamanızda kullanın</div>
                            <div class='coupon-code'>KUPON: CAR30</div>
                        </div>
                    </div>
                    <div class='footer'>
                        Bu kuponu bir sonraki rezervasyonunuzda kullanarak %30 indirim kazanabilirsiniz.<br/>
                        İyi yolculuklar dileriz 🚗<br/>
                        <strong>Carola Car Rental Ekibi</strong>
                    </div>
                </div>
            </body>
            </html>
            ";

            var smtpHost = _configuration["Smtp:Host"];
            var smtpUser = _configuration["Smtp:User"];
            var smtpPassword = _configuration["Smtp:Password"];
            var smtpPort = int.TryParse(_configuration["Smtp:Port"], out var configuredPort) ? configuredPort : 587;

            // Send with configured SMTP. In development, preserve an exact HTML preview locally.
            try
            {
                if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPassword))
                {
                    return await SavePreviewAsync(customerName, htmlContent, "SMTP ayarları yapılandırılmadı.");
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Carola Car Rental", smtpUser));
                message.To.Add(new MailboxAddress(customerName, recipientEmail));
                message.Subject = "Rezervasyonunuz Onaylandı - Özel Teklif Sizin İçin Hazır!";
                message.Body = new BodyBuilder { HtmlBody = htmlContent }.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(smtpUser, smtpPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return await SavePreviewAsync(customerName, htmlContent, ex.Message);
            }
        }

        private async Task<bool> SavePreviewAsync(string customerName, string htmlContent, string reason)
        {
            try
            {
                var emailsDir = Path.Combine(_env.WebRootPath, "sent-emails");
                Directory.CreateDirectory(emailsDir);
                var safeName = string.Concat(customerName.Select(ch => char.IsLetterOrDigit(ch) ? ch : '_'));
                var filePath = Path.Combine(emailsDir, $"reservation-{safeName}-{DateTime.Now:yyyyMMddHHmmss}.html");
                await File.WriteAllTextAsync(filePath, htmlContent, Encoding.UTF8);
                Console.WriteLine($"[EMAIL PREVIEW] {reason} Önizleme: {filePath}");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
