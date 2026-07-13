using Carola.BusinessLayer.Abstract;
using Carola.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Carola.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly ICategoryService _categoryService;
        private readonly IWeAreTheBestService _weAreTheBestService;
        private readonly ICarService _carService;
        private readonly IBrandService _brandService;
        private readonly ILocationService _locationService;
        private readonly IWebHostEnvironment _environment;

        public HomeController(
            ISliderService sliderService,
            ICategoryService categoryService,
            IWeAreTheBestService weAreTheBestService,
            ICarService carService,
            IBrandService brandService,
            ILocationService locationService,
            IWebHostEnvironment environment)
        {
            _sliderService = sliderService;
            _categoryService = categoryService;
            _weAreTheBestService = weAreTheBestService;
            _carService = carService;
            _brandService = brandService;
            _locationService = locationService;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _sliderService.TGetAllAsync();
            var categories = await _categoryService.TGetAllAsync();
            var weAreBests = await _weAreTheBestService.TGetAllAsync();
            var cars = await _carService.TGetAllAsync();
            var brands = await _brandService.TGetAllAsync();
            var locations = await _locationService.TGetAllAsync();

            var viewModel = new HomeIndexViewModel
            {
                Sliders = sliders.Where(s => s.Status).ToList(),
                Categories = categories,
                WeAreTheBest = weAreBests.FirstOrDefault() ?? new CarolaEnditiyLayer.Entities.WeAreTheBest
                {
                    Title = "Explore The World With Your Own Way Of Driving",
                    Description = "We offer the best car rental services with clean vehicles, round-the-clock support, and convenient branch pick-ups.",
                    Feature1Title = "Free Pick Up & Drop",
                    Feature1Description = "Your convenience matter. Complimentary pick-up and drop-off service for any your vehicle, a stress-free experience.",
                    Feature2Title = "24/7 Road Assistant",
                    Feature2Description = "No matter the time or place, and our 24/7 roadside assistance ensures you're never stranded. Drive confidently with support."
                },
                // Get last 6 cars
                LatestCars = cars.OrderByDescending(c => c.CarId).Take(6).ToList(),
                Brands = brands.Where(b => b.Status).ToList(),
                Branches = locations
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> About()
        {
            var stories = await _weAreTheBestService.TGetAllAsync();
            var cars = await _carService.TGetAllAsync();
            var brands = await _brandService.TGetAllAsync();
            var branches = await _locationService.TGetAllAsync();

            return View(new AboutPageViewModel
            {
                Story = stories.FirstOrDefault() ?? new CarolaEnditiyLayer.Entities.WeAreTheBest
                {
                    Title = "Her yolculuk güvenle başlar",
                    Description = "Carola; şeffaf fiyatlandırma, bakımlı araçlar ve ulaşılabilir destek anlayışıyla araç kiralamayı kolaylaştırır.",
                    Feature1Title = "Esnek Teslimat",
                    Feature1Description = "Aracınızı size en uygun Carola şubesinden teslim alın ve güvenle yola çıkın.",
                    Feature2Title = "7/24 Yol Desteği",
                    Feature2Description = "Yolculuğunuz boyunca ihtiyaç duyduğunuz her anda destek ekibimize ulaşın."
                },
                CarCount = cars.Count,
                BrandCount = brands.Count(b => b.Status),
                BranchCount = branches.Count
            });
        }

        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            return View(new ContactPageViewModel
            {
                Branches = await _locationService.TGetAllAsync()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Branches = await _locationService.TGetAllAsync();
                return View(model);
            }

            var messageDirectory = Path.Combine(_environment.ContentRootPath, "App_Data", "contact-messages");
            Directory.CreateDirectory(messageDirectory);

            var message = new
            {
                model.FullName,
                model.Email,
                model.Phone,
                model.Subject,
                model.Message,
                CreatedAt = DateTimeOffset.Now
            };
            var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid():N}.json";
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(messageDirectory, fileName),
                JsonSerializer.Serialize(message, new JsonSerializerOptions { WriteIndented = true }));

            TempData["ContactSuccess"] = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "en"
                ? "We received your message. Our team will contact you shortly."
                : "Mesajınız bize ulaştı. Ekibimiz en kısa sürede sizinle iletişime geçecek.";
            return RedirectToAction(nameof(Contact));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
