using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.CollectionPoints.Commands.CreateCollectionPoint;
using WW.Application.CollectionPoints.Commands.UpdateCollectionPoint;
using WW.Application.CollectionPoints.Queries.GetCollectionPoints;
using WW.Application.Common.Models;
using WW.Application.ConsentPageSetting.Queries.GetImage;
using WW.Application.PurposeCategory.Commands.CreatePurposeCategory;
using WW.Application.PurposeCategory.Commands.UpdatePurposeCategory;
using WW.Application.PurposeCategory.Queries.GetPurposeCategory;
using WW.Domain.Entities;
using WW.OpenAPI.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Wiskwork.OpenAPI.Controllers;

public class PurposeCategoryController : ApiControllerBase
{
    [HttpGet]
    [AuthorizationFilter]

    public async Task<ActionResult<PaginatedList<PurposeCategoryActiveList>>> GetPurposeCategoryQuery([FromQuery] GetPurposeCategoryQuery query)
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

    public async Task<ActionResult<PurposeCategoryActiveList>> Create(CreatePurposeCategoryCommand command)
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

    public async Task<ActionResult<PurposeCategoryActiveList>> Update(int id, UpdatePurposeCategoryCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.authentication = authentication;
        }
        if (id != command.ID)
        {
            return BadRequest();
        }

        return await Mediator.Send(command);

    }

    [HttpGet("{id}")]
    [AuthorizationFilter]

    public async Task<PurposeCategoryActiveList> Get(int id)
    {
        var query = new GetPurposeCategoryInfoQuery(id);

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }
        return (PurposeCategoryActiveList)await Mediator.Send(query);

    }

}
