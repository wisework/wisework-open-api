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

namespace WW.Application.Website.Queries.GetWebsite;
public record GetWebsiteQuery: IRequest<PaginatedList<WebsiteActiveList>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
public class GetWebsiteQueryHandler : IRequestHandler<GetWebsiteQuery, PaginatedList<WebsiteActiveList>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetWebsiteQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<WebsiteActiveList>> Handle(GetWebsiteQuery request, CancellationToken cancellationToken)
    {
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ConsentWebsite, WebsiteActiveList>().ForMember(d=>d.WebsiteId, a=>a.MapFrom(s=>s.WebsiteId))
            ;
        });

        Mapper mapper = new Mapper(config);

        //todo:edit conpanyid หลังมีการทำ identity server
        PaginatedList<WebsiteActiveList> model =
            await _context.DbSetConsentWebsite.Where(w => w.CompanyId == 1 && w.Status == Status.Active.ToString()).ProjectTo<WebsiteActiveList>(mapper.ConfigurationProvider).PaginatedListAsync(request.PageNumber, request.PageSize);

        return model;
    }
}