﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using static System.Net.WebRequestMethods;

namespace WW.Application.Permission.Queries.GetMenu;
public record GetMenuQuery : IRequest<List<Menu>>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GetMenuQueryHandler : IRequestHandler<GetMenuQuery, List<Menu>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMenuQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Menu>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        var actions = _context.DbSetAction;
        var programs = _context.DbSetProgram;
        var programActions = _context.DbSetProgramAction;
        var programDescriptions = _context.DbSetProgramDescription;
        var roles = _context.DbSetRole;
        var roleProgramActions = _context.DbSetRoleProgramAction;
        var userRoles = _context.DbSetUserRole;
        var userID = _context.DbSetUser;

        string baseUrl = "https://test-pdpa.thewiseworks.com/";
        string language = "en-US";
        int companyId = request.authentication.CompanyID;
        int userId = request.authentication.UserID;

        var roleIds = userRoles
            .Where(ur => ur.UserID == userId)
            .Select(ur => ur.RoleID)
            .ToList();

        List<long> roleIDs = roleIds;

        try
        {
            if (roleIds == null)
            {
                List<ValidationFailure> failures = new List<ValidationFailure> { };

                failures.Add(new ValidationFailure("user", "user not found"));

                throw new ValidationException(failures);
            }

            var menus = (from p1 in programs
                         join pd in programDescriptions.Where(d => d.LanguageCulture == language)
                         on p1.ProgramID equals pd.ProgramID into pdGroup
                         from pd in pdGroup.DefaultIfEmpty()
                         where p1.Status == "A" && (
                            (from rpa in roleProgramActions
                             join r in roles on rpa.RoleID equals r.RoleID
                             join pa in programActions on rpa.ProgramActionID equals pa.ProgramActionID
                             join p2 in programs on pa.ProgramID equals p2.ProgramID
                             join a in actions on pa.ActionID equals a.ActionID
                             where roleIds.Contains(Convert.ToInt32(rpa.RoleID)) && r.CompanyID == companyId && p2.Code == p1.Code && a.Code == "VIEW" && r.Status != "X"
                             select 1
                             ).Count() > 0
                         ) || p1.Code == "HOME_MAIN"
                         orderby p1.Priority
                         let parentId = p1.ProgramID // Store p1.ProgramID in a separate variable
                         select new Menu
                         {
                             Action = baseUrl + p1.Action,
                             Code = p1.Code,
                             Description = p1.Description,
                             ParentID = p1.ParentID,
                             Priority = p1.Priority,
                             ProgramGroupID = 20, // what?
                             ProgramID = p1.ProgramID,
                             IsExpanded = p1.expanded == null || p1.expanded == 0 ? false : true,
                             Icon = p1.Icon,
                             Items = (from childProgram in programs
                                      where childProgram.ParentID == p1.ProgramID
                                      select new SubMenu
                                      {
                                          Action = baseUrl + childProgram.Action,
                                          Code = childProgram.Code,
                                          Description = childProgram.Description,
                                          ParentID = childProgram.ParentID,
                                          Priority = childProgram.Priority,
                                          ProgramGroupID = 20, // what?
                                          ProgramID = childProgram.ProgramID,
                                          IsExpanded = childProgram.expanded == null || childProgram.expanded == 0 ? false : true,
                                          Icon = childProgram.Icon
                                      }).ToList()

                         }).ToList();

        if (menus == null) return new List<Menu>();

        return menus;
        }
        catch (UnauthorizedAccessException)
        {
            throw new UnauthorizedAccessException();
        }
        catch (NotFoundException)
        {
            throw new NotFoundException("User not found"); // 404 Not Found
        }
        catch (ValidationException)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };

            failures.Add(new ValidationFailure("count", "count must be greater than 0"));

            throw new ValidationException(failures);
        }

        catch (Exception ex)
        {
            throw new InternalException("An internal server error occurred."); // 500 error
        }

    }
}
