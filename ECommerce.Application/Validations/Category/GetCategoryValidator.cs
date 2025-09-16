using ECommerce.Application.DTOs.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.Category;

public class GetCategoryValidator : AbstractValidator<GetCategoryDto>
{
    public GetCategoryValidator()
    {
        RuleFor(x => x.Name)
           .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.")
           .Matches(@"^[a-zA-Z0-9\s]*$").WithMessage("Category name can only contain letters, digits, and spaces.")
           .When(x => !string.IsNullOrEmpty(x.Name));

        // BaseFilterDto controls 
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}
