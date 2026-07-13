using CarolaEnditiyLayer.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.ValidationRules
{
    public class CarValidator:AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("Marka seçilmelidir.")
            .GreaterThan(0).WithMessage("Geçerli bir marka seçiniz.");

            RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model alanı boş geçilemez.")
            .MinimumLength(2).WithMessage("Model en az 2 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("Model en fazla 50 karakter olabilir.");

            RuleFor(x => x.ModelYear)
            .NotEmpty().WithMessage("Model yılı boş geçilemez.")
            .InclusiveBetween(1990, DateTime.Now.Year)
            .WithMessage($"Model yılı 1990 ile {DateTime.Now.Year} arasında olmalıdır.");

            RuleFor(x => x.PlateNumber)
            .NotEmpty().WithMessage("Plaka numarası boş geçilemez.")
            .MinimumLength(5).WithMessage("Plaka numarası en az 5 karakter olmalıdır.")
            .MaximumLength(15).WithMessage("Plaka numarası en fazla 15 karakter olabilir.");

            RuleFor(x => x.DailyPrice)
            .NotEmpty().WithMessage("Günlük fiyat boş geçilemez.")
            .GreaterThan(0).WithMessage("Günlük fiyat 0'dan büyük olmalıdır.");

            RuleFor(x => x.SeatCount)
            .NotEmpty().WithMessage("Koltuk sayısı boş geçilemez.")
            .InclusiveBetween(1, 12).WithMessage("Koltuk sayısı 1 ile 12 arasında olmalıdır.");

            RuleFor(x => x.LuggageCapacity)
            .NotEmpty().WithMessage("Bagaj kapasitesi boş geçilemez.")
            .InclusiveBetween(0, 10).WithMessage("Bagaj kapasitesi 0 ile 10 arasında olmalıdır.");

            RuleFor(x => x.Mileage)
            .NotEmpty().WithMessage("Kilometre bilgisi boş geçilemez.")
            .GreaterThanOrEqualTo(0).WithMessage("Kilometre değeri negatif olamaz.");

            RuleFor(x => x.FuelType)
            .NotEmpty().WithMessage("Yakıt tipi boş geçilemez.")
            .MaximumLength(30).WithMessage("Yakıt tipi en fazla 30 karakter olabilir.");

            RuleFor(x => x.TransmissionType)
            .NotEmpty().WithMessage("Vites tipi boş geçilemez.")
            .MaximumLength(30).WithMessage("Vites tipi en fazla 30 karakter olabilir.");

            RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Kategori seçilmelidir.")
            .GreaterThan(0).WithMessage("Geçerli bir kategori seçiniz.");

            RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Araç görseli boş geçilemez.")
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Geçerli bir görsel URL giriniz.");
        }

    }
}
