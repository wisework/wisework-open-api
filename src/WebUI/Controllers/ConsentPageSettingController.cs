using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Models;
using WW.Application.ConsentPageSetting.Commands.CreateConsentTheme;
using WW.Application.ConsentPageSetting.Commands.UpdateConsentTheme;
using WW.Application.ConsentPageSetting.Queries.GetConsentTheme;
using WW.Application.ConsentPageSetting.Queries.GetImage;
using WW.Application.ConsentPageSetting.Queries.GetLogo;
using WW.Application.ConsentPageSetting.Queries.GetShortUrl;
using WW.Domain.Entities;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class ConsentPageSettingController : ApiControllerBase
{
    [HttpGet("themes")]
    public async Task<ActionResult<PaginatedList<ConsentTheme>>> GetConsentThemeQuery([FromQuery] GetConsentThemeQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost("theme")]
    public async Task<ActionResult<ConsentTheme>> Create(CreateConsentThemeCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("update-theme/{id}")]
    public async Task<ActionResult<ConsentTheme>> Update(int id, UpdateConsentThemeCommand command)
    {
        if (id != command.ThemeId)
        {
            return BadRequest();
        }

        return await Mediator.Send(command);
    }

    [HttpGet("short-url/{id}")]
    public async Task<ActionResult<ShortUrl>> GetShortUrlQuery(int id)
    {
        return await Mediator.Send(new GetShortUrlQuery(id));
    }

    [HttpGet("logo/{count}")]
    public async Task<ActionResult<List<Image>>> GetLogoQuery(int count)
    {
        return await Mediator.Send(new GetLogoQuery(count));
    }

    [HttpGet("image/{count}")]
    public async Task<ActionResult<List<Image>>> GetImageQuery(int count)
    {
        return await Mediator.Send(new GetImageQuery(count));
    }
}