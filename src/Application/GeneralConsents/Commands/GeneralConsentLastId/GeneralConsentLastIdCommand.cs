using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;

namespace WW.Application.GeneralConsents.Commands.GeneralConsentLastId;
public record GeneralConsentLastIdCommand : IRequest<int>
{
    public string? idCardNumber { get; set; }
    public string? fullName { get; set; }
    public string? email { get; set; }
    public string? phoneNumber { get; set; }
    public string collectionPointGuid { get; set; }
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GeneralConsentLastIdCommandHandler : IRequestHandler<GeneralConsentLastIdCommand, int>
{
    private readonly IApplicationDbContext _context;

    public GeneralConsentLastIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GeneralConsentLastIdCommand request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        try
        {
            var query = (from c in _context.DbSetConsent
                            join cp in _context.DbSetConsentCollectionPoints on c.CollectionPointId equals cp.CollectionPointId
                            where cp.Guid == request.collectionPointGuid
                                  && c.CompanyId == request.authentication.CompanyID
                            orderby c.ConsentId descending
                            select new { 
                                ConsentId = c.ConsentId,
                                IdCardNumber = c.IdCardNumber,
                                FullName = c.FullName,
                                PhoneNumber = c.PhoneNumber,
                                Email = c.Email,
                            });

            if (query.Count() == 0)
            {
                throw new NotFoundException();
            }

            #region Filter consent primary key
            if (!string.IsNullOrEmpty(request.idCardNumber))
            {
                query = query.Where(p => p.IdCardNumber == request.idCardNumber);

                if (query.Count() == 0)
                {
                    List<ValidationFailure> failures = new List<ValidationFailure> { };
                    failures.Add(new ValidationFailure("idCardNumber", "No matching ID card number found"));

                    throw new ValidationException(failures);
                }
            }
            if (!string.IsNullOrEmpty(request.fullName))
            {
                query = query.Where(p => p.FullName == request.fullName);

                if (query.Count() == 0)
                {
                    List<ValidationFailure> failures = new List<ValidationFailure> { };
                    failures.Add(new ValidationFailure("fullName", "No matching full name found"));

                    throw new ValidationException(failures);
                }
            }
            if (!string.IsNullOrEmpty(request.phoneNumber))
            {
                query = query.Where(p => p.PhoneNumber == request.phoneNumber);

                if (query.Count() == 0)
                {
                    List<ValidationFailure> failures = new List<ValidationFailure> { };
                    failures.Add(new ValidationFailure("phoneNumber", "No matching phone number found"));

                    throw new ValidationException(failures);
                }
            }
            if (!string.IsNullOrEmpty(request.email))
            {
                query = query.Where(p => p.Email == request.email);

                if (query.Count() == 0)
                {
                    List<ValidationFailure> failures = new List<ValidationFailure> { };
                    failures.Add(new ValidationFailure("email", "No matching email found"));

                    throw new ValidationException(failures);
                }
            }
            #endregion

            var model = query.FirstOrDefault();

            if (model == null)
            {
                throw new NotFoundException();
            }

            return model.ConsentId;
        }
        catch (ValidationException ex)
        {
            throw ex;
        }
        catch (NotFoundException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    }
}
