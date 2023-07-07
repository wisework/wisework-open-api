using System;
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
            .WithMessage("Name is required")
            .NotEmpty()
            .WithMessage("Name cannot be empty");

        RuleFor(v => v.UrlHomePage)
            .NotNull()
            .WithMessage("Url home page is required")
            .NotEmpty()
            .WithMessage("Url home page cannot be empty");

        RuleFor(v => v.UrlPolicyPage)
            .NotNull()
            .WithMessage("Url policy page is required")
            .NotEmpty()
            .WithMessage("Url policy page cannot be empty");

        RuleFor(v => v.Status)
            .NotNull()
            .WithMessage("Status is required")
            .NotEmpty()
            .WithMessage("Status cannot be empty");      

    }
}


