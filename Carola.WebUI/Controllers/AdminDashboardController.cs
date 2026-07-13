using Carola.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Carola.WebUI.Controllers
{
    public class RecentReservationDto
    {
        public CarolaEnditiyLayer.Entities.Reservation Reservation { get; set; } = new();
        public string CustomerName { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public string CarImage { get; set; } = string.Empty;
    }

    public class AdminDashboardController : Controller
    {
        private readonly ICarService _carService;
        private readonly IReservationService _reservationService;
        private readonly ICustomerService _customerService;
        private readonly IBrandService _brandService;
        private readonly ILocationService _locationService;

        public AdminDashboardController(
            ICarService carService,
            IReservationService reservationService,
            ICustomerService customerService,
            IBrandService brandService,
            ILocationService locationService)
        {
            _carService = carService;
            _reservationService = reservationService;
            _customerService = customerService;
            _brandService = brandService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _carService.TGetAllAsync();
            var reservations = await _reservationService.TGetAllAsync();
            var customers = await _customerService.TGetAllAsync();
            var brands = await _brandService.TGetAllAsync();
            var locations = await _locationService.TGetAllAsync();

            // Stats calculations
            ViewBag.TotalCars = cars.Count;
            ViewBag.TotalReservations = reservations.Count;
            ViewBag.TotalCustomers = customers.Count;
            ViewBag.TotalBrands = brands.Count;
            
            ViewBag.PendingReservationsCount = reservations.Count(r => r.Status == "Bekliyor");
            ViewBag.ApprovedReservationsCount = reservations.Count(r => r.Status == "Onaylandı");
            
            // Total monthly revenue (sum of approved bookings)
            ViewBag.MonthlyRevenue = reservations.Where(r => r.Status == "Onaylandı").Sum(r => r.TotalPrice);

            // Fetch recent bookings (ordered by reservation id desc, take 5)
            var recentReservations = reservations
                .OrderByDescending(r => r.ReservationId)
                .Take(6)
                .Select(r => {
                    var customer = customers.FirstOrDefault(c => c.CustomerId == r.CustomerId);
                    var car = cars.FirstOrDefault(c => c.CarId == r.CarId);
                    return new RecentReservationDto {
                        Reservation = r,
                        CustomerName = customer != null ? $"{customer.FirstName} {customer.LastName}" : "Bilinmeyen",
                        CarModel = car != null ? car.Model : "Araç Silindi",
                        CarImage = car?.ImageUrl ?? string.Empty
                    };
                }).ToList();

            ViewBag.RecentReservations = recentReservations;
            ViewBag.Locations = locations;
            ViewBag.Cars = cars;

            return View();
        }
    }
}
