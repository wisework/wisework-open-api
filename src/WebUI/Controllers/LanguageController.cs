using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wiskwork.OpenAPI.Filters;
using WW.Application.CollectionPoints.Queries.GetCollectionPoints;
using WW.Application.Common.Models;
using WW.Application.ConsentPageSetting.Queries.GetConsentTheme;
using WW.Application.ConsentPageSetting.Queries.GetLogo;
using WW.Application.Language.Queries;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class LanguageController : ApiControllerBase
{
    [HttpGet("{languageCultureKeys}")]
    [AuthorizationFilter]
    public async Task<Dictionary<string, Dictionary<string, string>>> GetResourceQuery(string languageCultureKeys,string resourceKeys)
    {
        var query = new GetLanguageQuery(languageCultureKeys, resourceKeys);

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }
}
