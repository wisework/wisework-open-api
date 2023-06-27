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

namespace WW.Application.CustomField.Commands.UpdateCustomField;

public record UpdateCustomFieldCommand : IRequest<CollectionPointCustomFieldActiveList>
{
    public int id { get; init; }
    public string code { get; init; }
    public string owner { get; init; }
    public string inputType { get; init; }
    public string title { get; init; }
    public string placeholder { get; init; }
    public int lengthLimit { get; init; }
    public int maxLines { get; init; }
    public int minLines { get; init; }
}

public class UpdateCustomFieldCommandHandler : IRequestHandler<UpdateCustomFieldCommand, CollectionPointCustomFieldActiveList>
{
    private readonly IApplicationDbContext _context;

    public UpdateCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CollectionPointCustomFieldActiveList> Handle(UpdateCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.DbSetConsentCollectionPointCustomFields
            .Where(cf => cf.CollectionPointCustomFieldId == request.id && cf.CompanyId == 1 && cf.Status != Status.X.ToString())
            .FirstOrDefault();

        if (entity == null)
        {
            return new CollectionPointCustomFieldActiveList();
        }

        entity.Code = request.code;
        entity.Owner = request.owner;
        entity.Type = request.inputType;
        entity.Description = request.title;
        entity.Placeholder = request.placeholder;
        entity.LengthLimit = request.lengthLimit;
        entity.MaxLines = request.maxLines;
        entity.MinLines = request.minLines;

        entity.UpdateBy = 1;
        entity.UpdateDate = DateTime.Now;
        entity.Version += 1;

        _context.DbSetConsentCollectionPointCustomFields.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        var customFieldInfo = new CollectionPointCustomFieldActiveList
        {
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
