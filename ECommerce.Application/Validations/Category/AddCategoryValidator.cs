using ECommerce.Application.DTOs.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.Category;

public class AddCategoryValidator : AbstractValidator<AddCategoryDto>
{
    public AddCategoryValidator()
    {
        RuleFor(x => x.Name)
          .NotEmpty().WithMessage("Category name is required.")
          .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.") // En fazla 100 karakter olmalı
          .Matches(@"^[a-zA-Z0-9\s]*$").WithMessage("Category name can only contain letters, digits, and spaces.");
    }
}
