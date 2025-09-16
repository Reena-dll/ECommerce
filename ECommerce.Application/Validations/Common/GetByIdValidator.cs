using ECommerce.Application.DTOs.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validations.Common;

public class GetByIdValidator : AbstractValidator<GetByIdDto>
{
	public GetByIdValidator()
	{
        // Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .Must(id => id != Guid.Empty).WithMessage("Id cannot be an empty Guid.");
    }
}
