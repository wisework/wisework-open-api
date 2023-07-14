using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.Common.Models;
using WW.Application.ConsentPageSetting.Commands.CreateConsentTheme;
using WW.Application.ConsentPageSetting.Commands.UpdateConsentTheme;
using WW.Application.ConsentPageSetting.Queries.GetAllImage;
using WW.Application.ConsentPageSetting.Queries.GetAllLogo;
using WW.Application.ConsentPageSetting.Queries.GetAllShortURL;
using WW.Application.ConsentPageSetting.Queries.GetConsentTheme;
using WW.Application.ConsentPageSetting.Queries.GetImage;
using WW.Application.ConsentPageSetting.Queries.GetLogo;
using WW.Application.ConsentPageSetting.Queries.GetShortUrl;
using WW.Domain.Entities;
using WW.OpenAPI.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Wiskwork.OpenAPI.Controllers;

public class ConsentPageSettingController : ApiControllerBase
{
    [HttpGet("themes")]
    [AuthorizationFilter]
    public async Task<ActionResult<PaginatedList<ConsentTheme>>> GetConsentThemeQuery([FromQuery] GetConsentThemeQuery query)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }

    [HttpPost("theme")]
    [AuthorizationFilter]
    public async Task<ActionResult<ConsentTheme>> Create(CreateConsentThemeCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.authentication = authentication;
        }

        return await Mediator.Send(command);
    }

    [HttpPut("update-theme/{id}")]
    [AuthorizationFilter]
    public async Task<ActionResult<ConsentTheme>> Update(int id, UpdateConsentThemeCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.themeId = id;
            command.authentication = authentication;
        }

        return await Mediator.Send(command);
    }

    [HttpGet("short-url")]
    [AuthorizationFilter]
    public async Task<ActionResult<List<ShortUrl>>> GetAllShortUrlQuery()
    {
        var query = new GetAllShortURLQuery();

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }

    [HttpGet("short-url/{id}")]
    [AuthorizationFilter]
    public async Task<ActionResult<ShortUrl>> GetShortUrlQuery(int id)
    {
        var query = new GetShortUrlQuery(id);

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }

    [HttpGet("logo")]
    [AuthorizationFilter]
    public async Task<ActionResult<List<Image>>> GetAllLogoQuery()
    {
        var query = new GetAllLogoQuery();

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }

    [HttpGet("logo/{count}")]
    [AuthorizationFilter]
    public async Task<ActionResult<List<Image>>> GetLogoQuery(int count)
    {
        var query = new GetLogoQuery(count);

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }

    [HttpGet("image")]
    [AuthorizationFilter]
    public async Task<ActionResult<List<Image>>> GetAllImageQuery()
    {
        var query = new GetAllImageQuery();

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }

    [HttpGet("image/{count}")]
    [AuthorizationFilter]
    public async Task<ActionResult<List<Image>>> GetImageQuery(int count)
    {
        var query = new GetImageQuery(count);

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }
}