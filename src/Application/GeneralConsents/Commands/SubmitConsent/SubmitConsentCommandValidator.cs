using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace WW.Application.GeneralConsents.Commands.SubmitConsent;
public class SubmitConsentCommandValidator : AbstractValidator<SubmitConsentCommand>
{
    public SubmitConsentCommandValidator()
    {
        RuleFor(v => v.AgeRangeCode)
            .NotNull()
            .WithMessage("AgeRangeCode is required")
            .NotEmpty()
            .WithMessage("AgeRangeCode cannot be empty")
            .MaximumLength(20)
            .WithMessage("AgeRangeCode must be no more than 20 characters long");
        RuleFor(v => v.CollectionPointGuid)
            .NotNull()
            .WithMessage("CollectionPointGuid is required")
            .NotEmpty()
            .WithMessage("CollectionPointGuid cannot be empty")
            .MaximumLength(36);
        RuleFor(v => v.CompanyId)
            .NotEmpty()
            .WithMessage("CompanyId limit cannot be empty")
            .GreaterThan(0)
            .WithMessage("CompanyId limit must be greater than 0");
        RuleFor(v => v.WebSiteId)
            .NotEmpty()
            .WithMessage("Length limit cannot be empty")
            .GreaterThan(0)
            .WithMessage("Length limit must be greater than 0");
        RuleFor(v => v.Uid)
            .NotEmpty()
            .WithMessage("Length limit cannot be empty")
            .GreaterThan(0)
            .WithMessage("Length limit must be greater than 0");
        RuleFor(v => v.FullName)
           .NotNull()
           .WithMessage("FullName is required")
           .NotEmpty()
           .WithMessage("FullName cannot be empty")
           .MaximumLength(1000)
           .WithMessage("FullName must be no more than 1000 characters long");
        RuleFor(v => v.Email)
           .NotNull()
           .WithMessage("Email is required")
           .NotEmpty()
           .WithMessage("Email cannot be empty")
           .MaximumLength(100)
           .WithMessage("Email must be no more than 100 characters long");
        RuleFor(v => v.PhoneNumber)
         .NotNull()
         .WithMessage("PhoneNumber is required")
         .NotEmpty()
         .WithMessage("PhoneNumber cannot be empty")
         .MaximumLength(10)
         .WithMessage("PhoneNumber must be no more than 10 characters long");
        RuleFor(v => v.IdCardNumber)
         .NotNull()
         .WithMessage("IdCardNumber is required")
         .NotEmpty()
         .WithMessage("IdCardNumber cannot be empty")
         .MaximumLength(13)
         .WithMessage("IdCardNumber must be no more than 13 characters long");
        RuleFor(v => v.Purpose)
         .NotNull();
        RuleFor(v => v.CollectionPointCustomField)
         .NotNull();
   
    }
}
