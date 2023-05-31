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
using WW.Domain.Common;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Purpose.Queries.GetPurpose;

public record GetPurposeQuery : IRequest<PaginatedList<PurposeActiveList>>
{
    public int Offset { get; init; } = 1;
    public int Limit { get; init; } = 10;
}
public class GetWebsiteQueryHandler : IRequestHandler<GetPurposeQuery, PaginatedList<PurposeActiveList>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetWebsiteQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<PurposeActiveList>> Handle(GetPurposeQuery request, CancellationToken cancellationToken)
    {
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Consent_Purpose, PurposeActiveList>()
            .ForMember(d => d.ExpiredDateTime, a => a.MapFrom(p => Calulate.ExpiredDateTime(p.KeepAliveData, p.CreateDate)))
            .ForMember(d => d.CategoryID, a => a.MapFrom(s => s.PurposeCategoryId))
            ;

            cfg.CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
        });

        Mapper mapper = new Mapper(config);

        //todo:edit conpanyid หลังมีการทำ identity server
        PaginatedList<PurposeActiveList> model =
            await _context.DbSetConsentPurpose.Where(p => p.CompanyId == 1 && p.Status == Status.Active.ToString())
            .ProjectTo<PurposeActiveList>(mapper.ConfigurationProvider).PaginatedListAsync(request.Offset, request.Limit);

        return model;
    }
}