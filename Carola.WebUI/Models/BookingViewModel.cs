using System;
using System.Collections.Generic;
using CarolaEnditiyLayer.Entities;

namespace Carola.WebUI.Models
{
    public class BookingViewModel
    {
        public Car Car { get; set; } = new();
        public Brand Brand { get; set; } = new();
        public List<Location> Locations { get; set; } = new();

        // Booking details
        public int CarId { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? PickupLocationId { get; set; }
        public int? ReturnLocationId { get; set; }
        public int RentalDays { get; set; }
        public decimal TotalPrice { get; set; }

        // Customer details filled by OCR
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string DriverLicenseNumber { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public DateTime? LicenseIssueDate { get; set; }
        public string LicenseClass { get; set; } = string.Empty;
        public bool OcrVerified { get; set; }
        public string OcrRawText { get; set; } = string.Empty;
        public string AdditionalNotes { get; set; } = string.Empty;

        // Booking Status
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
    }
}
