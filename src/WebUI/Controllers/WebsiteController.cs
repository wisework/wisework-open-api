using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Models;
using WW.Application.Website.Queries.GetWebsite;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class WebsiteController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<WebsiteActiveList>>> GetCollectionPointsQuery([FromQuery] GetWebsiteQuery query)
    {
        return await Mediator.Send(query);
    }
}
