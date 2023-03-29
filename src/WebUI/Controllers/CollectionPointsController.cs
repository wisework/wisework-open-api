using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WW.Application.CollectionPoints.Queries.GetCollectionPoints;
using WW.Application.Common.Models;

using Wisework.ConsentManagementSystem.Api;
using WW.Application.CollectionPoints.Commands.CreateCollectionPoint;
using WW.Application.CollectionPoints.Commands.UpdateCollectionPoint;

namespace WW.OpenAPI.Controllers;
//[Authorize]
public class CollectionPointsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<CollectionPointInfo>>> GetCollectionPointsQuery([FromQuery] GetCollectionPointsQuery query)
    {
        return await Mediator.Send(query);
    }
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCollectionPointCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateCollectionPointCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
  
    [HttpGet("{id}")]
    public async Task<CollectionPointInfo> Get(int id)
    {
        return (CollectionPointInfo)await Mediator.Send(new GetCollectionPointsInfoQuery(id));

    }
}
