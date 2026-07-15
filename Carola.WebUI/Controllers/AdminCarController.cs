using Carola.BusinessLayer.Abstract;
using CarolaEnditiyLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Carola.WebUI.Controllers
{
    public class AdminCarController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly ILocationService _locationService;

        public AdminCarController(
            ICarService carService,
            IBrandService brandService,
            ICategoryService categoryService,
            ILocationService locationService)
        {
            _carService = carService;
            _brandService = brandService;
            _categoryService = categoryService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _carService.TGetAllAsync();
            var brands = await _brandService.TGetAllAsync();
            var categories = await _categoryService.TGetAllAsync();
            var locations = await _locationService.TGetAllAsync();

            ViewBag.Brands = brands;
            ViewBag.Categories = categories;
            ViewBag.Locations = locations;
            ViewBag.TotalCars = cars.Count;
            ViewBag.AvailableCars = cars.Count(c => c.IsAvailable);
            ViewBag.UnavailableCars = cars.Count(c => !c.IsAvailable);
            ViewBag.AverageDailyPrice = cars.Count > 0 ? cars.Average(c => c.DailyPrice) : 0;

            var viewModel = cars.Select(c => new {
                Car = c,
                BrandName = brands.FirstOrDefault(b => b.BrandId == c.BrandId)?.BrandName ?? "Marka Yok",
                CategoryName = categories.FirstOrDefault(cat => cat.CarCategoryId == c.CategoryId)?.CategoryName ?? "Kategori Yok",
                LocationName = locations.FirstOrDefault(l => l.LocationId == c.LocationId)?.LocationName ?? "Şube Yok"
            }).ToList();

            ViewBag.CarsList = viewModel;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            car.IsAvailable = true; // By default new car is available
            await _carService.TInsertAsync(car);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Car car)
        {
            await _carService.TUpdateAsync(car);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _carService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
