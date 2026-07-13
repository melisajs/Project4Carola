using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarolaEnditiyLayer.Entities
{
    public class Car
    {
        public int CarId { get; set; }
        public int BrandId { get; set; }
        public string Model { get; set; }
        public int ModelYear { get; set; }
        public string PlateNumber { get; set; }
        public decimal DailyPrice { get; set; }
        public int SeatCount { get; set; }        
        public int LuggageCapacity { get; set; }  
        public int Mileage { get; set; }
        public bool IsAvailable { get; set; }
        public string FuelType { get; set; }
        public string TransmissionType { get; set; }
        public int CategoryId { get; set; }
        public int LocationId { get; set; }
        public string ImageUrl { get; set; }
    }

    
}
