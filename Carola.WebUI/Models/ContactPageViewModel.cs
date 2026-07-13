using System.ComponentModel.DataAnnotations;
using CarolaEnditiyLayer.Entities;

namespace Carola.WebUI.Models;

public class ContactPageViewModel
{
    [Required(ErrorMessage = "Ad soyad alanı zorunludur.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "Ad soyad 3-80 karakter arasında olmalıdır.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    [StringLength(25)]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Konu alanı zorunludur.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Konu 3-100 karakter arasında olmalıdır.")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mesaj alanı zorunludur.")]
    [StringLength(1500, MinimumLength = 10, ErrorMessage = "Mesaj en az 10, en fazla 1500 karakter olmalıdır.")]
    public string Message { get; set; } = string.Empty;

    public List<Location> Branches { get; set; } = new();
}
