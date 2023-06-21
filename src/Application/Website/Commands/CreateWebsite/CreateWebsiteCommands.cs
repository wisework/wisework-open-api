using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.CustomField.Commands.CreateCustomField;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Website.Commands.CreateWebsite;


public record CreateWebsiteCommands : IRequest<WebsiteActiveList>
{
    
    public string Name { get; init; }
    public string UrlHomePage { get; init; }
    public string UrlPolicyPage { get; init; }
    public string Status { get; init; }
}

public class CreateWebsiteCommandsHandler : IRequestHandler<CreateWebsiteCommands, WebsiteActiveList>
{
    private readonly IApplicationDbContext _context;

    public CreateWebsiteCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<WebsiteActiveList> Handle(CreateWebsiteCommands request, CancellationToken cancellationToken)
    {
        var entity = new ConsentWebsite();

        entity.Description = request.Name;
        entity.Url = request.UrlHomePage;
        entity.Urlpolicy = request.UrlPolicyPage;

        entity.CreateBy = 1;
        entity.UpdateBy = 1;
        entity.CreateDate = DateTime.Now;
        entity.UpdateDate = DateTime.Now;

        entity.Status = Status.Active.ToString();
        entity.Version = 1;
        entity.CompanyId = 1;

        _context.DbSetConsentWebsite.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        var websiteInfo = new WebsiteActiveList
        {
            WebsiteId = entity.WebsiteId,
            Name = entity.Description,
            UrlHomePage = entity.Url,
            UrlPolicyPage = entity.Urlpolicy,
        };

        return websiteInfo;
    }
}
