using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.Common.Models;
using WW.Application.CustomField.Commands.CreateCustomField;
using WW.Application.CustomField.Commands.UpdateCustomField;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.OpenAPI.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Wiskwork.OpenAPI.Controllers;

public class CustomFieldController : ApiControllerBase
{
    [HttpGet]
    [AuthorizationFilter]
    public async Task<ActionResult<PaginatedList<CollectionPointCustomFieldActiveList>>> GetCollectionPointsQuery([FromQuery] GetCustomFieldQuery query)
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
    public async Task<ActionResult<CollectionPointCustomFieldActiveList>> Create(CreateCustomFieldCommand command)
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
    public async Task<ActionResult<CollectionPointCustomFieldActiveList>> Update(int id, UpdateCustomFieldCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.id = id;
            command.authentication = authentication;
        }

        return await Mediator.Send(command);
    }

    [HttpGet("{id}")]
    [AuthorizationFilter]
    public async Task<CollectionPointCustomFieldActiveList> Get(int id)
    {
        var query = new GetCustomFieldInfoQuery(id);

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }
}
