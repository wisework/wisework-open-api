using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WW.Application.CollectionPoints.Queries.GetCollectionPoints;
using WW.Application.Common.Models;

using Wisework.ConsentManagementSystem.Api;
using WW.Application.CollectionPoints.Commands.CreateCollectionPoint;
using WW.Application.CollectionPoints.Commands.UpdateCollectionPoint;
using Wiskwork.OpenAPI.Filters;
using WW.Application.ConsentPageSetting.Queries.GetShortUrl;

namespace WW.OpenAPI.Controllers;
//[Authorize]
public class CollectionPointsController : ApiControllerBase
{
    [HttpGet("list")]
    [AuthorizationFilter]

    public async Task<ActionResult<PaginatedList<CollectionPointInfo>>> GetCollectionPointsQuery([FromQuery] GetCollectionPointsQuery query)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }
        return await Mediator.Send(query);
    }
    [HttpPost("add")]
    [AuthorizationFilter]

    public async Task<ActionResult<CollectionPointObject>> Create(CreateCollectionPointCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.authentication = authentication;
        }
        return await Mediator.Send(command);
    }
    [HttpPut("update/{id}")]
    [AuthorizationFilter]

    public async Task<ActionResult> Update(int id, UpdateCollectionPointCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.authentication = authentication;
        }
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
  
    [HttpGet("{id}")]
    [AuthorizationFilter]

    public async Task<CollectionPointInfo> GetInfo(int id)
    {
        var query = new GetCollectionPointsInfoQuery(id);

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }
        return await Mediator.Send(query);

    }
}
