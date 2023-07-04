using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WW.Application.CustomField.Commands.CreateCustomField;

namespace WW.Application.Purpose.Commands.CreatePurpose;
internal class CreatePurposeCommandValidator : AbstractValidator<CreatePurposeCommand>
{
    public CreatePurposeCommandValidator()
    {
        RuleFor(v => v.PurposeType)
           .NotEmpty()
           .WithMessage("Purpose type cannot be empty")
           .NotNull()
           .WithMessage("Purpose type is required");
       
        RuleFor(v => v.CategoryID)
           .NotEmpty()
           .WithMessage("Category ID type cannot be empty")
           .NotNull()
           .WithMessage("Category ID type is required");

        RuleFor(v => v.Code)
            .NotNull()
            .WithMessage("Code is required")
            .NotEmpty()
            .WithMessage("Code cannot be empty")      
            .MaximumLength(20)
            .WithMessage("Code must be no more than 20 characters long");

        RuleFor(v => v.Description)
            .MaximumLength(1000)
            .WithMessage("Description must be no more than 1000 characters long")
            .NotEmpty()
            .WithMessage("Description cannot be empty")
            .NotNull()
            .WithMessage("Description is required");

        RuleFor(v => v.KeepAliveData)
            .MaximumLength(100)
            .WithMessage("Keep alive data must be no more than 100 characters long")
            .NotEmpty()
            .WithMessage("Keep alive data cannot be empty")
            .NotNull()
            .WithMessage("Keep alive data is required");

        RuleFor(v => v.LinkMoreDetail)
            .MaximumLength(1000)
            .WithMessage("Link more detail must be no more than 1000 characters long")
            .NotEmpty()
            .WithMessage("Link more detail cannot be empty")
            .NotNull()
            .WithMessage("Link more detail is required");

        RuleFor(v => v.Status)
            .NotEmpty()
            .WithMessage("Status cannot be empty")
            .NotNull()
            .WithMessage("Status is required");

        RuleFor(v => v.TextMoreDetail)
            .NotEmpty()
            .WithMessage("Text more detail cannot be empty")
            .NotNull()
            .WithMessage("Text more detail is required");

        RuleFor(v => v.WarningDescription)
            .NotEmpty()
            .WithMessage("Warning description cannot be empty")
            .NotNull()
            .WithMessage("Warning description is required");
    }
}
