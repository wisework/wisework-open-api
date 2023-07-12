using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.Common.Models;
using WW.Application.CustomField.Commands.CreateCustomField;
using WW.Application.CustomField.Commands.UpdateCustomField;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Application.Purpose.Queries.GetAllPurpose;
using WW.Application.Purpose.Queries.GetPurpose;
using WW.Application.Website.Commands.CreateWebsite;
using WW.Application.Website.Commands.UpdateWebsite;
using WW.Application.Website.Queries.GetWebsite;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class WebsiteController : ApiControllerBase
{
    [HttpGet]
    [AuthorizationFilter]
    public async Task<ActionResult<PaginatedList<WebsiteActiveList>>> GetCollectionPointsQuery([FromQuery] GetWebsiteQuery query)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }
        return await Mediator.Send(query);
    }

    [HttpPost]
    [AuthorizationFilter]
    public async Task<ActionResult<WebsiteActiveList>> Create(CreateWebsiteCommands command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.authentication = authentication;
        }
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [AuthorizationFilter]
    public async Task<ActionResult<WebsiteActiveList>> Update(int id, UpdateWebsiteCommands command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.WebsiteId = id;
            command.authentication = authentication;
        }

        return await Mediator.Send(command);
    }

    [HttpGet("website-info/{id}")]
    [AuthorizationFilter]
    public async Task<WebsiteActiveList> Get(int id)
    {
        var query = new GetWebsiteInfoQuery(id);

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }
        return await Mediator.Send(query);
    }

}
