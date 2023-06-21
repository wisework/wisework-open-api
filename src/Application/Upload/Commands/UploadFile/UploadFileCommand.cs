using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;

namespace WW.Application.Upload.Commands.CreateUpload;

public record UploadFileCommand : IRequest<Response9>
{
    public IFormFile fileUpload { get; init; }
}

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, Response9>
{
    private readonly IApplicationDbContext _context;
    private readonly IUploadService _uploadService;

    public UploadFileCommandHandler(IApplicationDbContext context, IUploadService uploadService)
    { 
        _context = context;
        _uploadService = uploadService;
    }

    public async Task<Response9> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var file = request.fileUpload;
        var fullFileName = $"{Guid.NewGuid().ToString().ToUpper()}{Path.GetExtension(file.FileName)}";

        // full path to file in temp location
        var filePath = Path.GetTempFileName();

        if (file.Length > 0)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        // get file type
        var fileType = _context.DbSetFileType.FirstOrDefault(ft => file.ContentType.ToUpper().Contains(ft.Code));

        if (fileType == null)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };

            failures.Add(new ValidationFailure("FileType", "Unable to upload this file type"));

            throw new ValidationException(failures);
        }

        // create new entity
        var entity = new Domain.Entities.File();

        entity.CompanyId = 1;
        entity.FileCategoryId = 1;
        entity.FileTypeId = fileType.FileTypeId;
        entity.Status = "A";

        entity.CreateBy = 1;
        entity.CreateDate = DateTime.Now;
        entity.UpdateBy = 1;
        entity.UpdateDate = DateTime.Now;

        entity.FullFileName = fullFileName;
        entity.ThumbFileName = "";
        entity.OriginalFileName = file.FileName;
        entity.FileSize = Convert.ToInt32(file.Length);

        _context.DbSetFile.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response9 { Id = entity.FileId };
    }
}
