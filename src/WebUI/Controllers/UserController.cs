using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.User.Queries.GetUser;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class UserController : ApiControllerBase
{

    [HttpGet("{id}")]
    public async Task<UserInfo> Get(long id)
    {
        return await Mediator.Send(new GetUserInfoQuery(id));
    }
}
