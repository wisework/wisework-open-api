using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.CollectionPoints.Queries.GetCollectionPoints;
using WW.Application.Common.Models;
using WW.Application.GeneralConsents.Commands;
using WW.Application.GeneralConsents.Queries;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class GeneralConsentController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<GeneralConsent>>> GetGeneralConsentQuery([FromQuery] GeneralConsentListRequestQuery query)
    {
        return  await Mediator.Send(query);
    }

    [HttpPost]
    [Route("submit", Name = "Submitconsent")]
    public async Task<ActionResult<int>> Create(SubmitConsentCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost]
    [Route("latest-id", Name = "LatestId")]
    public async Task<int> GetLatestId(GetLatestIdRequestQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Route("info", Name = "ConsentInfo")]
    public async Task<ActionResult<GeneralConsent>> GetGeneralConsentInfoQuery(GeneralConsentInfoRequestQuery query)
    {
        return await Mediator.Send(query);
    }

}