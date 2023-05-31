using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Models;
using WW.Application.Purpose.Commands.CreatePurpose;
using WW.Application.Purpose.Commands.UpdatePurpose;
using WW.Application.Purpose.Queries.GetPurpose;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class PurposeController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<PurposeActiveList>>> GetCollectionPointsQuery([FromQuery] GetPurposeQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<PurposeActiveList>> Create(CreatePurposeCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PurposeActiveList>> Update(int id, UpdatePurposeCommand command)
    {
        if (id != command.PurposeID)
        {
            return BadRequest();
        }

        return await Mediator.Send(command);
    }

    [HttpGet("{id}")]
    public async Task<PurposeActiveList> Get(int id)
    {
        return await Mediator.Send(new GetPurposeInfoQuery(id));
    }
}
