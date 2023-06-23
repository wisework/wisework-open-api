using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace WW.Application.CustomField.Commands.CreateCustomField;

public class CreateCustomFieldCommandValidator : AbstractValidator<CreateCustomFieldCommand>
{
    public CreateCustomFieldCommandValidator()
    {
        RuleFor(v => v.code)
            .NotNull()
            .WithMessage("Code is required")
            .NotEmpty()
            .WithMessage("Code cannot be empty")
            .MaximumLength(20)
            .WithMessage("Code must be no more than 20 characters long");

        RuleFor(v => v.owner)
            .NotNull()
            .WithMessage("Owner is required")
            .NotEmpty()
            .WithMessage("Owner cannot be empty")
            .MaximumLength(20)
            .WithMessage("Owner must be no more than 20 characters long");

        RuleFor(v => v.inputType)
            .NotNull()
            .WithMessage("Input type is required")
            .NotEmpty()
            .WithMessage("Input type cannot be empty")
            .MaximumLength(20)
            .WithMessage("Input type must be no more than 20 characters long");

        RuleFor(v => v.title)
            .NotNull()
            .WithMessage("Title is required")
            .NotEmpty()
            .WithMessage("Title cannot be empty")
            .MaximumLength(100)
            .WithMessage("Title must be no more than 100 characters long");

        RuleFor(v => v.placeholder)
            .NotNull()
            .WithMessage("Placeholder is required")
            .NotEmpty()
            .WithMessage("Placeholder cannot be empty")
            .MaximumLength(100)
            .WithMessage("Placeholder must be no more than 100 characters long");

        RuleFor(v => v.lengthLimit)
            .NotEmpty()
            .WithMessage("Length limit cannot be empty")
            .GreaterThan(0)
            .WithMessage("Length limit must be greater than 0");

        RuleFor(v => v.maxLines)
            .NotEmpty()
            .WithMessage("Max lines cannot be empty")
            .GreaterThan(0)
            .WithMessage("Max lines must be greater than 0");

        RuleFor(v => v.minLines)
            .NotEmpty()
            .WithMessage("Min lines cannot be empty")
            .GreaterThan(0)
            .WithMessage("Min lines must be greater than 0");
    }
}
