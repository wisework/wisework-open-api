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
    public int Id { get; init; }
    public string Code { get; init; }
    public string Owner { get; init; }
    public string InputType { get; init; }
    public string Title { get; init; }
    public string Placeholder { get; init; }
    public int LengthLimit { get; init; }
    public int MaxLines { get; init; }
    public int MinLines { get; init; }
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
            .Where(cf => cf.CollectionPointCustomFieldId == request.Id && cf.CompanyId == 1 && cf.Status != Status.X.ToString())
            .FirstOrDefault();

        if (entity == null)
        {
            return new CollectionPointCustomFieldActiveList();
        }

        entity.Code = request.Code;
        entity.Owner = request.Owner;
        entity.Type = request.InputType;
        entity.Description = request.Title;
        entity.Placeholder = request.Placeholder;
        entity.LengthLimit = request.LengthLimit;
        entity.MaxLines = request.MaxLines;
        entity.MinLines = request.MinLines;

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
