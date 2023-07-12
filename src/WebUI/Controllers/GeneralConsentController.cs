using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.Common.Models;
using WW.Application.GeneralConsents.Commands;
using WW.Application.GeneralConsents.Commands.GeneralConsentFilterQuery;
using WW.Application.GeneralConsents.Commands.GeneralConsentInfo;
using WW.Application.GeneralConsents.Commands.GeneralConsentLastId;
using WW.Application.GeneralConsents.Queries;
using WW.OpenAPI.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Wiskwork.OpenAPI.Controllers;

public class GeneralConsentController : ApiControllerBase
{
    [HttpGet]
    [Route("list", Name = "List")]
    [AuthorizationFilter]

    public async Task<ActionResult<PaginatedList<GeneralConsent>>> GeneralConsentQuery([FromQuery] GeneralConsentQuery query)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Route("list-with-filter", Name = "ListWithFilter")]
    [AuthorizationFilter]

    public async Task<ActionResult<PaginatedList<GeneralConsent>>> GetGeneralConsentQuery( GeneralConsentFilterQueryCommand query)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }
        return  await Mediator.Send(query);
    }

    [HttpPost]
    [Route("submit", Name = "Submitconsent")]
    [AuthorizationFilter]

    public async Task<ActionResult<SubmitGeneralConsentResponse>> Create(SubmitConsentCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.authentication = authentication;
        }
        return await Mediator.Send(command);
    }

    [HttpPost]
    [Route("latest-id", Name = "LatestId")]
    [AuthorizationFilter]

    public async Task<int> GetLatestId(GeneralConsentLastIdCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.authentication = authentication;
        }
        return await Mediator.Send(command);
    }

    [HttpPost]
    [Route("info", Name = "ConsentInfo")]
    [AuthorizationFilter]

    public async Task<ActionResult<GeneralConsent>> GetGeneralConsentInfoQuery(GeneralConsentInfoCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.authentication = authentication;
        }
        return await Mediator.Send(command);
    }

}