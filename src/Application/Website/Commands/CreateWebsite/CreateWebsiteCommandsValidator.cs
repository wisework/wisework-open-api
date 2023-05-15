﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WW.Application.CustomField.Commands.CreateCustomField;

namespace WW.Application.Website.Commands.CreateWebsite;
public class CreateWebsiteCommandsValidator : AbstractValidator<CreateWebsiteCommands>
{
    public CreateWebsiteCommandsValidator()
    {
        RuleFor(v => v.Name)
            .NotNull()
            .WithMessage("Code is required")
            .NotEmpty()
            .WithMessage("Code cannot be empty")
            .MaximumLength(1000)
            .WithMessage("Code must be no more than 1000 characters long");

        RuleFor(v => v.UrlHomePage)
            .NotNull()
            .WithMessage("Code is required")
            .NotEmpty()
            .WithMessage("Code cannot be empty")
            .MaximumLength(1000)
            .WithMessage("Code must be no more than 1000 characters long");

        RuleFor(v => v.UrlPolicyPage)
            .NotNull()
            .WithMessage("Code is required")
            .NotEmpty()
            .WithMessage("Code cannot be empty")
            .MaximumLength(1000)
            .WithMessage("Code must be no more than 1000 characters long");

    }
}

