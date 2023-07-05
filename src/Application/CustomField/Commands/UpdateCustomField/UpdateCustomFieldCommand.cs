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
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.CustomField.Commands.UpdateCustomField;

public record UpdateCustomFieldCommand : IRequest<CollectionPointCustomFieldActiveList>
{
    [JsonIgnore]
    public int id { get; set; }
    public string code { get; init; }
    public string owner { get; init; }
    public string inputType { get; init; }
    public string title { get; init; }
    public string placeholder { get; init; }
    public int lengthLimit { get; init; }
    public int maxLines { get; init; }
    public int minLines { get; init; }
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
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
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (request.id <= 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };
            failures.Add(new ValidationFailure("id", "Custom field ID must be greater than 0"));

            throw new ValidationException(failures);
        }

        var entity = _context.DbSetConsentCollectionPointCustomFields
            .Where(cf => cf.CollectionPointCustomFieldId == request.id && cf.CompanyId == request.authentication.CompanyID && cf.Status != Status.X.ToString())
            .FirstOrDefault();

        if (entity == null)
        {
            throw new NotFoundException();
        }

        try
        {
            entity.Code = request.code;
            entity.Owner = request.owner;
            entity.Type = request.inputType;
            entity.Description = request.title;
            entity.Placeholder = request.placeholder;
            entity.LengthLimit = request.lengthLimit;
            entity.MaxLines = request.maxLines;
            entity.MinLines = request.minLines;

            entity.UpdateBy = request.authentication.UserID;
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
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    }
}
