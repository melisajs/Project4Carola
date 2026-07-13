using CarolaEnditiyLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.Abstract
{
    public interface IReservationService:IGenericService<Reservation>
    {
        Task<bool> TIsCarAvailableAsync(int carId, DateTime pickupDate, DateTime returnDate, int? excludedReservationId = null);
        Task<List<int>> TGetUnavailableCarIdsAsync(DateTime pickupDate, DateTime returnDate);
    }
}
