using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Models;
using WW.Application.CustomField.Commands.CreateCustomField;
using WW.Application.CustomField.Commands.UpdateCustomField;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Application.Website.Commands.CreateWebsite;
using WW.Application.Website.Commands.UpdateWebsite;
using WW.Application.Website.Queries.GetWebsite;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class WebsiteController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<WebsiteActiveList>>> GetCollectionPointsQuery([FromQuery] GetWebsiteQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<WebsiteActiveList>> Create(CreateWebsiteCommands command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WebsiteActiveList>> Update(int id, UpdateWebsiteCommands command)
    {
        if (id != command.WebsiteId)
        {
            return BadRequest();
        }

        return await Mediator.Send(command);
    }

    [HttpGet("{id}")]
    public async Task<WebsiteActiveList> Get(int id)
    {
        return await Mediator.Send(new GetWebsiteInfoQuery(id));
    }
}
