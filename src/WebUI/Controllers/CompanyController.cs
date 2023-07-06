using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.Common.Models;
using WW.Application.CompanyProfile.Queies.GetCompany;
using WW.Application.ConsentPageSetting.Queries.GetShortUrl;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.OpenAPI.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Wiskwork.OpenAPI.Controllers;

public class CompanyController : ApiControllerBase
{
    [HttpGet]
    [AuthorizationFilter]
    public async Task<ActionResult<List<Company>>> Get()
    {
        var query = new GetCompanyQuery();
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }
        return await Mediator.Send(query);
    }
}
