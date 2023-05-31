using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Models;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Mappings;
using WW.Domain.Common;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.PurposeCategory.Queries.GetPurposeCategory;


public record GetPurposeCategoryQuery : IRequest<PaginatedList<PurposeCategoryActiveList>>
{
    public int offset { get; init; } = 1;
    public int limit { get; init; } = 10;
}
public class GetPurposeCategoryQueryHandler : IRequestHandler<GetPurposeCategoryQuery, PaginatedList<PurposeCategoryActiveList>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetPurposeCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<PurposeCategoryActiveList>> Handle(GetPurposeCategoryQuery request, CancellationToken cancellationToken)
    {
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Consent_PurposeCategory, PurposeCategoryActiveList>().ForMember(d => d.Code, a => a.MapFrom(s => s.Code));
        });

        Mapper mapper = new Mapper(config);

        //todo:edit conpanyid หลังมีการทำ identity server
        PaginatedList<PurposeCategoryActiveList> model =
            await _context.DbSetConsentPurposeCategory.Where(p => p.Status == Status.Active.ToString() && p.CompanyID == 1)
            .ProjectTo<PurposeCategoryActiveList>(mapper.ConfigurationProvider).PaginatedListAsync(request.offset, request.limit);

        return model;
    }
}