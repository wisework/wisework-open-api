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
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Domain.Common;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Purpose.Queries.GetPurpose;

public record GetPurposeQuery : IRequest<PaginatedList<PurposeActiveList>>
{
    public int offset { get; init; } = 1;
    public int limit { get; init; } = 10;
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GetPurposeQueryHandler : IRequestHandler<GetPurposeQuery, PaginatedList<PurposeActiveList>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetPurposeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<PurposeActiveList>> Handle(GetPurposeQuery request, CancellationToken cancellationToken)
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
                cfg.CreateMap<Consent_Purpose, PurposeActiveList>()
             .ForMember(d => d.ExpiredDateTime, a => a.MapFrom(p => Calulate.ExpiredDateTime(p.KeepAliveData, p.CreateDate)))
             .ForMember(d => d.CategoryID, a => a.MapFrom(s => s.PurposeCategoryId))
             ;

                cfg.CreateMap<string, Guid?>().ConvertUsing(value => !string.IsNullOrEmpty(value) ? Guid.Parse(value) : (Guid?)null);
            });

            Mapper mapper = new Mapper(config);

            PaginatedList<PurposeActiveList> model =
                await _context.DbSetConsentPurpose.Where(p => p.CompanyId == request.authentication.CompanyID && p.Status == Status.Active.ToString())
                .ProjectTo<PurposeActiveList>(mapper.ConfigurationProvider).PaginatedListAsync(request.offset, request.limit);

            return model;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    }
}
