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
using WW.Domain.Enums;

namespace WW.Application.Purpose.Commands.UpdatePurpose;
public record UpdatePurposeCommand : IRequest<PurposeActiveList>
{
    [JsonIgnore]
    public int PurposeID { get; set; }
    public int PurposeType { get; init; }
    public int PurposeCategoryId { get; init; }
    public string Description { get; init; }
    public string KeepAliveData { get; init; }
    public string LinkMoreDetail { get; init; }
    public string Status { get; init; }
    public string TextMoreDetail { get; init; }
    public string WarningDescription { get; init; }
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

        if (request.PurposeID <= 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };
            failures.Add(new ValidationFailure("PurposeID", "Purpose ID must be greater than 0"));

            throw new ValidationException(failures);
        }

        var entity = _context.DbSetConsentPurpose
            .Where(cf => cf.PurposeId == request.PurposeID && cf.CompanyId == request.authentication.CompanyID && cf.Status != Status.X.ToString())
            .FirstOrDefault();

        if (entity == null)
        {
            throw new NotFoundException();
        }

        try
        {
            entity.PurposeType = request.PurposeType;
            entity.PurposeCategoryId = request.PurposeCategoryId;

            entity.Description = request.Description;
            entity.KeepAliveData = request.KeepAliveData;
            entity.LinkMoreDetail = request.LinkMoreDetail;
            entity.Status = request.Status;
            entity.TextMoreDetail = request.TextMoreDetail;
            entity.WarningDescription = request.WarningDescription;

            entity.UpdateBy = request.authentication.UserID;
            entity.UpdateDate = DateTime.Now;
            entity.Version += 1;


            _context.DbSetConsentPurpose.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            var purposeInfo = new PurposeActiveList
            {

                PurposeID = entity.PurposeId,
                PurposeType = entity.PurposeType,
                CategoryID = entity.PurposeCategoryId,
                Description = entity.Description,
                KeepAliveData = entity.KeepAliveData,
                LinkMoreDetail = entity.LinkMoreDetail,
                Status = entity.Status,
                TextMoreDetail = entity.TextMoreDetail,
                WarningDescription = entity.WarningDescription,
            };

            return purposeInfo;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }

        
    }
}
