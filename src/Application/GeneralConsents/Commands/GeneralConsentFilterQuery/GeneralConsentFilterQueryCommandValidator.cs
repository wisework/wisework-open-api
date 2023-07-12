using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;


namespace WW.Application.GeneralConsents.Commands.GeneralConsentFilterQuery;
public class GeneralConsentFilterQueryCommandValidator : AbstractValidator<GeneralConsentFilterQueryCommand>
{
    public GeneralConsentFilterQueryCommandValidator() 
    {
        RuleFor(v => v.SortingParams.SortName)
           .MaximumLength(100)
           .WithMessage("SortName must be no more than 100 characters long");
        RuleFor(v => v.SortingParams.SortDesc)
          .NotNull()
            .WithMessage("SortDesc is required")
            .NotEmpty()
            .WithMessage("SortDesc cannot be empty");
        RuleFor(v => v.GeneralConsentFilterKey.FullName)
            .MaximumLength(100)
            .WithMessage("FullName must be no more than 100 characters long");
        RuleFor(v => v.GeneralConsentFilterKey.PhoneNumber)
            .MaximumLength(10)
            .WithMessage("PhoneNumber must be no more than 10 characters long");
        RuleFor(v => v.GeneralConsentFilterKey.IdCardNumber)
            .MaximumLength(13)
            .WithMessage("IdCardNumber must be no more than 13 characters long");
        RuleFor(v => v.GeneralConsentFilterKey.Email)
            .MaximumLength(100)
            .WithMessage("Email must be no more than 100 characters long");
       
    }
}
