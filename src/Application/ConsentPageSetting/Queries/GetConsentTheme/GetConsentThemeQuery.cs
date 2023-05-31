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

namespace WW.Application.ConsentPageSetting.Queries.GetConsentTheme;

public record GetConsentThemeQuery : IRequest<PaginatedList<ConsentTheme>>
{
    public int offset { get; init; } = 1;
    public int limit { get; init; } = 10;
}
public class GetConsentThemeHandler : IRequestHandler<GetConsentThemeQuery, PaginatedList<ConsentTheme>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetConsentThemeHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<ConsentTheme>> Handle(GetConsentThemeQuery request, CancellationToken cancellationToken)
    {
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Consent_ConsentTheme, ConsentTheme>()
            .ForMember(d => d.HerderTextColor, a => a.MapFrom(s => s.HeaderTextColor));
        });

        Mapper mapper = new Mapper(config);

        PaginatedList<ConsentTheme> model =
            await _context.DbSetConsentTheme.Where(p => p.CompanyId == 1 && p.Status == Status.Active.ToString())
            .ProjectTo<ConsentTheme>(mapper.ConfigurationProvider).PaginatedListAsync(request.offset, request.limit);

        return model;
    }
}
