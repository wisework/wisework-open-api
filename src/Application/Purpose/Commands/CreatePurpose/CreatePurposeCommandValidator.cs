using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace WW.Application.Purpose.Commands.CreatePurpose;
public class CreatePurposeCommandValidator : AbstractValidator<CreatePurposeCommand>
{
    public CreatePurposeCommandValidator()
    {
        RuleFor(v => v.purposeType)
            .NotEmpty()
            .WithMessage("Purpose type cannot be empty")
            .GreaterThan(0)
            .WithMessage("Purpose type must be greater than 0");

        RuleFor(v => v.purposeCategoryId)
            .NotEmpty()
            .WithMessage("Purpose category ID cannot be empty")
            .GreaterThan(0)
            .WithMessage("Purpose category ID must be greater than 0");

        RuleFor(v => v.code)
            .NotNull()
            .WithMessage("Code is required")
            .NotEmpty()
            .WithMessage("Code cannot be empty")
            .MaximumLength(20)
            .WithMessage("Code must be no more than 20 characters long");

        RuleFor(v => v.description)
            .NotNull()
            .WithMessage("Description is required")
            .NotEmpty()
            .WithMessage("Description cannot be empty")
            .MaximumLength(1000)
            .WithMessage("Description must be no more than 1000 characters long");

        RuleFor(v => v.keepAliveData)
            .NotNull()
            .WithMessage("Keep alive data is required")
            .NotEmpty()
            .WithMessage("Keep alive data cannot be empty")
            .MaximumLength(100)
            .WithMessage("Keep alive data must be no more than 100 characters long");

        RuleFor(v => v.linkMoreDetail)
            .NotNull()
            .WithMessage("Link more detail is required")
            .NotEmpty()
            .WithMessage("Link more detail cannot be empty")
            .MaximumLength(1000)
            .WithMessage("Link more detail must be no more than 1000 characters long");

        RuleFor(v => v.status)
            .NotNull()
            .WithMessage("Status is required")
            .NotEmpty()
            .WithMessage("Status cannot be empty");

        RuleFor(v => v.textMoreDetail)
            .NotNull()
            .WithMessage("Text more detail is required")
            .NotEmpty()
            .WithMessage("Text more detail cannot be empty");

        RuleFor(v => v.warningDescription)
            .NotNull()
            .WithMessage("Warning description is required")
            .NotEmpty()
            .WithMessage("Warning description cannot be empty");
    }
}
