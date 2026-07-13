using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Abstract;
using CarolaEnditiyLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.Concrete
{
    public class ReservationManager : IReservationService
    {
        private readonly IReservationDal _reservationDal;

        public ReservationManager(IReservationDal reservationDal)
        {
            _reservationDal = reservationDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _reservationDal.DeleteAsync(id);
        }

        public async Task<List<Reservation>> TGetAllAsync()
        {
            return await _reservationDal.GetAllAsync();
        }

        public async Task<Reservation> TGetByIdAsync(int id)
        {
            return await _reservationDal.GetByIdAsync(id);
        }

        public async Task TInsertAsync(Reservation entity)
        {
            await _reservationDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Reservation entity)
        {
            await _reservationDal.UpdateAsync(entity);
        }

        public async Task<bool> TIsCarAvailableAsync(int carId, DateTime pickupDate, DateTime returnDate, int? excludedReservationId = null)
        {
            if (pickupDate.Date < DateTime.Today || returnDate.Date <= pickupDate.Date)
            {
                return false;
            }

            return !await _reservationDal.HasOverlapAsync(carId, pickupDate.Date, returnDate.Date, excludedReservationId);
        }

        public Task<List<int>> TGetUnavailableCarIdsAsync(DateTime pickupDate, DateTime returnDate)
        {
            return _reservationDal.GetUnavailableCarIdsAsync(pickupDate.Date, returnDate.Date);
        }
    }
}
