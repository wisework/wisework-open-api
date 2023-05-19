using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Models;
using WW.Application.Section.Commands.CreateSection;
using WW.Application.Section.Commands.UpdateSection;
using WW.Application.Section.Queries.GetSection;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class SectionController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<SectionActiveList>>> GetCollectionPointsQuery([FromQuery] GetSectionQuery query)
    {
        return await Mediator.Send(query);
    }
    [HttpPost]
    public async Task<ActionResult<SectionActiveList>> Create(CreateSectionCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<SectionActiveList>> Update(int id, UpdateSectionCommand command)
    {
        if (id != command.ID)
        {
            return BadRequest();
        }

       return await Mediator.Send(command);

    }

    [HttpGet("{id}")]
    public async Task<SectionActiveList> Get(int id)
    {
        return (SectionActiveList)await Mediator.Send(new GetSectionInfoQuery(id));

    }



}