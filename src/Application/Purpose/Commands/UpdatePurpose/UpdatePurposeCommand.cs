using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Application.CustomField.Commands.UpdateCustomField;
using WW.Domain.Common;
using WW.Domain.Enums;

namespace WW.Application.Purpose.Commands.UpdatePurpose;


public record UpdatePurposeCommand : IRequest<PurposeActiveList>
{
    [JsonIgnore]
    public int id { get; set; }
    public int purposeType { get; init; }
    public int purposeCategoryId { get; init; }
    public string description { get; init; }
    public string keepAliveData { get; init; }
    public string linkMoreDetail { get; init; }
    public string status { get; init; }
    public string textMoreDetail { get; init; }
    public string warningDescription { get; init; }
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
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
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (request.id <= 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };
            failures.Add(new ValidationFailure("purposeID", "Purpose ID must be greater than 0"));

            throw new ValidationException(failures);
        }

        var entity = _context.DbSetConsentPurpose
            .Where(cf => cf.PurposeId == request.id && cf.CompanyId == request.authentication.CompanyID && cf.Status != Status.X.ToString())
            .FirstOrDefault();

        if (entity == null)
        {
            throw new NotFoundException();
        }

        try
        {
            entity.Guid = entity.Guid;
            entity.PurposeType = request.purposeType;
            entity.PurposeCategoryId = request.purposeCategoryId;
            entity.Code = entity.Code;
            entity.Description = request.description;
            entity.KeepAliveData = request.keepAliveData;
            entity.LinkMoreDetail = request.linkMoreDetail;
            entity.Status = request.status;
            entity.TextMoreDetail = request.textMoreDetail;
            entity.WarningDescription = request.warningDescription;
            entity.Language = entity.Language;

            entity.UpdateBy = request.authentication.UserID;
            entity.UpdateDate = DateTime.Now;
            entity.Version += 1;

            var purpose = (from cf in _context.DbSetConsentPurpose
                               where cf.PurposeId == request.id && cf.CompanyId == request.authentication.CompanyID && cf.Status != Status.X.ToString()
                               select new PurposeActiveList
                               {
                                   PurposeID = cf.PurposeId,
                                   GUID = new Guid(cf.Guid),
                                   PurposeType = cf.PurposeType,
                                   CategoryID = cf.PurposeCategoryId,
                                   Code = cf.Code,
                                   Description = cf.Description,
                                   KeepAliveData = cf.KeepAliveData,
                                   LinkMoreDetail = cf.LinkMoreDetail,
                                   Status = cf.Status,
                                   TextMoreDetail = cf.TextMoreDetail,
                                   WarningDescription = cf.WarningDescription,
                                   ExpiredDateTime = Calulate.ExpiredDateTime(cf.KeepAliveData, cf.CreateDate),
                                   Language = cf.Language,
                               }).FirstOrDefault();

            if (purpose == null)
            {
                throw new NotFoundException();
            }

            String ExpiredDateTime;

            if(request.keepAliveData == purpose.KeepAliveData)
            {
                ExpiredDateTime = Calulate.ExpiredDateTime(entity.KeepAliveData, entity.CreateDate);
            }
            else
            {
                ExpiredDateTime = Calulate.ExpiredDateTime(entity.KeepAliveData, DateTime.Now);
            }

            _context.DbSetConsentPurpose.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            var purposeInfo = new PurposeActiveList
            {
                PurposeID = entity.PurposeId,
                GUID = new Guid(entity.Guid),
                PurposeType = entity.PurposeType,
                CategoryID = entity.PurposeCategoryId,
                Code = entity.Code,
                Description = entity.Description,
                KeepAliveData = entity.KeepAliveData,
                LinkMoreDetail = entity.LinkMoreDetail,
                Status = entity.Status,
                TextMoreDetail = entity.TextMoreDetail,
                WarningDescription = entity.WarningDescription,
                ExpiredDateTime = ExpiredDateTime,
                Language = entity.Language,
            };

            return purposeInfo;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    }
}
