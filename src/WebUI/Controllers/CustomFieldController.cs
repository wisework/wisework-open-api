using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Models;
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
}
