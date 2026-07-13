using Carola.BusinessLayer.Abstract;
using Carola.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Carola.WebUI.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ILocationService _locationService;
        private readonly IReservationService _reservationService;

        public CarController(
            ICarService carService,
            ICategoryService categoryService,
            IBrandService brandService,
            ILocationService locationService,
            IReservationService reservationService)
        {
            _carService = carService;
            _categoryService = categoryService;
            _brandService = brandService;
            _locationService = locationService;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> CarList(
            int? categoryId,
            int? brandId,
            int? locationId,
            DateTime? startDate,
            DateTime? endDate,
            int? seatCount,
            decimal? minPrice,
            decimal? maxPrice,
            string searchTerm,
            int page = 1)
        {
            var cars = await _carService.TGetAllAsync();
            var categories = await _categoryService.TGetAllAsync();
            var brands = await _brandService.TGetAllAsync();
            var branches = await _locationService.TGetAllAsync();
            var reservations = await _reservationService.TGetAllAsync();

            var carsQuery = cars.Where(c => c.IsAvailable).AsQueryable();

            // Filter 1: Date availability check (collision prevention)
            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate.Value.Date < DateTime.Today || endDate.Value.Date <= startDate.Value.Date)
                {
                    ModelState.AddModelError(string.Empty, "Lütfen geçerli bir alış ve iade tarihi seçin.");
                    carsQuery = Enumerable.Empty<CarolaEnditiyLayer.Entities.Car>().AsQueryable();
                }

                var overlappingCarIds = await _reservationService
                    .TGetUnavailableCarIdsAsync(startDate.Value, endDate.Value);

                carsQuery = carsQuery.Where(c => !overlappingCarIds.Contains(c.CarId));
            }

            // Filter 2: Category
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                carsQuery = carsQuery.Where(c => c.CategoryId == categoryId.Value);
            }

            // Filter 3: Brand
            if (brandId.HasValue && brandId.Value > 0)
            {
                carsQuery = carsQuery.Where(c => c.BrandId == brandId.Value);
            }

            // Filter 4: Location (Branch)
            if (locationId.HasValue && locationId.Value > 0)
            {
                carsQuery = carsQuery.Where(c => c.LocationId == locationId.Value);
            }

            // Filter 5: Seat Count
            if (seatCount.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.SeatCount >= seatCount.Value);
            }

            // Filter 6: Search Term (Model or Plate)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                carsQuery = carsQuery.Where(c => c.Model.ToLower().Contains(term) || c.PlateNumber.ToLower().Contains(term));
            }

            // Filter 7: Price Range
            if (minPrice.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.DailyPrice >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.DailyPrice <= maxPrice.Value);
            }

            // Paging calculation
            int pageSize = 6;
            var filteredCarsList = carsQuery.ToList();
            int totalItems = filteredCarsList.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (totalPages == 0) totalPages = 1;
            page = Math.Clamp(page, 1, totalPages);

            var pagedCarsList = filteredCarsList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new CarListViewModel
            {
                Cars = pagedCarsList,
                Categories = categories,
                Brands = brands,
                Branches = branches,
                SelectedCategoryId = categoryId,
                SelectedBrandId = brandId,
                SelectedLocationId = locationId,
                StartDate = startDate,
                EndDate = endDate,
                SelectedSeatCount = seatCount,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SearchTerm = searchTerm,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }
    }
}
