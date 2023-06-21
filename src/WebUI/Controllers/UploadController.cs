using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Upload.Commands.CreateUpload;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;
public class UploadController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Response9>> Upload([FromForm] UploadFileCommand command)
    {
        return await Mediator.Send(command);
    }
}
