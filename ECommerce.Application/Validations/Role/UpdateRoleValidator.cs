using ECommerce.Application.DTOs.Role;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.Role;
public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
{
    public UpdateRoleValidator()
    {
        // Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Role Id is required.")
            .Must(id => id != Guid.Empty).WithMessage("Role Id cannot be an empty Guid.");

        // RoleName
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Role name cannot be empty.")
            .MinimumLength(3).WithMessage("Role name must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Role name must not exceed 50 characters.");
          
    }
}
