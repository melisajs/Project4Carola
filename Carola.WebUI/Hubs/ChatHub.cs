using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using Carola.BusinessLayer.Abstract;
using System.Text.RegularExpressions;

namespace Carola.WebUI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;

        public ChatHub(ICarService carService, ICategoryService categoryService)
        {
            _carService = carService;
            _categoryService = categoryService;
        }

        public async Task SendMessage(string user, string message)
        {
            // Eco the user message back to the sender
            await Clients.Caller.SendAsync("ReceiveMessage", "User", message);

            // Generate an AI / Smart response
            string botResponse = await GenerateBotResponseAsync(message);

            // Send bot response to the sender after a small delay to simulate typing
            await Task.Delay(800);
            await Clients.Caller.SendAsync("ReceiveMessage", "Assistant", botResponse);
        }

        private async Task<string> GenerateBotResponseAsync(string message)
        {
            string query = message.ToLower();
            var cars = await _carService.TGetAllAsync();
            var categories = await _categoryService.TGetAllAsync();

            // 1. Scenario: Family car recommendation (e.g., "anne, baba ve 10 yaşında çocuk", "aile", "geniş")
            if (query.Contains("aile") || query.Contains("çocuk") || query.Contains("anne") || query.Contains("baba") || query.Contains("geniş") || query.Contains("bagaj"))
            {
                var familyCars = cars.Where(c => c.SeatCount >= 5 && c.LuggageCapacity >= 350).ToList();
                if (familyCars.Any())
                {
                    string carListStr = string.Join("<br/>", familyCars.Select(c => $"• <strong>{c.Model}</strong> ({c.FuelType}, {c.TransmissionType}) - Günlük ₺{c.DailyPrice:N0}"));
                    return $"Merhaba! Aileniz ve 10 yaşındaki çocuğunuzla konforlu bir seyahat için geniş bagaj hacmine ve en az 5 koltuk kapasitesine sahip araçlarımızı öneririm. Filomuzda sizin için en uygun seçenekler şunlardır:<br/><br/>{carListStr}<br/><br/>Özellikle geniş bagajı ve yüksek konforu nedeniyle SUV veya geniş sedan modellerimiz aileniz için harika bir seçim olacaktır. Rezervasyon sayfamızdan dilediğinizi seçebilirsiniz!";
                }
                return "Merhaba! Ailenizle yapacağınız seyahatler için geniş bagaj hacmine sahip 5 kişilik sedan veya SUV modellerimizi tercih edebilirsiniz. Şu an filomuzda bu kriterlere uygun tüm araçlar rezerve edilmiş durumda olabilir.";
            }

            // 2. Scenario: Economic car recommendation (e.g., "ekonomik", "ucuz", "yakıt", "az yakan", "şehir içi")
            if (query.Contains("ekonomik") || query.Contains("ucuz") || query.Contains("yakıt") || query.Contains("az yakan") || query.Contains("şehir içi"))
            {
                var ecoCars = cars.Where(c => c.DailyPrice <= 2000 || c.FuelType == "Dizel" || c.FuelType == "Hibrit").OrderBy(c => c.DailyPrice).ToList();
                if (ecoCars.Any())
                {
                    string carListStr = string.Join("<br/>", ecoCars.Select(c => $"• <strong>{c.Model}</strong> ({c.FuelType}, {c.TransmissionType}) - Günlük ₺{c.DailyPrice:N0}"));
                    return $"Merhaba! Şehir içi kullanım için yakıt cimrisi ve bütçe dostu araçlarımızı öneririm. Filomuzdaki en ekonomik ve düşük yakıt tüketen seçenekler şunlardır:<br/><br/>{carListStr}<br/><br/>Bu araçlar hem park kolaylığı hem de düşük yakıt tüketimi ile şehir içi sürüşleriniz için mükemmeldir.";
                }
            }

            // 3. Scenario: Sport / Premium car recommendation (e.g., "spor", "premium", "hızlı", "porsche", "bmw", "mercedes")
            if (query.Contains("spor") || query.Contains("premium") || query.Contains("hızlı") || query.Contains("porsche") || query.Contains("bmw") || query.Contains("mercedes") || query.Contains("prestij"))
            {
                var sportCars = cars.Where(c => c.CategoryId == 4 || c.DailyPrice >= 2500).ToList();
                if (sportCars.Any())
                {
                    string carListStr = string.Join("<br/>", sportCars.Select(c => $"• <strong>{c.Model}</strong> ({c.FuelType}, {c.TransmissionType}) - Günlük ₺{c.DailyPrice:N0}"));
                    return $"Merhaba! Yüksek performans, prestij ve sürüş keyfi arıyorsanız, premium spor segmentindeki araçlarımızı öneririm. Aktif seçeneklerimiz:<br/><br/>{carListStr}<br/><br/>Bu araçlarımız özel günleriniz ve üst düzey sürüş konforu deneyimi için özel olarak hazırlanmaktadır.";
                }
            }

            // 4. Default Fallback response (Generates helpful info like a support agent)
            return "Merhaba! Carola Müşteri Temsilcisiyim. Size nasıl yardımcı olabilirim? Araç önerisi almak için seyahat edecek kişi sayısını, bütçe tercihlerinizi veya istediğiniz vites/yakıt tipini belirtebilirsiniz. (Örn: 'Aile için araç önerisi' veya 'Ekonomik araçlar hangileri?') Gold, prestij ve konforlu sürüş için buradayım.";
        }
    }
}
