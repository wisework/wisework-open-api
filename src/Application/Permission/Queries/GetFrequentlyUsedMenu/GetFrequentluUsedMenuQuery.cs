using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;

namespace WW.Application.Permission.Queries.GetFrequentlyUsedMenu;


public record GetFrequentluUsedMenuQuery(int count) : IRequest<List<FrequentlyUsedMenu>>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GetFrequentluUsedMenuQueryHandler : IRequestHandler<GetFrequentluUsedMenuQuery, List<FrequentlyUsedMenu>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFrequentluUsedMenuQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FrequentlyUsedMenu>> Handle(GetFrequentluUsedMenuQuery request, CancellationToken cancellationToken)
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

        string baseUrl = "https://test-pdpa.thewiseworks.com/";
        string language = "en-US";
        List<int> roleIDs = new List<int> { 4, 50168, 50169 };
        int companyId = request.authentication.CompanyID;
        int userId = request.authentication.UserID;

        var roleIds = userRoles
            .Where(ur => ur.UserID == userId)
            .Select(ur => ur.RoleID)
            .ToList();

        try
        {
            if (request.count < 0)
            {
                List<ValidationFailure> failures = new List<ValidationFailure> { };

                failures.Add(new ValidationFailure("count", "count must be greater than 0"));

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
                         select new FrequentlyUsedMenu
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
                             
                         }
                    ).Take(request.count).ToList();

            if (menus == null) return new List<FrequentlyUsedMenu>();


            if (menus == null)
            {
                throw new NotFoundException("User not found"); // 404 Not Found
            }

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
