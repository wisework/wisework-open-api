using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Website.Queries.GetWebsite;


public record GetWebsiteInfoQuery(int id) : IRequest<WebsiteActiveList>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
};


public class GetWebsiteInfoQueryHandler : IRequestHandler<GetWebsiteInfoQuery, WebsiteActiveList>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWebsiteInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WebsiteActiveList> Handle(GetWebsiteInfoQuery request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (request.id <= 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };
                     
            failures.Add(new ValidationFailure("websiteID", "Website ID must be greater than 0"));      

            throw new ValidationException(failures);
        }

        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ConsentWebsite, WebsiteActiveList>();
        });

        Mapper mapper = new Mapper(config);

        var customFieldInfo = (from cf in _context.DbSetConsentWebsite
                               where cf.WebsiteId == request.id && cf.CompanyId == 1 && cf.Status != Status.X.ToString()
                               select new WebsiteActiveList
                               {
                                   WebsiteId = request.id,
                                   Name = cf.Description,
                                   UrlHomePage = cf.Url,
                                   UrlPolicyPage = cf.Urlpolicy,

                               }).FirstOrDefault();

        if (customFieldInfo == null)
        {
            throw new NotFoundException();
        }
        try
        {
            return customFieldInfo;

        }
        catch (Exception ex)
        {

            throw new InternalServerException(ex.Message);
        }
        
    }
}