using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.CustomField.Commands.CreateCustomField;

public record CreateCustomFieldCommand : IRequest<CollectionPointCustomFieldActiveList>
{
    public string Code { get; init; }
    public string Owner { get; init; }
    public string InputType { get; init; }
    public string Title { get; init; }
    public string Placeholder { get; init; }
    public int LengthLimit { get; init; }
    public int MaxLines { get; init; }
    public int MinLines { get; init; }
}

public class CreateCustomFieldCommandHandler : IRequestHandler<CreateCustomFieldCommand, CollectionPointCustomFieldActiveList>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CollectionPointCustomFieldActiveList> Handle(CreateCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = new Consent_CollectionPointCustomField();
        
        entity.Code = request.Code;
        entity.Owner = request.Owner;
        entity.Type = request.InputType;
        entity.Description = request.Title;
        entity.Placeholder = request.Placeholder;
        entity.LengthLimit = request.LengthLimit;
        entity.MaxLines = request.MaxLines;
        entity.MinLines = request.MinLines;

        entity.CreateBy = 1;
        entity.UpdateBy = 1;
        entity.CreateDate = DateTime.Now;
        entity.UpdateDate = DateTime.Now;

        entity.Status = Status.Active.ToString();
        entity.Version = 1;
        entity.CompanyId = 1;

        _context.DbSetConsentCollectionPointCustomFields.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        var customFieldInfo = new CollectionPointCustomFieldActiveList{
            Id = entity.CollectionPointCustomFieldId,
            Code = entity.Code,
            Owner = entity.Owner,
            InputType = entity.Type,
            Title = entity.Description,
            Placeholder = entity.Placeholder,
            LengthLimit = entity.LengthLimit,
            MaxLines = entity.MaxLines,
            MinLines = entity.MinLines,
        };

        return customFieldInfo;
    }
}
