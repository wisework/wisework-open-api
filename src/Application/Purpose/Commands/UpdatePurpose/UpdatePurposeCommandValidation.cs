using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WW.Application.Purpose.Commands.CreatePurpose;

namespace WW.Application.Purpose.Commands.UpdatePurpose;
internal class UpdatePurposeCommandValidation : AbstractValidator<UpdatePurposeCommand>
{
    public UpdatePurposeCommandValidation()
    {
        RuleFor(v => v.PurposeType)
          .NotEmpty()
          .NotNull();

        RuleFor(v => v.CategoryID)
           .NotEmpty()
           .NotNull();

        RuleFor(v => v.Description)
            .MaximumLength(1000)
           .NotEmpty()
           .NotNull();

        RuleFor(v => v.KeepAliveData)
            .MaximumLength(100)
           .NotEmpty()
           .NotNull();

        RuleFor(v => v.LinkMoreDetail).MaximumLength(1000)
           .NotEmpty()
           .NotNull();

        RuleFor(v => v.Status)
           .NotEmpty()
           .NotNull();

        RuleFor(v => v.TextMoreDetail)
           .NotEmpty()
           .NotNull();

        RuleFor(v => v.WarningDescription)
           .NotEmpty()
           .NotNull();
    }
}
