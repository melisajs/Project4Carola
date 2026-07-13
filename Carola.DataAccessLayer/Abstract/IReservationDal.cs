using CarolaEnditiyLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DataAccessLayer.Abstract
{
    public interface IReservationDal:IGenericalDal<Reservation>
    {
        Task<bool> HasOverlapAsync(int carId, DateTime pickupDate, DateTime returnDate, int? excludedReservationId = null);
        Task<List<int>> GetUnavailableCarIdsAsync(DateTime pickupDate, DateTime returnDate);
    }
}
