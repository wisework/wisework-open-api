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

namespace WW.Application.CustomField.Queries.GetCustomField;

public record GetCustomFieldQuery : IRequest<PaginatedList<CollectionPointCustomFieldActiveList>>
{
    public int Offset { get; init; } = 1;
    public int Limit { get; init; } = 10;
}
public class GetCustomFieldQueryHandler : IRequestHandler<GetCustomFieldQuery, PaginatedList<CollectionPointCustomFieldActiveList>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetCustomFieldQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<CollectionPointCustomFieldActiveList>> Handle(GetCustomFieldQuery request, CancellationToken cancellationToken)
    {
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Consent_CollectionPointCustomField, CollectionPointCustomFieldActiveList>()
            .ForMember(d => d.Id, a => a.MapFrom(s => s.CollectionPointCustomFieldId))
            .ForMember(d => d.Title, a => a.MapFrom(s => s.Description))
            .ForMember(d => d.InputType, a => a.MapFrom(s => s.Type));
        });

        Mapper mapper = new Mapper(config);

        //todo:edit conpanyid หลังมีการทำ identity server
        PaginatedList<CollectionPointCustomFieldActiveList> model =
            await _context.DbSetConsentCollectionPointCustomFields.Where(p => p.CompanyId == 1 && p.Status == Status.Active.ToString())
            .ProjectTo<CollectionPointCustomFieldActiveList>(mapper.ConfigurationProvider).PaginatedListAsync(request.Offset, request.Limit);

        return model;
    }
}
