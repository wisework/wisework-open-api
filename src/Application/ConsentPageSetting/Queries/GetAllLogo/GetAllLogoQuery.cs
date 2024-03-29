﻿using System.Text.Json.Serialization;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using Wisework.UploadModule.Interfaces;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Entities;

namespace WW.Application.ConsentPageSetting.Queries.GetAllLogo;

public record GetAllLogoQuery : IRequest<List<Image>>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GetAllLogoHandle : IRequestHandler<GetAllLogoQuery, List<Image>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUploadProvider _uploadProvider;

    public GetAllLogoHandle(IApplicationDbContext context, IUploadProvider uploadProvider)
    {
        _context = context;
        _uploadProvider = uploadProvider;
    }

    public async Task<List<Image>> Handle(GetAllLogoQuery request, CancellationToken cancellationToken)
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
