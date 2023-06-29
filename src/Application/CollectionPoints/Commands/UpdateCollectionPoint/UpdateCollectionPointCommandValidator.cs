
using FluentValidation;
using WW.Application.CollectionPoints.Commands.UpdateCollectionPoint;

namespace WW.Application.CollectionPoints.Commands.CreateCollectionPoint;
public class UpdateCollectionPointCommandValidator : AbstractValidator<UpdateCollectionPointCommand>
{
    public UpdateCollectionPointCommandValidator()
    {
        RuleFor(v => v.CollectionPointName)
            .MaximumLength(1000)
            .NotEmpty()
            .NotNull();
        RuleFor(v => v.WebsiteId)
          .NotNull()
          .NotEmpty();
    }
}
