using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WW.Application.PurposeCategory.Commands.CreatePurposeCategory;

namespace WW.Application.PurposeCategory.Commands.UpdatePurposeCategory;
public class UpdatePurposeCategoryCommandValidator : AbstractValidator<UpdatePurposeCategoryCommand>
{
    public UpdatePurposeCategoryCommandValidator()
    {
        RuleFor(v => v.Description)
           .MaximumLength(1000)
            .WithMessage("Description must be no more than 1000 characters long")
            .NotEmpty()
             .WithMessage("Description cannot be empty")
            .NotNull()
        .WithMessage("Description is required");


    }
}