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

namespace WW.Application.Section.Queries.GetSection;

public record GetSectionQuery : IRequest<PaginatedList<SectionActiveList>>
{
    public int offset { get; init; } = 1;
    public int limit { get; init; } = 10;
}
public class GetSectionQueryHandler : IRequestHandler<GetSectionQuery, PaginatedList<SectionActiveList>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetSectionQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<SectionActiveList>> Handle(GetSectionQuery request, CancellationToken cancellationToken)
    {
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Consent_SectionInfo, SectionActiveList>().ForMember(d => d.SectionId, a => a.MapFrom(s => s.SectionInfoId));
        });

        Mapper mapper = new Mapper(config);

        //todo:edit conpanyid หลังมีการทำ identity server
        PaginatedList<SectionActiveList> model =
            await _context.DbSetConsentSectionInfo.Where(p => p.CompanyId == 1 && p.Status == Status.Active.ToString())
            .ProjectTo<SectionActiveList>(mapper.ConfigurationProvider).PaginatedListAsync(request.offset, request.limit);

        return model;
    }
}