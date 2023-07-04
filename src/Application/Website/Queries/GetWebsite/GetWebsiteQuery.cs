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

namespace WW.Application.Website.Queries.GetWebsite;
public record GetWebsiteQuery: IRequest<PaginatedList<WebsiteActiveList>>
{
    public int offset { get; init; } = 1;
    public int limit { get; init; } = 10;

    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
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
                cfg.CreateMap<ConsentWebsite, WebsiteActiveList>().ForMember(d => d.WebsiteId, a => a.MapFrom(s => s.WebsiteId))
                .ForMember(d => d.Name, a => a.MapFrom(s => s.Description))
                .ForMember(d => d.UrlHomePage, a => a.MapFrom(s => s.Url))
                .ForMember(d => d.UrlPolicyPage, a => a.MapFrom(s => s.Urlpolicy))
                ;
            });

            Mapper mapper = new Mapper(config);

            //todo:edit conpanyid หลังมีการทำ identity server
            PaginatedList<WebsiteActiveList> model =
                await _context.DbSetConsentWebsite.Where(w => w.CompanyId == 1 && w.Status == Status.Active.ToString()).ProjectTo<WebsiteActiveList>(mapper.ConfigurationProvider).PaginatedListAsync(request.offset, request.limit);

            return model;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }

       
    }
}