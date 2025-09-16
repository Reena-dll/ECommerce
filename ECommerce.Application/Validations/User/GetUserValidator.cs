using ECommerce.Application.DTOs.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.User;

public class GetUserValidator : AbstractValidator<GetUserDto>
{
    public GetUserValidator()
    {
        // Name 
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Name filter must not exceed 100 characters.")
            .MinimumLength(2).When(x => !string.IsNullOrWhiteSpace(x.Name))
                .WithMessage("Name filter must be at least 2 characters long.");

        // BaseFilterDto controls 
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}
