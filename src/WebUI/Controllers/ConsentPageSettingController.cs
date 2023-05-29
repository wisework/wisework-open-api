using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Models;
using WW.Application.ConsentPageSetting.Commands.CreateConsentTheme;
using WW.Application.ConsentPageSetting.Commands.UpdateConsentThemeCommand;
using WW.Application.ConsentPageSetting.Queries.GetConsentTheme;
using WW.Application.ConsentPageSetting.Queries.GetImage;
using WW.Application.ConsentPageSetting.Queries.GetLogo;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Domain.Entities;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class ConsentPageSettingController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ConsentTheme>>> GetConsentThemeQuery([FromQuery] GetConsentThemeQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<ConsentTheme>> Create(CreateConsentThemeCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ConsentTheme>> Update(int id, UpdateConsentThemeCommand command)
    {
        if (id != command.ThemeId)
        {
            return BadRequest();
        }

        return await Mediator.Send(command);
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