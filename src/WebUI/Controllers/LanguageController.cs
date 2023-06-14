using MediatR;
using Microsoft.AspNetCore.Mvc;
using WW.Application.Language.Queries;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class LanguageController : ApiControllerBase
{
    [HttpGet("{languageCultureKeys}/{resourceKeys?}")]
    public async Task<Dictionary<string, Dictionary<string, string>>> Get(String languageCultureKeys, String resourceKeys)
    {
        return await Mediator.Send(new GetLanguageQuery(languageCultureKeys, resourceKeys));
    }
}
