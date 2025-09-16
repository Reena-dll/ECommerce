using ECommerce.Application.DTOs.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.Product;

public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        // Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product Id is required.")
            .Must(id => id != Guid.Empty).WithMessage("Product Id cannot be an empty Guid.");

        // Name
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

        // Price
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive number.");

        // Stock
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock must be a positive number.");

        // CategoryId
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required.")
            .Must(id => id != Guid.Empty).WithMessage("CategoryId cannot be an empty Guid.");
    }
}
