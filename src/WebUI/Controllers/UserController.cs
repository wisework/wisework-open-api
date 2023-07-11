using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.Common.Models;
using WW.Application.ConsentPageSetting.Queries.GetShortUrl;
using WW.Application.User.Queries.GetUser;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class UserController : ApiControllerBase
{

    [HttpGet]
    [AuthorizationFilter]
    public async Task<UserInfo> GetInfoUser()
    {
        var query = new GetUserInfoQuery();

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);

       
    }
}
