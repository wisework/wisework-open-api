using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Purpose.Queries.GetPurpose;
public record GetPurposeInfoQuery(int id) : IRequest<PurposeActiveList>;

public class GetPurposeInfoQueryHandler : IRequestHandler<GetPurposeInfoQuery, PurposeActiveList>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPurposeInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PurposeActiveList> Handle(GetPurposeInfoQuery request, CancellationToken cancellationToken)
    {
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Consent_Purpose, PurposeActiveList>();
            cfg.CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
        });

        Mapper mapper = new Mapper(config);

        var purposeInfo = (from cf in _context.DbSetConsentPurpose
                               where cf.PurposeId == request.id && cf.CompanyId == 1 && cf.Status != Status.X.ToString() 
                           select new PurposeActiveList
                               {
                                   PurposeID = cf.PurposeId,
                                   GUID =new Guid(cf.Guid),
                                   PurposeType = cf.PurposeType,
                                   CategoryID = cf.PurposeCategoryId,
                                   Code = cf.Code,
                                   Description = cf.Description,
                                   KeepAliveData = cf.KeepAliveData,
                                   LinkMoreDetail = cf.LinkMoreDetail,
                                   Status = cf.Status ,
                                   TextMoreDetail = cf.TextMoreDetail,
                                   WarningDescription = cf.WarningDescription,
                                   ExpiredDateTime = cf.ExpiredDateTime,
                                   Language = cf.Language,

                                   
                               }).FirstOrDefault();

        if (purposeInfo == null)
        {
            return new PurposeActiveList();
        }
        return purposeInfo;
    }
}
