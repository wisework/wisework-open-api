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
using System.Text.Json.Serialization;
using FluentValidation.Results;
using WW.Application.Common.Exceptions;

namespace WW.Application.PurposeCategory.Queries.GetPurposeCategory;


public record GetPurposeCategoryQuery : IRequest<PaginatedList<PurposeCategoryActiveList>>
{
    public int offset { get; init; } = 1;
    public int limit { get; init; } = 10;

    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
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
                cfg.CreateMap<Consent_PurposeCategory, PurposeCategoryActiveList>().ForMember(d => d.Code, a => a.MapFrom(s => s.Code));
            });

            Mapper mapper = new Mapper(config);

            //todo:edit conpanyid หลังมีการทำ identity server
            PaginatedList<PurposeCategoryActiveList> model =
                await _context.DbSetConsentPurposeCategory.Where(p => p.Status == Status.Active.ToString() && p.CompanyID == request.authentication.CompanyID)
                .ProjectTo<PurposeCategoryActiveList>(mapper.ConfigurationProvider).PaginatedListAsync(request.offset, request.limit);

            return model;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
      
    }
}