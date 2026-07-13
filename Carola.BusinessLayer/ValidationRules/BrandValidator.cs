using CarolaEnditiyLayer.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.ValidationRules
{
    public class BrandValidator : AbstractValidator<Brand>
    {
      public BrandValidator()
        {

            RuleFor(x => x.BrandName)
                .NotEmpty().WithMessage("Marka adı boş geçilemez.")
                .Length(2, 40).WithMessage("Marka adı 2 ile 40 karakter arasında olmalıdır.")
                .Matches(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$")
                .WithMessage("Marka adı sadece harflerden oluşmalıdır.")
                .NotEqual("Test").WithMessage("Geçersiz marka adı.");

            RuleFor(x => x.BrandName)
                .Must(BeAValidBrandName)
                .WithMessage("Marka adı özel karakter içeremez.");

            RuleFor(x => x.LogoUrl)
                .NotEmpty().WithMessage("Logo adresi boş geçilemez.")
                .Must(BeAValidImageUrl)
                .WithMessage("Logo URL geçerli bir görsel adresi olmalıdır.");

            RuleFor(x => x.Status)
                .NotNull().WithMessage("Durum bilgisi belirtilmelidir.");

            RuleFor(x => x.BrandId)
                .GreaterThanOrEqualTo(0)
                .WithMessage("BrandId negatif olamaz.");

            When(x => x.Status == true, () =>
            {
                RuleFor(x => x.LogoUrl)
                    .NotEmpty().WithMessage("Aktif markalar için logo zorunludur.");
            });
        }

        private bool BeAValidBrandName(string brandName)
        {
            return !brandName.Contains("@") && !brandName.Contains("#");
        }

        private bool BeAValidImageUrl(string url)
        {
            return url.EndsWith(".png") || url.EndsWith(".jpg") || url.EndsWith(".jpeg");
        }

    }
}

    

