using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Models;
using WW.Application.Purpose.Queries.GetPurpose;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class PurposeController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<PurposeActiveList>>> GetCollectionPointsQuery([FromQuery] GetPurposeQuery query)
    {
        return await Mediator.Send(query);
    }
}
