using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace WW.Application.Section.Commands.UpdateSection;
public class UpdateSectionCommandValidator : AbstractValidator<UpdateSectionCommand>
{
    public UpdateSectionCommandValidator()
    {
        RuleFor(v => v.Code)
        .NotNull()
        .WithMessage("Code is required")
        .NotEmpty()
        .WithMessage("Code cannot be empty")
        .MaximumLength(20)
        .WithMessage("Code must be no more than 20 characters long");

        RuleFor(v => v.Description)
         .NotNull()
         .WithMessage("Description is required")
         .NotEmpty()
         .WithMessage("Description cannot be empty")
         .MaximumLength(1000)
         .WithMessage("Description must be no more than 1000 characters long");

        RuleFor(v => v.Status)
          .NotNull()
          .WithMessage("Status is required")
          .NotEmpty()
          .WithMessage("Status cannot be empty");


    }
}