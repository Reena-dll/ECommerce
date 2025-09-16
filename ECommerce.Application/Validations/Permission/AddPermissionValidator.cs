using ECommerce.Application.DTOs.Permission;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.Permission;
public class AddPermissionValidator : AbstractValidator<AddPermissionDto>
{
    public AddPermissionValidator()
    {
        // Name
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Permission name cannot be empty.")
            .MinimumLength(3).WithMessage("Permission name must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Permission name must not exceed 50 characters.");
           

        // Description
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Permission description cannot be empty.")
            .MinimumLength(5).WithMessage("Permission description must be at least 5 characters long.")
            .MaximumLength(500).WithMessage("Permission description must not exceed 500 characters.");
    }
}

