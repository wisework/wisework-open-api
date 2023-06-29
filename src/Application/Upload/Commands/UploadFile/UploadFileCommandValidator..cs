using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WW.Application.Upload.Commands.CreateUpload;

namespace WW.Application.Upload.Commands.UploadFile;
public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator() 
    {
        RuleFor(v => v.fileUpload)
            .NotNull()
            .WithMessage("File is required")
            .NotEmpty()
            .WithMessage("File cannot be empty");
    }
}
