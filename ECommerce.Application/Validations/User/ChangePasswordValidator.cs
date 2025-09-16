using ECommerce.Application.DTOs.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.User;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordValidator()
    {
        // Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

        // OldPassword
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password cannot be empty.");

        // NewPassword
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password cannot be empty.")
            .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("New password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("New password must contain at least one digit.")
            .Matches(@"[\W_]").WithMessage("New password must contain at least one special character.")
            .Must((dto, newPwd) => newPwd != dto.OldPassword)
                .WithMessage("New password cannot be the same as the old password.");
    }
}
