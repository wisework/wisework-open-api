using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace WW.Application.ConsentPageSetting.Commands.CreateConsentTheme;

public class CreateConsentThemeCommandValidator : AbstractValidator<CreateConsentThemeCommand>
{
    public CreateConsentThemeCommandValidator()
    {
        RuleFor(v => v.themeTitle)
            .NotNull()
            .WithMessage("Theme title is required")
            .NotEmpty()
            .WithMessage("Theme title cannot be empty")
            .MaximumLength(100)
            .WithMessage("Theme title must be no more than 100 characters long");

        RuleFor(v => v.headerTextColor)
            .NotNull()
            .WithMessage("Header text color is required")
            .NotEmpty()
            .WithMessage("Header text color cannot be empty");

        RuleFor(v => v.headerBackgroundColor)
            .NotNull()
            .WithMessage("Header background color is required")
            .NotEmpty()
            .WithMessage("Header background color cannot be empty");

        RuleFor(v => v.bodyBackgroundColor)
            .NotNull()
            .WithMessage("Body background color is required")
            .NotEmpty()
            .WithMessage("Body background color cannot be empty");

        RuleFor(v => v.topDescriptionTextColor)
            .NotNull()
            .WithMessage("Top description text color is required")
            .NotEmpty()
            .WithMessage("Top description text color cannot be empty");

        RuleFor(v => v.bottomDescriptionTextColor)
            .NotNull()
            .WithMessage("Bottom description text color is required")
            .NotEmpty()
            .WithMessage("Bottom description text color cannot be empty");

        RuleFor(v => v.policyUrlTextColor)
            .NotNull()
            .WithMessage("Policy URL text color is required")
            .NotEmpty()
            .WithMessage("Policy URL text color cannot be empty");

        RuleFor(v => v.linkToPolicyTextColor)
            .NotNull()
            .WithMessage("Link to policy text color is required")
            .NotEmpty()
            .WithMessage("Link to policy text color cannot be empty");

        RuleFor(v => v.acceptionButtonColor)
            .NotNull()
            .WithMessage("Acception button color is required")
            .NotEmpty()
            .WithMessage("Acception button color cannot be empty");

        RuleFor(v => v.acceptionConsentTextColor)
            .NotNull()
            .WithMessage("Acception consent text color is required")
            .NotEmpty()
            .WithMessage("Acception consent text color cannot be empty");

        RuleFor(v => v.cancelButtonColor)
            .NotNull()
            .WithMessage("Cancel button color is required")
            .NotEmpty()
            .WithMessage("Cancel button color cannot be empty");

        RuleFor(v => v.cancelTextButtonColor)
            .NotNull()
            .WithMessage("Cancel text button color is required")
            .NotEmpty()
            .WithMessage("Cancel text button color cannot be empty");
    }
}
