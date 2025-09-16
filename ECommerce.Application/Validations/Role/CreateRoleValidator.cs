using ECommerce.Application.DTOs.Role;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.Role;
public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
{
    public CreateRoleValidator()
    {
        // RoleName
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Role name cannot be empty.")
            .MinimumLength(3).WithMessage("Role name must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Role name must not exceed 50 characters.");
           

        // PermissionIds
        RuleFor(x => x.PermissionIds)
            .NotEmpty().WithMessage("At least one permission must be assigned.")
            .Must(list => list.All(id => id != Guid.Empty))
                .WithMessage("PermissionIds cannot contain an empty Guid.");
    }
}
