using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation.Results;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Mappings;
using WW.Application.Common.Models;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.CustomField.Queries.GetCustomField;

public record GetCustomFieldQuery : IRequest<PaginatedList<CollectionPointCustomFieldActiveList>>
{
    public int offset { get; init; } = 1;
    public int limit { get; init; } = 10;
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
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
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (request.offset <= 0 || request.limit <= 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };

            if (request.offset <= 0)
            {
                failures.Add(new ValidationFailure("offset", "Offset must be greater than 0"));
            }
            if (request.limit <= 0)
            {
                failures.Add(new ValidationFailure("limit", "Limit must be greater than 0"));
            }

            throw new ValidationException(failures);
        }

        try
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Consent_CollectionPointCustomField, CollectionPointCustomFieldActiveList>()
                .ForMember(d => d.Id, a => a.MapFrom(s => s.CollectionPointCustomFieldId))
                .ForMember(d => d.Title, a => a.MapFrom(s => s.Description))
                .ForMember(d => d.InputType, a => a.MapFrom(s => s.Type));
            });

            Mapper mapper = new Mapper(config);

            PaginatedList<CollectionPointCustomFieldActiveList> model =
                await _context.DbSetConsentCollectionPointCustomFields.Where(p => p.CompanyId == request.authentication.CompanyID && p.Status == Status.Active.ToString())
                .ProjectTo<CollectionPointCustomFieldActiveList>(mapper.ConfigurationProvider).PaginatedListAsync(request.offset, request.limit);

            return model;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    }
}
