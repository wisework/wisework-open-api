using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Website.Queries.GetWebsite;


public record GetWebsiteInfoQuery(int id) : IRequest<WebsiteActiveList>;


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
            return new WebsiteActiveList();
        }
        return customFieldInfo;
    }
}