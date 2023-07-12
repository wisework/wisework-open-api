using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.Common.Models;
using WW.Application.ConsentPageSetting.Queries.GetAllImage;
using WW.Application.purpose.Commands.CreatePurpose;
using WW.Application.Purpose.Commands.CreatePurpose;
using WW.Application.Purpose.Commands.UpdatePurpose;
using WW.Application.Purpose.Queries.GetAllPurpose;
using WW.Application.Purpose.Queries.GetPurpose;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class PurposeController : ApiControllerBase
{
    [HttpGet]
    [AuthorizationFilter]
    public async Task<ActionResult<PaginatedList<PurposeActiveList>>> GetCollectionPointsQuery([FromQuery] GetPurposeQuery query)
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
    public async Task<ActionResult<PurposeActiveList>> Create(CreatePurposeCommand command)
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
    public async Task<ActionResult<PurposeActiveList>> Update(int id, UpdatePurposeCommand command)
    {

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.purposeID = id;
            command.authentication = authentication;
        }
        return await Mediator.Send(command);
    }


    [HttpGet("purpose-info/{id}")]
    [AuthorizationFilter]
    public async Task<PurposeActiveList> Get(int id)
    {
        var query = new GetPurposeInfoQuery(id);

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }

    [HttpGet("purpose-info")]
    [AuthorizationFilter]
    public async Task<ActionResult<List<PurposeActiveList>>> GetAllPurposeQuery()
    {
        var query = new GetAllPurposeQuery();

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }
}
