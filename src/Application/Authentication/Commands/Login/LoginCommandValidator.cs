using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace WW.Application.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(v => v.Username)
            .NotNull()
            .WithMessage("Username is required")
            .NotEmpty()
            .WithMessage("Username cannot be empty");

        RuleFor(v => v.Password)
            .NotNull()
            .WithMessage("Password is required")
            .NotEmpty()
            .WithMessage("Password cannot be empty");
    }
}
