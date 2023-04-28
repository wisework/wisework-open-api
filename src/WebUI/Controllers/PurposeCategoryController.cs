using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.CollectionPoints.Commands.CreateCollectionPoint;
using WW.Application.CollectionPoints.Commands.UpdateCollectionPoint;
using WW.Application.CollectionPoints.Queries.GetCollectionPoints;
using WW.Application.Common.Models;
using WW.Application.PurposeCategory.Commands.CreatePurposeCategory;
using WW.Application.PurposeCategory.Commands.UpdatePurposeCategory;
using WW.Application.PurposeCategory.Queries.GetPurposeCategory;
using WW.Domain.Entities;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class PurposeCategoryController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<PurposeCategoryActiveList>>> GetPurposeCategoryQuery([FromQuery] GetPurposeCategoryQuery query)
    {
        return await Mediator.Send(query);
    }
    [HttpPost]
    public async Task<ActionResult<PurposeCategoryActiveList>> Create(CreatePurposeCategoryCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<PurposeCategoryActiveList>> Update(int id, UpdatePurposeCategoryCommand command)
    {
        if (id != command.ID)
        {
            return BadRequest();
        }

       return await Mediator.Send(command);

    }

    [HttpGet("{id}")]
    public async Task<PurposeCategoryActiveList> Get(int id)
    {
        return (PurposeCategoryActiveList)await Mediator.Send(new GetPurposeCategoryInfoQuery(id));

    }

}
