using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.CustomField.Commands.CreateCustomField;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Purpose.Commands.CreatePurpose;
public record CreatePurposeCommand : IRequest<PurposeActiveList>
{
    public int PurposeType { get; init; }
    public int CategoryID { get; init; }
    public string Code { get; init; }
    public string Description { get; init; }
    public string KeepAliveData { get; init; }
    public string LinkMoreDetail { get; init; }
    public string Status { get; init; }
    public string TextMoreDetail { get; init; }
    public string WarningDescription { get; init; }

}

public class CreatePurposeCommandHandler : IRequestHandler<CreatePurposeCommand, PurposeActiveList>
{
    private readonly IApplicationDbContext _context;

    public CreatePurposeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurposeActiveList> Handle(CreatePurposeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Consent_Purpose();
        var guid = Guid.NewGuid();

        entity.Guid = guid.ToString();

        entity.PurposeType = request.PurposeType;
        entity.CategoryID = request.CategoryID;
        entity.Code = request.Code;
        entity.Description = request.Description;
        entity.KeepAliveData = request.KeepAliveData;
        entity.LinkMoreDetail = request.LinkMoreDetail; 
        entity.Status = request.Status;         
        entity.TextMoreDetail = request.TextMoreDetail; 
        entity.WarningDescription = request.WarningDescription;

        entity.CreateBy = 1;
        entity.UpdateBy = 1;
        entity.CreateDate = DateTime.Now;
        entity.UpdateDate = DateTime.Now;

        entity.Status = Status.Active.ToString();
        entity.Version = 1;
        entity.CompanyId = 1;
        entity.Language = "en";

        _context.DbSetConsentPurpose.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        var PurposeInfo = new PurposeActiveList
        {
            GUID = guid,
            PurposeID = entity.PurposeId,
            PurposeType = entity.PurposeType,
            CategoryID = entity.CategoryID,
            Code = entity.Code,
            Description = entity.Description,
            KeepAliveData = entity.KeepAliveData,
            LinkMoreDetail = entity.LinkMoreDetail,
            Status = entity.Status,
            TextMoreDetail = entity.TextMoreDetail,
            WarningDescription = entity.WarningDescription,
            Language = entity.Language,
        };

        return PurposeInfo;
    }
}
