using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Models;
using WW.Application.ConsentPageSetting.Queries.GetConsentTheme;
using WW.Domain.Entities;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers
{
    public class ConsentPageSettingController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ConsentTheme>>> GetConsentThemeQuery([FromQuery] GetConsentThemeQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}