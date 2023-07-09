using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.Common.Models;
using WW.Application.ConsentPageSetting.Queries.GetShortUrl;
using WW.Application.Section.Commands.CreateSection;
using WW.Application.Section.Commands.UpdateSection;
using WW.Application.Section.Queries.GetSection;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class SectionController : ApiControllerBase
{
    [HttpGet]
    [AuthorizationFilter]
    public async Task<ActionResult<PaginatedList<SectionActiveList>>> GetCollectionPointsQuery([FromQuery] GetSectionQuery query)
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
    public async Task<ActionResult<SectionActiveList>> Create(CreateSectionCommand command)
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
    public async Task<ActionResult<SectionActiveList>> Update(int id, UpdateSectionCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.SectionId = id;
            command.authentication = authentication;
        }

        return await Mediator.Send(command);

    }

    [HttpGet("{id}")]
    [AuthorizationFilter]
    public async Task<SectionActiveList> Get(int id)
    {
        var query = new GetSectionInfoQuery(id);

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
      
    }



}