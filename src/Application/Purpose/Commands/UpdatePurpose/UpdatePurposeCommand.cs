using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Domain.Enums;

namespace WW.Application.Purpose.Commands.UpdatePurpose;
public record UpdatePurposeCommand : IRequest<PurposeActiveList>
{
    public int PurposeID { get; set; }
    public int PurposeType { get; init; }
    public int CategoryID { get; init; }
    public string Description { get; init; }
    public string KeepAliveData { get; init; }
    public string LinkMoreDetail { get; init; }
    public string Status { get; init; }
    public string TextMoreDetail { get; init; }
    public string WarningDescription { get; init; }
}

public class UpdatePurposeCommandHandler : IRequestHandler<UpdatePurposeCommand, PurposeActiveList>
{
    private readonly IApplicationDbContext _context;

    public UpdatePurposeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurposeActiveList> Handle(UpdatePurposeCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.DbSetConsentPurpose
            .Where(cf => cf.PurposeId == request.PurposeID && cf.CompanyId == 1 && cf.Status != Status.X.ToString())
            .FirstOrDefault();

        if (entity == null)
        {
            return new PurposeActiveList();
        }

        entity.PurposeType = request.PurposeType;
        entity.CategoryID = request.CategoryID;
        
        entity.Description = request.Description;
        entity.KeepAliveData = request.KeepAliveData;
        entity.LinkMoreDetail = request.LinkMoreDetail;
        entity.Status = request.Status;
        entity.TextMoreDetail = request.TextMoreDetail;
        entity.WarningDescription = request.WarningDescription;

        entity.UpdateBy = 1;
        entity.UpdateDate = DateTime.Now;
        entity.Version += 1;
        

        _context.DbSetConsentPurpose.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        var purposeInfo = new PurposeActiveList
        {
            
            PurposeID = entity.PurposeId,
            PurposeType = entity.PurposeType,
            CategoryID = entity.CategoryID,
            Description = entity.Description,
            KeepAliveData = entity.KeepAliveData,
            LinkMoreDetail = entity.LinkMoreDetail,
            Status = entity.Status,
            TextMoreDetail = entity.TextMoreDetail,
            WarningDescription = entity.WarningDescription,
        };

        return purposeInfo;
    }
}
