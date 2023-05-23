using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.CompanyProfile.Queies.GetCompany;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;

public class CompanyController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<Company> Get(int id)
    {
        return await Mediator.Send(new GetCompanyQuery(id));
    }
}
