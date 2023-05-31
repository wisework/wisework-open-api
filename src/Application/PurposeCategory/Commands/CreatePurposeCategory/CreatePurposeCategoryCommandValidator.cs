using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WW.Application.CollectionPoints.Commands.CreateCollectionPoint;

namespace WW.Application.PurposeCategory.Commands.CreatePurposeCategory;

public class CreatePurposeCategoryCommandValidator : AbstractValidator<CreatePurposeCategoryCommand>
{
    public CreatePurposeCategoryCommandValidator()
    {
        RuleFor(v => v.Code)
            .MaximumLength(20)
            .WithMessage("Code must be no more than 20 characters long")
            .NotEmpty()
             .WithMessage("Code cannot be empty")
            .NotNull()
        .WithMessage("Code is required");
        RuleFor(v => v.Description)
           .MaximumLength(1000)
            .WithMessage("Description must be no more than 1000 characters long")
            .NotEmpty()
             .WithMessage("Description cannot be empty")
            .NotNull()
        .WithMessage("Description is required");


    }
}


