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
using WW.Domain.Common;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.PurposeCategory.Queries.GetPurposeCategory;
public record GetPurposeCategoryInfoQuery(int Id) : IRequest<PurposeCategoryActiveList>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GetPurposeCategoryInfoQueryHandler : IRequestHandler<GetPurposeCategoryInfoQuery, PurposeCategoryActiveList>
{
    private readonly IApplicationDbContext _context;

    public GetPurposeCategoryInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
    }

    public async Task<PurposeCategoryActiveList> Handle(GetPurposeCategoryInfoQuery request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }
        if (request.Id == null)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };
            failures.Add(new ValidationFailure("ID", "ID can't be null"));

            throw new ValidationException(failures);
        }
        if (request.Id <= 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };
            failures.Add(new ValidationFailure("ID", "ID must be greater than 0"));

            throw new ValidationException(failures);
        }
        try
        {
            //todo:edit conpanyid หลังมีการทำ identity server
            var purposecategoryInfo = (from ppc in _context.DbSetConsentPurposeCategory
                                       join uCreate in _context.DbSetUser on ppc.CreateBy equals uCreate.CreateBy
                                       join uUpdate in _context.DbSetUser on ppc.CreateBy equals uUpdate.UpdateBy
                                       join c in _context.DbSetCompanies on ppc.CompanyID equals (int)(long)c.CompanyId
                                       join w in _context.DbSetConsentWebsite on (int)(long)c.CompanyId equals w.CompanyId
                                       where ppc.ID == request.Id && ppc.CompanyID == request.authentication.CompanyID && ppc.Status == "Active"
                                       select new PurposeCategoryActiveList
                                       {
                                           Code = ppc.Code,
                                           Description = ppc.Description,
                                           Language = ppc.Language,
                                           Status = ppc.Status

                                       }).FirstOrDefault();
            if (purposecategoryInfo == null)
            {
                throw new NotFoundException();
            }
            return purposecategoryInfo;
        }
        catch (NotFoundException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);

        }
     
    }

}