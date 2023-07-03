
using FluentValidation;

namespace WW.Application.CollectionPoints.Commands.CreateCollectionPoint;
public class CreateCollectionPointCommandValidator : AbstractValidator<CreateCollectionPointCommand>
{
    public CreateCollectionPointCommandValidator()
    {
        RuleFor(v => v.CollectionPointName)
            .MaximumLength(1000)
            .NotEmpty()
            .NotNull();

        /*RuleFor(v => v.ConsentKeyIdentifier)
            .NotNull()
            .NotEmpty();*/

       /* RuleFor(v => v.ExpirationPeriod)
            .NotNull()
            .NotEmpty();*/

        /*RuleFor(v => v.Language)
            .NotNull()
            .NotEmpty();*/

        /*RuleFor(v => v.PurposesList)
           .NotNull()
           .NotEmpty();*/

        RuleFor(v => v.WebsiteId)
          .NotNull()
          .NotEmpty();
    }
}
