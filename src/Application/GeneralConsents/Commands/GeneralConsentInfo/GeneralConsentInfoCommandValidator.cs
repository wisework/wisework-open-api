using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace WW.Application.GeneralConsents.Commands.GeneralConsentInfo;
public class GeneralConsentInfoCommandValidator : AbstractValidator<GeneralConsentInfoCommand>
{
    public GeneralConsentInfoCommandValidator()
    {
        RuleFor(v => v.fullName)
            .MaximumLength(100)
            .WithMessage("Full name must be no more than 100 characters long");

        RuleFor(v => v.email)
            .MaximumLength(100)
            .WithMessage("Email must be no more than 100 characters long");

        RuleFor(v => v.phoneNumber)
            .MaximumLength(100)
            .WithMessage("Phone number must be no more than 100 characters long");

        RuleFor(v => v.collectionPointGuid)
            .NotNull()
            .WithMessage("Collection point GUID is required")
            .NotEmpty()
            .WithMessage("Collection point GUID cannot be empty");
    }
}
