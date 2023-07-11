using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.Common.Models;
using WW.Application.purpose.Commands.CreatePurpose;
using WW.Application.Purpose.Commands.CreatePurpose;
using WW.Application.Purpose.Commands.UpdatePurpose;
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

    [HttpPut("{code}")]
    [AuthorizationFilter]
    public async Task<ActionResult<PurposeActiveList>> Update(int code, UpdatePurposeCommand command)
    {

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.purposeID = code;
            command.authentication = authentication;
        }
        return await Mediator.Send(command);
    }


    [HttpGet("purpose-info/{code}")]
    [AuthorizationFilter]
    public async Task<PurposeActiveList> Get(int code)
    {
        var query = new GetPurposeInfoQuery(code);

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }
}
