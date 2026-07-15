using Carola.BusinessLayer.Abstract;
using Carola.WebUI.Models;
using CarolaEnditiyLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;

namespace Carola.WebUI.Controllers
{
    public class BookingController : Controller
    {
        private static string L(string turkish, string english) =>
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "en" ? english : turkish;

        private readonly ICarService _carService;
        private readonly IBrandService _brandService;
        private readonly ILocationService _locationService;
        private readonly ICustomerService _customerService;
        private readonly IReservationService _reservationService;

        public BookingController(
            ICarService carService,
            IBrandService brandService,
            ILocationService locationService,
            ICustomerService customerService,
            IReservationService reservationService)
        {
            _carService = carService;
            _brandService = brandService;
            _locationService = locationService;
            _customerService = customerService;
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int carId, DateTime? pickupDate, DateTime? returnDate, int? pickupLocationId)
        {
            var car = await _carService.TGetByIdAsync(carId);
            if (car == null || !car.IsAvailable)
            {
                return RedirectToAction("CarList", "Car");
            }

            var brand = await _brandService.TGetByIdAsync(car.BrandId);
            var locations = await _locationService.TGetAllAsync();

            // Set default dates if not provided
            var start = pickupDate ?? DateTime.Today.AddDays(1);
            var end = returnDate ?? DateTime.Today.AddDays(4);
            int days = (int)(end - start).TotalDays;
            if (days <= 0) days = 1;

            var viewModel = new BookingViewModel
            {
                Car = car,
                Brand = brand,
                Locations = locations,
                CarId = carId,
                PickupDate = start,
                ReturnDate = end,
                PickupLocationId = pickupLocationId ?? locations.FirstOrDefault()?.LocationId,
                ReturnLocationId = pickupLocationId ?? locations.FirstOrDefault()?.LocationId,
                RentalDays = days,
                TotalPrice = car.DailyPrice * days
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CheckAvailability(int carId, DateTime pickupDate, DateTime returnDate)
        {
            var available = await _reservationService.TIsCarAvailableAsync(carId, pickupDate, returnDate);
            return Json(new
            {
                available,
                message = available
                    ? L("Araç seçtiğiniz tarihlerde müsait.", "The vehicle is available for your selected dates.")
                    : L("Araç bu tarih aralığında müsait değil.", "The vehicle is not available for these dates.")
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(BookingViewModel model)
        {
            var car = await _carService.TGetByIdAsync(model.CarId);
            if (car == null || !car.IsAvailable)
            {
                return RedirectToAction("CarList", "Car");
            }

            var brand = await _brandService.TGetByIdAsync(car.BrandId);
            var locations = await _locationService.TGetAllAsync();

            model.Car = car;
            model.Brand = brand;
            model.Locations = locations;

            // Recalculate days and price
            if (model.PickupDate.HasValue && model.ReturnDate.HasValue)
            {
                model.RentalDays = (int)(model.ReturnDate.Value - model.PickupDate.Value).TotalDays;
                if (model.RentalDays <= 0) model.RentalDays = 1;
                model.TotalPrice = car.DailyPrice * model.RentalDays;
            }

            // 1. Date Validation: both dates are required and return must be after pickup.
            if (!model.PickupDate.HasValue || !model.ReturnDate.HasValue ||
                model.PickupDate.Value.Date < DateTime.Today ||
                model.PickupDate.Value >= model.ReturnDate.Value)
            {
                model.ErrorMessage = L("İade tarihi alış tarihinden sonra olmalıdır.", "The return date must be after the pick-up date.");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName) ||
                string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Phone) ||
                !model.BirthDate.HasValue || !model.LicenseIssueDate.HasValue ||
                string.IsNullOrWhiteSpace(model.LicenseClass) || string.IsNullOrWhiteSpace(model.DriverLicenseNumber))
            {
                model.ErrorMessage = L("Lütfen zorunlu sürücü ve iletişim bilgilerini eksiksiz doldurun.", "Complete all required driver and contact details.");
                return View(model);
            }

            // 2. Collision Check: overlap checking
            if (!await _reservationService.TIsCarAvailableAsync(model.CarId, model.PickupDate.Value, model.ReturnDate.Value))
            {
                model.ErrorMessage = L("Bu tarihler arasında araç başka bir rezervasyonda. Lütfen farklı tarih seçin.", "This vehicle already has a booking for these dates. Select different dates.");
                return View(model);
            }

            // 3. Save Customer Details
            var customer = new Customer
            {
                FirstName = model.FirstName ?? "İsimsiz",
                LastName = model.LastName ?? "Müşteri",
                Email = model.Email ?? "mail@example.com",
                Phone = model.Phone ?? "+905550000000",
                DriverLicenseNumber = model.DriverLicenseNumber ?? "TR-000000",
                BirthDate = model.BirthDate ?? DateTime.Today.AddYears(-25)
            };
            await _customerService.TInsertAsync(customer);

            // 4. Save Reservation
            var reservation = new Reservation
            {
                CarId = model.CarId,
                CustomerId = customer.CustomerId,
                PickupDate = model.PickupDate.Value,
                ReturnDate = model.ReturnDate.Value,
                PickupLocationId = model.PickupLocationId ?? 1,
                ReturnLocationId = model.ReturnLocationId ?? 1,
                TotalPrice = model.TotalPrice,
                Description = $"{(model.OcrVerified ? "OCR doğrulandı" : "Manuel giriş")} | Ehliyet sınıfı: {model.LicenseClass} | " +
                              $"Ehliyet veriliş: {model.LicenseIssueDate:dd.MM.yyyy} | " +
                              $"Not: {model.AdditionalNotes ?? "Belirtilmedi"}",
                Status = "Bekliyor" // Starts as "Onay Bekleniyor" as required
            };
            await _reservationService.TInsertAsync(reservation);
            return RedirectToAction(nameof(Confirmation), new { id = reservation.ReservationId });
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(int id)
        {
            var model = await BuildConfirmationModelAsync(id);
            return model == null ? RedirectToAction(nameof(Track)) : View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Status(int id)
        {
            var reservation = await _reservationService.TGetByIdAsync(id);
            return reservation == null
                ? NotFound()
                : Json(new { reservationId = id, status = reservation.Status, code = $"CAR-{id:000000}" });
        }

        [HttpGet]
        public IActionResult Track()
        {
            return View(new ReservationTrackViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Track(ReservationTrackViewModel model)
        {
            var normalizedCode = (model.ReservationCode ?? string.Empty).Trim().ToUpperInvariant();
            if (!normalizedCode.StartsWith("CAR-") || !int.TryParse(normalizedCode[4..], out var id))
            {
                model.ErrorMessage = L("Rezervasyon kodu CAR-000000 formatında olmalıdır.", "The reservation code must use the CAR-000000 format.");
                return View(model);
            }

            var confirmation = await BuildConfirmationModelAsync(id);
            if (confirmation == null || !string.Equals(confirmation.Customer.Email, model.Email?.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                model.ErrorMessage = L("Rezervasyon kodu veya e-posta adresi eşleşmedi.", "The reservation code and email address do not match.");
                return View(model);
            }

            model.Result = confirmation;
            return View(model);
        }

        private async Task<ReservationConfirmationViewModel?> BuildConfirmationModelAsync(int id)
        {
            var reservation = await _reservationService.TGetByIdAsync(id);
            if (reservation == null) return null;

            var customer = await _customerService.TGetByIdAsync(reservation.CustomerId);
            var car = await _carService.TGetByIdAsync(reservation.CarId);
            if (customer == null || car == null) return null;

            return new ReservationConfirmationViewModel
            {
                Reservation = reservation,
                Customer = customer,
                Car = car,
                Brand = await _brandService.TGetByIdAsync(car.BrandId) ?? new Brand(),
                PickupLocation = await _locationService.TGetByIdAsync(reservation.PickupLocationId) ?? new Location(),
                ReturnLocation = await _locationService.TGetByIdAsync(reservation.ReturnLocationId) ?? new Location()
            };
        }
    }
}
