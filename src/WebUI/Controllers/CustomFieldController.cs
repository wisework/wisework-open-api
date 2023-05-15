using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Models;
using WW.Application.CustomField.Commands.CreateCustomField;
using WW.Application.CustomField.Commands.UpdateCustomField;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class CustomFieldController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<CollectionPointCustomFieldActiveList>>> GetCollectionPointsQuery([FromQuery] GetCustomFieldQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<CollectionPointCustomFieldActiveList>> Create(CreateCustomFieldCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CollectionPointCustomFieldActiveList>> Update(int id, UpdateCustomFieldCommand command)
    {
        if(id != command.Id)
        {
            return BadRequest();
        }

        return await Mediator.Send(command);
    }

    [HttpGet("{id}")]
    public async Task<CollectionPointCustomFieldActiveList> Get(int id)
    {
        return await Mediator.Send(new GetCustomFieldInfoQuery(id));
    }
}
