using CarolaEnditiyLayer.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.ValidationRules
{
    public class CategoryValidator:AbstractValidator<Category>
    {
        public CategoryValidator()
        { 
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Kategori adı boş olamaz");
            RuleFor(x => x.CategoryName).MinimumLength(2).WithMessage("Kategori adı en az 2 karakter olmalıdır").MaximumLength(20).WithMessage("Lütfen en fazla 20 karakter girişi yapınız");
        }
    }
}
