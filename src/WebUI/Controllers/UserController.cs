using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Application.User.Queries.GetUser;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class UserController : ApiControllerBase
{
    //[HttpGet]
    //public async Task<ActionResult<UserInfo>> Get()
    //{
    //    return await Mediator.Send(new GetUserQuery());
    //}

    [HttpGet("{id}")]
    public async Task<UserInfo> Get(long id)
    {
        return await Mediator.Send(new GetUserQuery(id));
    }
}
