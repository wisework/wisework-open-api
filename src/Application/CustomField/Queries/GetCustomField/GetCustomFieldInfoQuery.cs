using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.CustomField.Queries.GetCustomField;

public record GetCustomFieldInfoQuery(int id) : IRequest<CollectionPointCustomFieldActiveList>;

public class GetCustomFieldInfoQueryHandler : IRequestHandler<GetCustomFieldInfoQuery, CollectionPointCustomFieldActiveList>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomFieldInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CollectionPointCustomFieldActiveList> Handle(GetCustomFieldInfoQuery request, CancellationToken cancellationToken)
    {
        MapperConfiguration config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<Consent_CollectionPointCustomField, CollectionPointCustomFieldActiveList>();
        });

        Mapper mapper = new Mapper(config);

        var customFieldInfo = (from cf in _context.DbSetConsentCollectionPointCustomFields
                               where cf.CollectionPointCustomFieldId == request.id && cf.CompanyId == 1 && cf.Status != Status.X.ToString()
                               select new CollectionPointCustomFieldActiveList
                               {
                                   Id = cf.CollectionPointCustomFieldId,
                                   Code = cf.Code,
                                   Owner = cf.Owner,
                                   InputType = cf.Type,
                                   Title = cf.Description,
                                   Placeholder = cf.Placeholder,
                                   LengthLimit = cf.LengthLimit,
                                   MaxLines = cf.MaxLines,
                                   MinLines = cf.MinLines,
                               }).FirstOrDefault();

        if (customFieldInfo == null)
        {
            return new CollectionPointCustomFieldActiveList();
        }
        return customFieldInfo;
    }
}