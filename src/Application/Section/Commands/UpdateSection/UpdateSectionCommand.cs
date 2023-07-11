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

namespace WW.Application.Section.Commands.UpdateSection;
public record UpdateSectionCommand : IRequest<SectionActiveList>
{
    [JsonIgnore]
    public int SectionId { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }

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

        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (request.SectionId <= 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };
            failures.Add(new ValidationFailure("sectionID", "Section ID must be greater than 0"));

            throw new ValidationException(failures);
        }

        var entity = _context.DbSetConsentSectionInfo
            .Where(ppc => ppc.SectionInfoId == request.SectionId && ppc.CompanyId == request.authentication.CompanyID && ppc.Status != Status.X.ToString())
            .FirstOrDefault();

        if (entity == null)
        {
            throw new NotFoundException();
        }

        try
        {
            #region add section and PrimaryKey
            entity.Code = request.Code;
            entity.Description = request.Description;
            entity.Status = request.Status;

            entity.UpdateBy = request.authentication.UserID;
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
            throw new InternalServerException(ex.Message);
        }

      
    }
}
