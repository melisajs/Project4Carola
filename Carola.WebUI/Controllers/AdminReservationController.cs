using Carola.BusinessLayer.Abstract;
using Carola.WebUI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Carola.WebUI.Controllers
{
    public class AdminReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ICustomerService _customerService;
        private readonly ICarService _carService;
        private readonly ILocationService _locationService;
        private readonly EmailService _emailService;

        public AdminReservationController(
            IReservationService reservationService,
            ICustomerService customerService,
            ICarService carService,
            ILocationService locationService,
            EmailService emailService)
        {
            _reservationService = reservationService;
            _customerService = customerService;
            _carService = carService;
            _locationService = locationService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationService.TGetAllAsync();
            var customers = await _customerService.TGetAllAsync();
            var cars = await _carService.TGetAllAsync();
            var locations = await _locationService.TGetAllAsync();

            var viewModel = reservations.Select(r => {
                var customer = customers.FirstOrDefault(c => c.CustomerId == r.CustomerId);
                var car = cars.FirstOrDefault(c => c.CarId == r.CarId);
                var pickupLoc = locations.FirstOrDefault(l => l.LocationId == r.PickupLocationId);
                var returnLoc = locations.FirstOrDefault(l => l.LocationId == r.ReturnLocationId);

                return new {
                    Reservation = r,
                    CustomerName = customer != null ? $"{customer.FirstName} {customer.LastName}" : "Bilinmeyen Müşteri",
                    CustomerEmail = customer?.Email ?? "mail@example.com",
                    CarModel = car != null ? car.Model : "Araç Silindi",
                    PickupLocationName = pickupLoc?.LocationName ?? "Belirtilmemiş",
                    ReturnLocationName = returnLoc?.LocationName ?? "Belirtilmemiş"
                };
            }).ToList();

            ViewBag.ReservationsList = viewModel;
            return View();
        }

        public async Task<IActionResult> Approve(int id)
        {
            var reservation = await _reservationService.TGetByIdAsync(id);
            if (reservation != null)
            {
                if (reservation.Status == "Onaylandı")
                {
                    TempData["Info"] = "Rezervasyon daha önce onaylanmış.";
                    return RedirectToAction("Index");
                }

                // Persist the business operation first. Email delivery must never roll back an approval.
                reservation.Status = "Onaylandı";
                await _reservationService.TUpdateAsync(reservation);

                // Load related details to send email
                var customer = await _customerService.TGetByIdAsync(reservation.CustomerId);
                var car = await _carService.TGetByIdAsync(reservation.CarId);
                var pickupLoc = await _locationService.TGetByIdAsync(reservation.PickupLocationId);
                var returnLoc = await _locationService.TGetByIdAsync(reservation.ReturnLocationId);

                var emailPrepared = false;
                if (customer != null && car != null)
                {
                    try
                    {
                        emailPrepared = await _emailService.SendReservationApprovalEmailAsync(
                            customer.Email,
                            $"{customer.FirstName} {customer.LastName}",
                            car.Model,
                            pickupLoc?.LocationName ?? "Şube",
                            returnLoc?.LocationName ?? "Şube",
                            reservation.PickupDate,
                            reservation.ReturnDate,
                            reservation.TotalPrice,
                            $"CAR-{reservation.ReservationId:000000}");
                    }
                    catch
                    {
                        emailPrepared = false;
                    }
                }

                TempData["Success"] = emailPrepared
                    ? "Rezervasyon onaylandı ve teklif e-postası hazırlandı."
                    : "Rezervasyon başarıyla onaylandı.";

                if (!emailPrepared)
                {
                    TempData["Info"] = "E-posta şu anda gönderilemedi; rezervasyon onayı başarıyla kaydedildi.";
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Reject(int id)
        {
            var reservation = await _reservationService.TGetByIdAsync(id);
            if (reservation != null)
            {
                reservation.Status = "İptal";
                await _reservationService.TUpdateAsync(reservation);
            }
            return RedirectToAction("Index");
        }
    }
}
