using CarolaEnditiyLayer.Entities;

namespace Carola.WebUI.Models;

public class ReservationConfirmationViewModel
{
    public Reservation Reservation { get; set; } = new();
    public Customer Customer { get; set; } = new();
    public Car Car { get; set; } = new();
    public Brand Brand { get; set; } = new();
    public Location PickupLocation { get; set; } = new();
    public Location ReturnLocation { get; set; } = new();
    public string ReservationCode => $"CAR-{Reservation.ReservationId:000000}";
}

public class ReservationTrackViewModel
{
    public string ReservationCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public ReservationConfirmationViewModel? Result { get; set; }
}
