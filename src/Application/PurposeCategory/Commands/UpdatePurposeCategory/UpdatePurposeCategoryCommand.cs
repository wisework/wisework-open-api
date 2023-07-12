using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.CollectionPoints.Commands.UpdateCollectionPoint;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Entities;
using WW.Domain.Enums;
using WW.Domain.Exceptions;

namespace WW.Application.PurposeCategory.Commands.UpdatePurposeCategory;
public record UpdatePurposeCategoryCommand : IRequest<PurposeCategoryActiveList>
{
    public int ID { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}
public class UpdatePurposeCategoryCommandHandler : IRequestHandler<UpdatePurposeCategoryCommand, PurposeCategoryActiveList>
{
    private readonly IApplicationDbContext _context;

    public UpdatePurposeCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurposeCategoryActiveList> Handle(UpdatePurposeCategoryCommand request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (request.ID <= 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };
            failures.Add(new ValidationFailure("ID", "ID must be greater than 0"));

            throw new ValidationException(failures);
        }

        var entity = _context.DbSetConsentPurposeCategory
            .Where(ppc => ppc.ID == request.ID && ppc.CompanyID == request.authentication.CompanyID && ppc.Status != Status.X.ToString())
            .FirstOrDefault();

        if (entity == null)
        {
            return new PurposeCategoryActiveList();
        }
        try
        {
            #region add collection point and PrimaryKey

            entity.Description = request.Description;
            entity.UpdateBy = request.authentication.UserID;
            entity.UpdateDate = DateTime.Now;
            entity.Status = request.Status;
            entity.Version += entity.Version;


            _context.DbSetConsentPurposeCategory.Update(entity);
            #endregion

            await _context.SaveChangesAsync(cancellationToken);

            var purposeCreate = new PurposeCategoryActiveList
            {

                Code = entity.Code,
                Description = entity.Description,
                Language = entity.Language,
                Status = entity.Status,
            };
            return purposeCreate;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);

        }

    }
}