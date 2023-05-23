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

public record GetImageQuery(int count) : IRequest<Image>;

public class GetImageHandler : IRequestHandler<GetImageQuery, Image>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetImageHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Image> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        /*IUploadService uploadService = FactoryUpload.getService();
        FileDisplay FileDisplay = FileService.GetFileInfo(file, AuthenticationApp.AuthenticationData);

        string url = uploadService.GetStorageBlobUrl(fileName: FileDisplay.FullFileName, path: SEMS.Service.ConfigurationService.ProfileDirectory);*/

        Image image = new Image();
        image.FullPath = "https://test-pdpa.thewiseworks.com/Images/imageName.png";

        return image;
    }
}
