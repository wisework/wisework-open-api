using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.Common.Models;
using WW.Application.ConsentPageSetting.Queries.GetShortUrl;
using WW.Application.Permission.Queries.GetFrequentlyUsedMenu;
using WW.Application.Permission.Queries.GetMenu;
using WW.OpenAPI.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Wiskwork.OpenAPI.Controllers;
public class PermissionController : ApiControllerBase
{
    [HttpGet("programs")]
    [AuthorizationFilter]
    public async Task<ActionResult<List<Menu>>> GetMenuQuery()
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        var query = new GetMenuQuery();
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("favorite-menu/{count}")]
    [AuthorizationFilter]
    public async Task<ActionResult<List<FrequentlyUsedMenu>>> GetFrequentluUsedMenuQuery(int count)
    {
        if (count < 0)
        {
            return BadRequest();
        }
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        var query = new GetFrequentluUsedMenuQuery(count);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }
        return await Mediator.Send(query);
    }
}
