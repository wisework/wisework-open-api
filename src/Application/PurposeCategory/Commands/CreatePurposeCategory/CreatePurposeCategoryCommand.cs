using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.PurposeCategory.Commands.CreatePurposeCategory;
public record CreatePurposeCategoryCommand : IRequest<PurposeCategoryActiveList>
{
    public string Code { get; init; }
    public string Description { get; init; }
}

public class CreatePurposeCategoryCommandHandler : IRequestHandler<CreatePurposeCategoryCommand, PurposeCategoryActiveList>
{
    private readonly IApplicationDbContext _context;

    public CreatePurposeCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurposeCategoryActiveList> Handle(CreatePurposeCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Consent_PurposeCategory();
        entity.PurposeCategoryID = 1;
        entity.Code = request.Code;
        entity.Description = request.Description;
        entity.CreateBy = 1;
        entity.UpdateBy = 1;
        entity.UpdateDate = DateTime.Now;
        entity.CreateDate = DateTime.Now;
        entity.Status = Status.Active.ToString();
        entity.Version = 1;
        entity.CompanyID = 1;
        entity.Language = "en";

        _context.DbSetConsentPurposeCategory.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        var purposeInfo = new PurposeCategoryActiveList
        {
            Code = entity.Code,
            Description = entity.Description,
            Language = entity.Language,
            Status = entity.Status,

        };


        return purposeInfo;
    }
}