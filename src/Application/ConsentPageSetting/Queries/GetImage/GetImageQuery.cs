using System.Text.Json.Serialization;
using FluentValidation.Results;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using Wisework.UploadModule.Interfaces;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Entities;

namespace WW.Application.ConsentPageSetting.Queries.GetImage;

public record GetImageQuery(int count) : IRequest<List<Image>>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GetImageHandler : IRequestHandler<GetImageQuery, List<Image>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUploadProvider _uploadProvider;

    public GetImageHandler(IApplicationDbContext context, IUploadProvider uploadProvider)
    {
        _context = context;
        _uploadProvider = uploadProvider;
    }

    public async Task<List<Image>> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (request.count < 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };
            failures.Add(new ValidationFailure("count", "Count must be greater than or equal to 0"));

            throw new ValidationException(failures);
        }

        try
        {
            List<Domain.Entities.File> files = _context.DbSetFile.Where(f => f.CompanyId == request.authentication.CompanyID && f.Status == "A").OrderByDescending(f => f.UpdateDate).ToList();
            List<FileType> fileTypes = _context.DbSetFileType.Where(ft => ft.Code == "IMAGE").ToList();

            var joinedData = (from file in files
                              join fileType in fileTypes on file.FileTypeId equals fileType.FileTypeId
                              select new
                              {
                                  FileId = file.FileId,
                                  FullFileName = file.FullFileName,
                                  OriginalFileName = file.OriginalFileName,
                                  Code = fileType.Code,
                                  CreateBy = file.CreateBy,
                                  CreateDate = file.CreateDate,
                                  UpdateBy = file.UpdateBy,
                                  UpdateDate = file.UpdateDate,
                              }).Take(request.count).ToList();


            List<Image> images = joinedData.Select(f => new Image
            {
                FullPath = _uploadProvider.GetURL(f.FullFileName)
            }).ToList();

            return images;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    }
}
