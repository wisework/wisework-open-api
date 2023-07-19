using System.Text.Json.Serialization;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Wisework.ConsentManagementSystem.Api;
using Wisework.UploadModule.Interfaces;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;

namespace WW.Application.Upload.Commands.CreateUpload;

public record UploadFileCommand : IRequest<Response9>
{
    public IFormFile fileUpload { get; init; }
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, Response9>
{
    private readonly IApplicationDbContext _context;
    private readonly IUploadProvider _uploadService;

    public UploadFileCommandHandler(IApplicationDbContext context, IUploadProvider uploadService)
    {
        _context = context;
        _uploadService = uploadService;
    }

    public async Task<Response9> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        var file = request.fileUpload;
        var fullFileName = $"{Guid.NewGuid().ToString().ToUpper()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.GetTempFileName();

        if (file.Length > 0)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        var fileType = _context.DbSetFileType.FirstOrDefault(ft => file.ContentType.ToUpper().Contains(ft.Code));

        if (fileType == null)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };

            failures.Add(new ValidationFailure("fileUpload", "Unable to upload this file type"));

            throw new ValidationException(failures);
        }

        try
        {
            var entity = new Domain.Entities.File();

            entity.CompanyId = request.authentication.CompanyID;
            entity.FileCategoryId = 1;
            entity.FileTypeId = fileType.FileTypeId;
            entity.Status = "A";

            entity.CreateBy = request.authentication.UserID;
            entity.CreateDate = DateTime.Now;
            entity.UpdateBy = request.authentication.UserID;
            entity.UpdateDate = DateTime.Now;

            entity.FullFileName = fullFileName;
            entity.ThumbFileName = "";
            entity.OriginalFileName = file.FileName;
            entity.FileSize = Convert.ToInt32(file.Length);

            _context.DbSetFile.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response9 { Id = entity.FileId };
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    }
}
