using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Mappings;
using WW.Application.Common.Models;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.ConsentPageSetting.Queries.GetImage;

public record GetImageQuery(int count) : IRequest<List<Image>>;

public class GetImageHandler : IRequestHandler<GetImageQuery, List<Image>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUploadService _uploadService;

    public GetImageHandler(IApplicationDbContext context, IMapper mapper, IUploadService uploadService)
    {
        _context = context;
        _mapper = mapper;
        _uploadService = uploadService;
    }

    public async Task<List<Image>> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.File> files = _context.DbSetFile.Where(f => f.CompanyId == 1 && f.Status == "A").OrderByDescending(f => f.UpdateDate).ToList();
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
            FullPath = _uploadService.GetStorageBlobUrl(f.FullFileName, "")
        }).ToList();

        return images;
    }
}
