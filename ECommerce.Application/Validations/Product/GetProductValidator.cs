using ECommerce.Application.DTOs.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.Product;

public class GetProductValidator : AbstractValidator<GetProductDto>
{
    public GetProductValidator()
    {
        // Name
        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

        // MinPrice
        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue)
            .WithMessage("Minimum price must be greater than or equal to 0.");

        // MaxPrice
        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(x => x.MinPrice).When(x => x.MaxPrice.HasValue && x.MinPrice.HasValue)
            .WithMessage("Maximum price must be greater than or equal to minimum price.");

        // MinStock
        RuleFor(x => x.MinStock)
            .GreaterThanOrEqualTo(0).When(x => x.MinStock.HasValue)
            .WithMessage("Minimum stock must be greater than or equal to 0.");

        // MaxStock
        RuleFor(x => x.MaxStock)
            .GreaterThanOrEqualTo(x => x.MinStock).When(x => x.MaxStock.HasValue && x.MinStock.HasValue)
            .WithMessage("Maximum stock must be greater than or equal to minimum stock.");

        // CategoryIds
        RuleFor(x => x.CategoryIds)
            .Must(list => list == null || list.All(id => id != Guid.Empty))
            .WithMessage("CategoryIds cannot contain empty GUIDs.");

        // CategoryIds
        RuleFor(x => x.CategoryIds)
            .Must(list => list == null || list.Count > 0)
            .WithMessage("At least one CategoryId must be provided.");

        // BaseFilterDto controls 
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}
