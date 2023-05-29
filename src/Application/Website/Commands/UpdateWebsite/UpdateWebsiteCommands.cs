using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.CustomField.Commands.UpdateCustomField;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Website.Commands.UpdateWebsite;


public record UpdateWebsiteCommands : IRequest<WebsiteActiveList>
{
    public int WebsiteId { get; init; }
    public string Name { get; init; }
    public string UrlHomePage { get; init; }
    public string UrlPolicyPage { get; init; }
    public string Status { get; init; }
}

public class UpdateWebsiteCommandsHandler : IRequestHandler<UpdateWebsiteCommands, WebsiteActiveList>
{
    private readonly IApplicationDbContext _context;

    public UpdateWebsiteCommandsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WebsiteActiveList> Handle(UpdateWebsiteCommands request, CancellationToken cancellationToken)
    {
        var entity = _context.DbSetConsentWebsite
             .Where(cf => cf.WebsiteId == request.WebsiteId && cf.CompanyId == 1 && cf.Status != Status.X.ToString())
             .FirstOrDefault();

        if (entity == null)
        {
            return new WebsiteActiveList();
        }

        entity.Description = request.Name;
        entity.Url = request.UrlHomePage;
        entity.Urlpolicy = request.UrlPolicyPage;
        entity.Status = Status.Active.ToString();

        entity.UpdateBy = 1;
        entity.UpdateDate = DateTime.Now;

        
        entity.Version = 1;
        

        _context.DbSetConsentWebsite.Update(entity);

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
