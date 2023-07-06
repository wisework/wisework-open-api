using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Application.CustomField.Commands.UpdateCustomField;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Website.Commands.UpdateWebsite;


public record UpdateWebsiteCommands : IRequest<WebsiteActiveList>
{
    [JsonIgnore]
    public int WebsiteId { get; set; }
    public string Name { get; init; }
    public string UrlHomePage { get; init; }
    public string UrlPolicyPage { get; init; }
    public string Status { get; init; }

    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
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

        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (request.WebsiteId <= 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };
            failures.Add(new ValidationFailure("PurposeID", "Purpose ID must be greater than 0"));

            throw new ValidationException(failures);
        }

        var entity = _context.DbSetConsentWebsite
             .Where(cf => cf.WebsiteId == request.WebsiteId && cf.CompanyId == request.authentication.CompanyID && cf.Status != Status.X.ToString())
             .FirstOrDefault();

        if (entity == null)
        {
            throw new NotFoundException();
        }

        try
        {
            entity.Description = request.Name;
            entity.Url = request.UrlHomePage;
            entity.Urlpolicy = request.UrlPolicyPage;
            entity.Status = Status.Active.ToString();

            entity.UpdateBy = request.authentication.UserID;
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
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }

       
    }
}
