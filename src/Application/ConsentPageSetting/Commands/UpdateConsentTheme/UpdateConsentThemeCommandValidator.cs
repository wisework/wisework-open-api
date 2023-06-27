using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace WW.Application.ConsentPageSetting.Commands.UpdateConsentTheme;

public class UpdateConsentThemeCommandValidator : AbstractValidator<UpdateConsentThemeCommand>
{
    public UpdateConsentThemeCommandValidator()
    {
        RuleFor(v => v.themeTitle)
            .NotNull()
            .WithMessage("Theme title is required")
            .NotEmpty()
            .WithMessage("Theme title cannot be empty")
            .MaximumLength(100)
            .WithMessage("Theme title must be no more than 100 characters long");
    }
}
