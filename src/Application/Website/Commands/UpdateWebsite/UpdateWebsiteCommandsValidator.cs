using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WW.Application.Website.Commands.CreateWebsite;

namespace WW.Application.Website.Commands.UpdateWebsite;

public class UpdateWebsiteCommandsValidator : AbstractValidator<UpdateWebsiteCommands>
{
    public UpdateWebsiteCommandsValidator()
    {
        RuleFor(v => v.Name)
          .NotNull()
          .WithMessage("Name is required")
          .NotEmpty()
          .WithMessage("Name cannot be empty")
          .MaximumLength(1000)
          .WithMessage("Name must be no more than 1000 characters long");

        RuleFor(v => v.UrlHomePage)
            .NotNull()
            .WithMessage("Url home page is required")
            .NotEmpty()
            .WithMessage("Url home page cannot be empty")
            .MaximumLength(1000)
            .WithMessage("Url home page must be no more than 1000 characters long");

        RuleFor(v => v.UrlPolicyPage)
            .NotNull()
            .WithMessage("Url policy page is required")
            .NotEmpty()
            .WithMessage("Url policy page cannot be empty")
            .MaximumLength(1000)
            .WithMessage("Url policy page must be no more than 1000 characters long");

        RuleFor(v => v.Status)
           .NotNull()
           .WithMessage("Status is required")
           .NotEmpty()
           .WithMessage("Status cannot be empty");

    }
}
