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
using WW.Domain.Common;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.GeneralConsents.Commands.GeneralConsentInfo;
public record GeneralConsentInfoCommand : IRequest<GeneralConsent>
{
    public string idCardNumber { get; set; }
    public string fullName { get; set; }
    public string email { get; set; }
    public string phoneNumber { get; set; }
    public string collectionPointGuid { get; set; }
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GeneralConsentInfoHandler : IRequestHandler<GeneralConsentInfoCommand, GeneralConsent>
{
    private readonly IApplicationDbContext _context;

    public GeneralConsentInfoHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GeneralConsent> Handle(GeneralConsentInfoCommand request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        try
        {
            var query = (from c in _context.DbSetConsent
                         join cp in _context.DbSetConsentCollectionPoints on c.CollectionPointId equals cp.CollectionPointId
                         join w in _context.DbSetConsentWebsite on c.WebsiteId equals w.WebsiteId
                         join company in _context.DbSetCompanies on c.CompanyId.Value equals company.CompanyId
                         join uCreate in _context.DbSetUser on cp.CreateBy equals uCreate.CreateBy
                         join uUpdate in _context.DbSetUser on cp.CreateBy equals uUpdate.UpdateBy
                         where c.CompanyId == request.authentication.CompanyID && c.New == 1
                         && cp.Guid == request.collectionPointGuid
                         select new GeneralConsent
                         {
                             ConsentId = c.ConsentId,
                             CollectionPointId = c.CollectionPointId,
                             Uid = c.Uid,
                             TotalTransactions = c.TotalTransactions,
                             FullName = c.FullName,
                             CollectionPointGuid = cp.Guid,
                             ConsentDateTime = c.ConsentDatetime.ToString(),
                             CollectionPointVersion = cp.Version,
                             FromBrowser = c.FromBrowser,
                             PhoneNumber = c.PhoneNumber,
                             IdCardNumber = c.IdCardNumber,
                             Email = c.Email,
                             Remark = c.Remark,
                             CompanyId = c.CompanyId,
                             CompanyName = company.Name,
                             Status = c.Status,
                             VerifyType = c.VerifyType,
                         });

            if (query == null)
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

            var consentIdIds = query.Select(c => c.ConsentId);
            var collectionPointIds = query.Select(c => c.CollectionPointId);

            if (consentIdIds.ToList().Count > 0)
            {
                #region fetch website
                var websiteList = (from cp in _context.DbSetConsentCollectionPoints
                                   join w in _context.DbSetConsentWebsite on cp.WebsiteId equals w.WebsiteId
                                   where collectionPointIds.Contains(cp.CollectionPointId)
                                   select
                                       new GeneralConsentWebsiteObject
                                       {
                                           Id = w.WebsiteId,
                                           Description = w.Description,
                                           UrlHomePage = w.Url,
                                           UrlPolicyPage = w.Urlpolicy,
                                       }).ToList();

                model.GeneralConsentWebsiteObject = websiteList.FirstOrDefault();
                #endregion

                #region fetch purpose
                var purposeLists = (from cp in _context.DbSetConsentCollectionPointItem
                                    join p in _context.DbSetConsentPurpose on cp.PurposeId equals p.PurposeId
                                    where cp.Status == Status.Active.ToString()
                                    && p.Status == Status.Active.ToString()
                                    && collectionPointIds.Contains(cp.CollectionPointId)
                                    select
                                        new GeneralConsentPurpose
                                        {
                                            PurposeId = p.PurposeId,
                                            CompanyId = p.CompanyId,
                                            Code = p.Code,
                                            Description = p.Description,
                                            WarningDescription = p.WarningDescription,
                                            PurposeCategoryId = p.PurposeCategoryId,
                                            ExpiredDateTime = Calulate.ExpiredDateTime(p.KeepAliveData, p.CreateDate),
                                            Guid = new Guid(p.Guid),
                                            Version = p.Version,
                                            Priority = cp.Priority,
                                            Status = p.Status
                                        }).ToList();
                model.PurposeList = purposeLists;
                #endregion
            }

            return model;
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
