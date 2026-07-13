using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarolaEnditiyLayer.Entities
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int PickupLocationId { get; set; }
        public int ReturnLocationId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // "Bekliyor", "Onaylandı", "İptal"

       
    }
}
