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

[ApiController]
[Route("api/healthcheck")]
public class HealthCheckController : ApiControllerBase
{
    [HttpGet("readiness")]

    public async Task<ActionResult> Readiness([FromQuery] GeneralConsentQuery query)
    {
        return Ok("OK");
    }

    [HttpGet("liveness")]

    public async Task<ActionResult> Liveness([FromQuery] GeneralConsentQuery query)
    {
        return Ok("OK");
    }

    

}