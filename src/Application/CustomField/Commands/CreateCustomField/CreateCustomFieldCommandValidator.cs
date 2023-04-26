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
        RuleFor(v => v.Code)
            .MaximumLength(1000)
            .NotEmpty()
            .NotNull();

        RuleFor(v => v.Owner)
            .NotEmpty()
            .NotNull();

        RuleFor(v => v.InputType)
            .NotEmpty()
            .NotNull();

        RuleFor(v => v.Title)
            .NotEmpty()
            .NotNull();

        RuleFor(v => v.Placeholder)
            .NotEmpty()
            .NotNull();

        RuleFor(v => v.LengthLimit)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(v => v.MaxLines)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(v => v.MinLines)
            .NotEmpty()
            .GreaterThan(0);
    }
}
