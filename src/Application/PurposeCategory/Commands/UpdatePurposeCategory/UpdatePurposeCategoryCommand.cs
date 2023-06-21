using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.CollectionPoints.Commands.UpdateCollectionPoint;
using WW.Application.Common.Interfaces;
using WW.Domain.Entities;
using WW.Domain.Enums;
using WW.Domain.Exceptions;

namespace WW.Application.PurposeCategory.Commands.UpdatePurposeCategory;
public record UpdatePurposeCategoryCommand : IRequest<PurposeCategoryActiveList>
{
    public int ID { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }

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
        var entity = _context.DbSetConsentPurposeCategory
            .Where(ppc => ppc.ID == request.ID && ppc.CompanyID == 1 && ppc.Status != Status.X.ToString())
            .FirstOrDefault();

        if (entity == null)
        {
            return new PurposeCategoryActiveList();
        }

        #region add collection point and PrimaryKey

        entity.Description = request.Description;
        entity.UpdateBy = 1;
        entity.UpdateDate = DateTime.Now;
        entity.Status = request.Status;


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
}