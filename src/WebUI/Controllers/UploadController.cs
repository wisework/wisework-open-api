using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.Common.Models;
using WW.Application.Upload.Commands.CreateUpload;
using WW.OpenAPI.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Wiskwork.OpenAPI.Controllers;
public class UploadController : ApiControllerBase
{
    [HttpPost]
    [AuthorizationFilter]
    public async Task<ActionResult<Response9>> Upload([FromForm] UploadFileCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.authentication = authentication;
        }

        return await Mediator.Send(command);
    }
}
