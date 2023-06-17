using Microsoft.AspNetCore.Mvc.Filters;
using WW.Application.Common.Interfaces;
using WW.Infrastructure.Services.Authentication;

namespace Wiskwork.OpenAPI.Filters;

public class AuthorizationFilter : ActionFilterAttribute
{
    private readonly IAuthenticationService _authenticationService = new AuthenticationService();

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (token == null || !_authenticationService.VerifyToken(token))
        {
            throw new UnauthorizedAccessException();
        }

        base.OnActionExecuting(context);
    }
}
