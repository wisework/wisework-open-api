using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Entities;

namespace WW.Application.ConsentPageSetting.Queries.GetAllImage;

public record GetAllImageQuery : IRequest<List<Image>>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GetAllImageHandle : IRequestHandler<GetAllImageQuery, List<Image>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUploadService _uploadService;

    public GetAllImageHandle(IApplicationDbContext context, IUploadService uploadService)
    {
        _context = context;
        _uploadService = uploadService;
    }

    public async Task<List<Image>> Handle(GetAllImageQuery request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
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
                              }).ToList();


            List<Image> images = joinedData.Select(f => new Image
            {
                FullPath = _uploadService.GetStorageBlobUrl(f.FullFileName, "")
            }).ToList();

            return images;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    }
}
