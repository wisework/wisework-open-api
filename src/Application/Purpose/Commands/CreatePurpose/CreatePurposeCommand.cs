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
using WW.Domain.Common;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Purpose.Commands.CreatePurpose;

public record CreatePurposeCommand : IRequest<PurposeActiveList>
{
    public int purposeType { get; init; }
    public int purposeCategoryId { get; init; }
    public string code { get; init; }
    public string description { get; init; }
    public string keepAliveData { get; init; }
    public string linkMoreDetail { get; init; }
    public string status { get; init; }
    public string textMoreDetail { get; init; }
    public string warningDescription { get; init; }
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

            entity.Guid = Guid.NewGuid().ToString();
            entity.PurposeType = request.purposeType;
            entity.PurposeCategoryId = request.purposeCategoryId;
            entity.Code = request.code;
            entity.Description = request.description;
            entity.KeepAliveData = request.keepAliveData;
            entity.LinkMoreDetail = request.linkMoreDetail;
            entity.TextMoreDetail = request.textMoreDetail;
            entity.WarningDescription = request.warningDescription;

            entity.CreateBy = request.authentication.UserID;
            entity.UpdateBy = request.authentication.UserID;
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;

            entity.Status = Status.Active.ToString();
            entity.Version = 1;
            entity.CompanyId = request.authentication.CompanyID;
            entity.Language = "en";

            _context.DbSetConsentPurpose.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            var PurposeInfo = new PurposeActiveList
            {
                GUID = new Guid(entity.Guid),
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
                ExpiredDateTime = Calulate.ExpiredDateTime(request.keepAliveData, DateTime.Now),
            };

            return PurposeInfo;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    }
}
