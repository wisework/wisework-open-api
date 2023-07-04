using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Application.CustomField.Commands.CreateCustomField;
using WW.Domain.Entities;
using WW.Domain.Enums;
using WW.Domain.Common;

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

    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }

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
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        try
        {
            var entity = new Consent_Purpose();
            var guid = Guid.NewGuid();
          

            string expiredDateTime = Calulate.ExpiredDateTime(request.KeepAliveData, DateTime.Now);

            entity.Guid = guid.ToString();

            entity.PurposeType = request.PurposeType;
            entity.PurposeCategoryId = request.CategoryID;
            entity.Code = request.Code;
            entity.Description = request.Description;

            entity.KeepAliveData = request.KeepAliveData;
            entity.LinkMoreDetail = request.LinkMoreDetail;


            entity.TextMoreDetail = request.TextMoreDetail;
            entity.WarningDescription = request.WarningDescription;

            entity.CreateBy = request.authentication.UserID;
            entity.UpdateBy = request.authentication.UserID;
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;

            entity.Status = Status.Active.ToString();
            entity.Version = 1;
            entity.CompanyId = request.authentication.CompanyID;
            entity.Language = "en-US";
            entity.ExpiredDateTime = $"{expiredDateTime}";

            _context.DbSetConsentPurpose.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            var PurposeInfo = new PurposeActiveList
            {
                GUID = guid,
                PurposeID = entity.PurposeId,
                PurposeType = entity.PurposeType,
                CategoryID = entity.PurposeCategoryId,
                Code = entity.Code,
                Description = entity.Description,
                KeepAliveData = entity.KeepAliveData,
                LinkMoreDetail = entity.LinkMoreDetail,
                Status = entity.Status,
                TextMoreDetail = entity.TextMoreDetail,
                WarningDescription = entity.WarningDescription,
                Language = entity.Language,
                ExpiredDateTime = entity.ExpiredDateTime,

            };

            return PurposeInfo;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }

      
    }
}
