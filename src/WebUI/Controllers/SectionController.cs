using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Models;
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
}