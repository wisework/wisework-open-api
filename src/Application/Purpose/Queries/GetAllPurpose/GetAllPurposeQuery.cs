using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Purpose.Queries.GetAllPurpose;

public record GetAllPurposeQuery : IRequest<List<PurposeActiveList>>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
};

public class GetAllPurposeQueryHandler : IRequestHandler<GetAllPurposeQuery, List<PurposeActiveList>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUploadService _uploadService;

    public GetAllPurposeQueryHandler(IApplicationDbContext context, IMapper mapper, IUploadService uploadService)
    {
        _context = context;
        _mapper = mapper;
        _uploadService = uploadService;
    }

    public async Task<List<PurposeActiveList>> Handle(GetAllPurposeQuery request, CancellationToken cancellationToken)
    {

        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        var purposes = (from cf in _context.DbSetConsentPurpose
                        where cf.PurposeId != 0 && cf.CompanyId == request.authentication.CompanyID && cf.Status != Status.X.ToString()
                        select new PurposeActiveList
                           {
                               PurposeID = cf.PurposeId,
                               GUID = new Guid(cf.Guid),
                               PurposeType = cf.PurposeType,
                               CategoryID = cf.PurposeCategoryId,
                               Code = cf.Code,
                               Description = cf.Description,
                               KeepAliveData = cf.KeepAliveData,
                               LinkMoreDetail = cf.LinkMoreDetail,
                               Status = cf.Status,
                               TextMoreDetail = cf.TextMoreDetail,
                               WarningDescription = cf.WarningDescription,
                               ExpiredDateTime = cf.ExpiredDateTime,
                               Language = cf.Language,
                           }).ToList();

        if (purposes.Count == 0)
        {
            throw new NotFoundException();
        }

        try
        {
            return purposes;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    }
}

