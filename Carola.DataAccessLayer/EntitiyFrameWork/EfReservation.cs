using Carola.DataAccessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using CarolaEnditiyLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Carola.DataAccessLayer.EntitiyFrameWork
{
    public class EfReservation : GenericRepository<Reservation>, IReservationDal
    {
        private readonly CaraloContext _context;

        public EfReservation(CaraloContext context) : base(context)
        {
            _context = context;
        }

        public Task<bool> HasOverlapAsync(int carId, DateTime pickupDate, DateTime returnDate, int? excludedReservationId = null)
        {
            return _context.Reservations.AnyAsync(r =>
                r.CarId == carId &&
                r.Status != "İptal" &&
                (!excludedReservationId.HasValue || r.ReservationId != excludedReservationId.Value) &&
                r.PickupDate < returnDate && pickupDate < r.ReturnDate);
        }

        public Task<List<int>> GetUnavailableCarIdsAsync(DateTime pickupDate, DateTime returnDate)
        {
            return _context.Reservations
                .Where(r => r.Status != "İptal" && r.PickupDate < returnDate && pickupDate < r.ReturnDate)
                .Select(r => r.CarId)
                .Distinct()
                .ToListAsync();
        }
    }
}
