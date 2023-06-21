using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Authentication.Commands.Login;
using WW.Application.Authentication.Commands.Logout;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class AuthenticationController : ApiControllerBase
{
    [HttpPost("login")]
    public async Task<AuthenticationInfo> Login(LoginCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost("logout")]
    public async Task<int> Logout(LogoutCommand command)
    {
        return await Mediator.Send(command);
    }
}
