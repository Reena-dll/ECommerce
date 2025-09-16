using ECommerce.Application.DTOs.Role;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.Role;

public class UpdateRolePermissionValidator : AbstractValidator<UpdateRolePermissionDto>
{
    public UpdateRolePermissionValidator()
    {
        // roleId
        RuleFor(x => x.roleId)
            .NotEmpty().WithMessage("Role Id is required.")
            .Must(id => id != Guid.Empty).WithMessage("Role Id cannot be an empty Guid.");

        // PermissionIds
        RuleFor(x => x.PermissionIds)
            .NotEmpty().WithMessage("At least one permission must be assigned.")
            .Must(list => list.All(id => id != Guid.Empty))
                .WithMessage("PermissionIds cannot contain an empty Guid.");
    }
}
