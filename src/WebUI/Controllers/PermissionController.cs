using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Permission.Queries.GetMenu;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;
public class PermissionController : ApiControllerBase
{
    [HttpGet("programs")]
    public async Task<ActionResult<List<Menu>>> GetMenuQuery()
    {
        return await Mediator.Send(new GetMenuQuery());
    }
}
