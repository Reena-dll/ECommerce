using ECommerce.Application.DTOs.Role;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.Role;  

public class RoleFilterValidator : AbstractValidator<RoleFilterDto>
{
    public RoleFilterValidator()
    {
        // RoleId
        RuleFor(x => x.RoleId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("RoleId cannot be an empty Guid.");

        // BaseFilterDto assumed properties
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}
