﻿using Microsoft.AspNetCore.Mvc;
using Wisework.ConsentManagementSystem.Api;
using Wiskwork.OpenAPI.Filters;
using WW.Application.Common.Models;
using WW.Application.CustomField.Commands.CreateCustomField;
using WW.Application.CustomField.Commands.UpdateCustomField;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Application.Purpose.Commands.CreatePurpose;
using WW.Application.Purpose.Commands.UpdatePurpose;
using WW.Application.Purpose.Queries.GetPurpose;
using WW.OpenAPI.Controllers;

namespace Wiskwork.OpenAPI.Controllers;
public class PurposeController : ApiControllerBase
{
    [HttpGet]
    [AuthorizationFilter]
    public async Task<ActionResult<PaginatedList<PurposeActiveList>>> GetCollectionPointsQuery([FromQuery] GetPurposeQuery query)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }

    [HttpPost]
    [AuthorizationFilter]
    public async Task<ActionResult<PurposeActiveList>> Create(CreatePurposeCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.authentication = authentication;
        }

        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [AuthorizationFilter]
    public async Task<ActionResult<PurposeActiveList>> Update(int id, UpdatePurposeCommand command)
    {
        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            command.id = id;
            command.authentication = authentication;
        }

        return await Mediator.Send(command);
    }

    [HttpGet("purpose-info/{id}")]
    [AuthorizationFilter]
    public async Task<PurposeActiveList> Get(int id)
    {
        var query = new GetPurposeInfoQuery(id);

        HttpContext.Items.TryGetValue("Authentication", out var authenticationObj);
        if (authenticationObj is AuthenticationModel authentication)
        {
            query.authentication = authentication;
        }

        return await Mediator.Send(query);
    }
}
