using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Section.Commands.UpdateSection;
public record UpdateSectionCommand : IRequest<SectionActiveList>
{
    public int ID { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }

}
public class UpdateSectionCommandHandler : IRequestHandler<UpdateSectionCommand, SectionActiveList>
{
    private readonly IApplicationDbContext _context;

    public UpdateSectionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SectionActiveList> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {

        try
        {
            var entity = _context.DbSetConsentSectionInfo
           .Where(ppc => ppc.SectionInfoId == request.ID && ppc.CompanyId == 1 && ppc.Status != Status.X.ToString())
           .FirstOrDefault();

            if (entity == null)
            {
                return new SectionActiveList();
            }

            #region add section and PrimaryKey
            entity.Code = request.Code;
            entity.Description = request.Description;
            entity.Status = request.Status;

            entity.UpdateBy = 1;
            entity.UpdateDate = DateTime.Now;


            _context.DbSetConsentSectionInfo.Update(entity);
            #endregion

            await _context.SaveChangesAsync(cancellationToken);

            var sectionupdate = new SectionActiveList
            {
                SectionId = entity.SectionInfoId,
                Code = entity.Code,
                Description = entity.Description,
                Status = entity.Status,
            };
            return sectionupdate;
        }
        catch (Exception ex)
        {
            throw new InternalException("An internal server error occurred."); // 500 error
        }
      
    }
}
