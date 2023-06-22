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

namespace WW.Application.Section.Commands.CreateSection;
public record CreateSectionCommand : IRequest<SectionActiveList>
{
    public string Code { get; init; }
    public string Description { get; init; }
    public string Status { get; init; }
}

public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, SectionActiveList>
{
    private readonly IApplicationDbContext _context;

    public CreateSectionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SectionActiveList> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = new Consent_SectionInfo();

            entity.Code = request.Code;
            entity.Description = request.Description;

            entity.CreateBy = 1;
            entity.UpdateBy = 1;
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;

            entity.Status = Status.Active.ToString();
            entity.Version = 1;
            entity.CompanyId = 1;

            _context.DbSetConsentSectionInfo.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var sectionInfo = new SectionActiveList
            {
                SectionId = entity.SectionInfoId,
                Code = entity.Code,
                Description = entity.Description,
                Status = entity.Status,

            };


            return sectionInfo;
        }
        catch (Exception ex)
        {
            throw new InternalException("An internal server error occurred."); // 500 error
        }
       
    }
}