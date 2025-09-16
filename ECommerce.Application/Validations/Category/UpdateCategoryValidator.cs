using ECommerce.Application.DTOs.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.Category;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
        // Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Category Id is required.")
            .Must(id => id != Guid.Empty).WithMessage("Category Id cannot be an empty Guid.");

        // Name
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.")
            .Matches(@"^[a-zA-Z0-9\s]*$").WithMessage("Category name can only contain letters, digits, and spaces.");
    }
}
