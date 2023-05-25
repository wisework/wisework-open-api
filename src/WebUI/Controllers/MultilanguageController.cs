
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Application.Language;
using WW.Application.Language.Queries;
using WW.Domain.Entities;
using WW.OpenAPI.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Wiskwork.OpenAPI.Controllers;
public class MultilanguageController : ApiControllerBase
{
    [HttpGet("{languageID}")]
    public async Task<LanguageCulture> Get(int languageID)
    {
        return await Mediator.Send(new GetMultilanguage(languageID));
    }

}
