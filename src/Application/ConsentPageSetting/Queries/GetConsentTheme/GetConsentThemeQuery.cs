using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WW.Application.ConsentPageSetting.Queries.GetConsentTheme;

public record GetConsentThemeQuery : IRequest<PaginatedList<ConsentTheme>>
{
    public int offset { get; init; } = 1;
    public int limit { get; init; } = 10;
    public AuthenticationModel? authentication { get; set; }
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
        if(request.authentication == null) 
        {
            throw new UnauthorizedAccessException();
        }

        if(request.offset < 0 || request.limit < 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure>{ };

            if (request.offset < 0)
            {
                failures.Add(new ValidationFailure("Offset", "Offset must be greater than 0"));
            }
            if (request.limit < 0)
            {
                failures.Add(new ValidationFailure("Limit", "Limit must be greater than 0"));
            }

            throw new ValidationException(failures);
        }


        try
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
        catch (Exception ex)
        {
            throw new Exception("Internal Error");
        }

    }
}
