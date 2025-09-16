using ECommerce.Application.DTOs.User;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.User;

public class AddUserValidator : AbstractValidator<AddUserDto>
{
    public AddUserValidator()
    {
        // FirstName
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        // LastName
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        // Username
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username cannot be empty.")
            .Matches("^[a-zA-Z0-9._-]+$").WithMessage("Username can only contain letters, digits, dots, dashes, or underscores.")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters long.")
            .MaximumLength(20).WithMessage("Username must not exceed 20 characters.");

        // Password
        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .Must(p => string.IsNullOrEmpty(p) || p.Length >= 8)
                .WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W_]").When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("Password must contain at least one special character.");

        // Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

        // PersonnelNumber
        RuleFor(x => x.PersonnelNumber)
            .NotEmpty().WithMessage("Personnel number cannot be empty.")
            .Matches(@"^[0-9]+$").WithMessage("Personnel number must contain only digits.")
            .Length(5, 15).WithMessage("Personnel number must be between 5 and 15 digits long.");

        // RoleId
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("At least one role must be assigned.")
            .Must(list => list.All(id => id != Guid.Empty))
                .WithMessage("RoleId cannot contain an empty Guid.");
    }
}
