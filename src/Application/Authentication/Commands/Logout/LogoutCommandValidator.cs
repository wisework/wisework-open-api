using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace WW.Application.Authentication.Commands.Logout;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(v => v.VisitorId)
            .NotNull()
            .WithMessage("Visitor ID is required")
            .NotEmpty()
            .WithMessage("Visitor ID cannot be empty");
    }
}
