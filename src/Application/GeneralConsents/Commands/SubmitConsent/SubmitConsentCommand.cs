using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Domain.Entities;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.Design;
using System.Diagnostics.Tracing;
using System.Security.Cryptography;
using WW.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WW.Application.Common.Models;


namespace WW.Application.GeneralConsents.Commands;
public record SubmitConsentCommand : IRequest<SubmitGeneralConsentResponse>
{
    public string? AgeRangeCode { get; init; }
    public string CollectionPointGuid { get; init; }
    public int CompanyId { get; init; }
    public int WebSiteId { get; init; }
    public int? Uid { get; init; }
    public string? FullName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? IdCardNumber { get; init; }
    public List<SubmitConsentPurpose> Purpose { get; set; }
    public List<SubmitCollectionPointCustomField>? CollectionPointCustomField { get; set; }
    public AuthenticationModel? authentication { get; set; }

}
public class SubmitConsentCommandHandler : IRequestHandler<SubmitConsentCommand, SubmitGeneralConsentResponse>
{
    private readonly IApplicationDbContext _context;
  
    public SubmitConsentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
      
    }

    public async Task<SubmitGeneralConsentResponse> Handle(SubmitConsentCommand request, CancellationToken cancellationToken)
    {

        var consentID = 2359;
        var newCon = new Consent_Consent();
        var CollectionPointID = 0;
        var purposeEntity = new SubmitConsentPurpose();
        var customfieldEntity = new SubmitCollectionPointCustomField();

        var collectionPoint = _context.DbSetConsentCollectionPoints.Where
                (c => c.Guid == request.CollectionPointGuid 
                && c.CompanyId == request.CompanyId && c.Status == "Active").FirstOrDefault();

        #region fetch purpose
        foreach (var purpose in request.Purpose)
        {
            purposeEntity.PurposeGuid = purpose.PurposeGuid;
            purposeEntity.Active = purpose.Active;

        }
        #endregion
        #region fetch Customfield
        foreach (var customfield in request.CollectionPointCustomField)
        {
            customfieldEntity.CollectionPointCustomFieldConfigId = customfield.CollectionPointCustomFieldConfigId;
            customfieldEntity.Value = customfield.Value;

        }
        #endregion
        if (collectionPoint != null)
            {
                CollectionPointID = collectionPoint.CollectionPointId;
                bool? ActiveConsentCardNumberPK = collectionPoint.ActiveConsentCardNumberPk;
                bool? ActiveConsentNamePK = collectionPoint.ActiveConsentNamePk;
                bool? ActiveConsentEmailPK = collectionPoint.ActiveConsentEmailPk;
                bool? ActiveConsentTelPK = collectionPoint.ActiveConsentTelPk;
                bool? ActiveConsentUIDPK = collectionPoint.ActiveConsentUidpk;

                // Build the dynamic query for CountTransactions
                var countTransactionsQuery = _context.DbSetConsent
                    .Where(c => c.CompanyId == request.CompanyId && c.CollectionPointId == CollectionPointID);

                if ((bool)ActiveConsentCardNumberPK)
                    countTransactionsQuery = countTransactionsQuery.Where(c => c.IdCardNumber == request.IdCardNumber);

                if ((bool)ActiveConsentNamePK)
                    countTransactionsQuery = countTransactionsQuery.Where(c => c.FullName == request.FullName);

                if ((bool)ActiveConsentEmailPK)
                    countTransactionsQuery = countTransactionsQuery.Where(c => c.Email == request.Email);

                if ((bool)ActiveConsentTelPK)
                    countTransactionsQuery = countTransactionsQuery.Where(c => c.PhoneNumber == request.PhoneNumber);

                if (ActiveConsentUIDPK ?? true)
                    countTransactionsQuery = countTransactionsQuery.Where(c => c.Uid == request.Uid);

                int CountTransactions = countTransactionsQuery.Count();

                if (CountTransactions > 0)
                {
                    // Update existing consent records
                    var updateQuery = _context.DbSetConsent
                        .Where(c => c.CompanyId == request.CompanyId && c.CollectionPointId == CollectionPointID);

                    if ((bool)ActiveConsentCardNumberPK)
                        updateQuery = updateQuery.Where(c => c.IdCardNumber == request.IdCardNumber);

                    if ((bool)ActiveConsentNamePK)
                        updateQuery = updateQuery.Where(c => c.FullName == request.FullName);

                    if ((bool)ActiveConsentEmailPK)
                        updateQuery = updateQuery.Where(c => c.Email == request.Email);

                    if ((bool)ActiveConsentTelPK)
                        updateQuery = updateQuery.Where(c => c.PhoneNumber == request.PhoneNumber);

                    if (ActiveConsentUIDPK ?? true)
                        updateQuery = updateQuery.Where(c => c.Uid == request.Uid);

                    //updateQuery.Update(c => new Consent_Consent { New = false });

                // Update TotalRow
                var totalRow = _context.DbSetTotalRow.FirstOrDefault(tr => tr.CompanyId == request.CompanyId && tr.TableName == "consent_consent");
                    if (totalRow != null)
                    {
                        totalRow.TotalCountRow++;
                }
            }

                else if (CountTransactions == 0)
                {
                    // Update TotalRow
                    var totalRow = _context.DbSetTotalRow.Where(tr => tr.CompanyId == request.CompanyId 
                    && tr.TableName == "consent_consent").FirstOrDefault();
                    if (totalRow != null)
                    {                    
                        totalRow.TotalCountRow++;
                        totalRow.TotalCountGroup++;

                }
            }
           

            // Insert new consent record
            newCon = new Consent_Consent
            {
                
                CompanyId = request.CompanyId,
                CollectionPointId = CollectionPointID,
                ConsentDatetime = DateTimeOffset.Now,
                WebsiteId = request.WebSiteId,
                Email = request.Email,
                FullName = request.FullName,
                FromBrowser = "",
                FromWebsite = "",
                ConsentSignature = "",
                VerifyType = "",
                TotalTransactions = CountTransactions + 1,
                New = 1,
                Remark = "",
                EventCode = "",
                Expired = DateTimeOffset.Now,
                HasNotificationRenew = 0,
                Uid = request.Uid,
                AgeRange = request.AgeRangeCode,
                Status = Status.Active.ToString(),
                PhoneNumber = request.PhoneNumber,
                IdCardNumber = request.IdCardNumber,

                CreateBy = request.authentication.UserID,
                CreateDate = DateTimeOffset.Now,
                UpdateBy = request.authentication.UserID,
                UpdateDate = DateTimeOffset.Now,
                };

                _context.DbSetConsent.Add(newCon);

            await _context.SaveChangesAsync(cancellationToken);


        }
        var consentIDTemp = newCon.ConsentId;

        consentID = consentIDTemp;

        Console.WriteLine(consentID); // Output the generated ConsentID value

        var collectionPointItemID = _context.DbSetConsentCollectionPointItem
               .Join(_context.DbSetConsentPurpose,
               cpi => cpi.PurposeId,
               cp => cp.PurposeId,
               (cpi, cp) => new { CollectionPointItem = cpi, Purpose = cp })
               .Where(c => c.CollectionPointItem.CollectionPointId == CollectionPointID && c.Purpose.Guid == purposeEntity.PurposeGuid.ToString())
               .Select(c => c.CollectionPointItem.CollectionPointItemId).FirstOrDefault();
               

        Consent_ConsentItem consentItem = new Consent_ConsentItem
        {
            CompanyId = request.CompanyId,
            ConsentId = consentID,
            CollectionPointItemId = collectionPointItemID,
            ConsentActive = 1,
            Expired = newCon.Expired,
        };

        _context.DbSetConsentItem.Add(consentItem);
        await _context.SaveChangesAsync(cancellationToken);

        int consentItemID = consentItem.ConsentItemId;

        Consent_ConsentCustomField consentCustomField = new Consent_ConsentCustomField
        {
            CollectionPointCustomFieldConfigID = customfieldEntity.CollectionPointCustomFieldConfigId, 
            Value = customfieldEntity.Value,
            ConsentId = consentID
        };

        _context.DbSetConsentCustomField.Add(consentCustomField);
        await _context.SaveChangesAsync(cancellationToken);

        var websiteList = (from cc in _context.DbSetConsent
                           join w in _context.DbSetConsentWebsite on cc.WebsiteId equals w.WebsiteId
                           where cc.ConsentId == consentID
                           select
                               new SubmitGeneralConsentWebsiteObject
                               {
                                   Id = w.WebsiteId,
                                   Description = w.Description,
                                   UrlHomePage = w.Url,
                                   UrlPolicyPage = w.Urlpolicy,
                               }).FirstOrDefault();

        var result = (from cc in _context.DbSetConsent
                      join cw in _context.DbSetConsentWebsite on cc.WebsiteId equals cw.WebsiteId
                      join cct in _context.DbSetConsentItem on cc.ConsentId equals cct.ConsentId
                      join ccp in _context.DbSetConsentCollectionPoints on CollectionPointID equals ccp.CollectionPointId
                      join cp in _context.DbSetConsentPurpose on purposeEntity.PurposeGuid.ToString() equals cp.Guid
                      where cc.ConsentId == consentID && cc.Status == "Active" && cc.CompanyId == request.CompanyId
                      select new SubmitGeneralConsentResponse
                      {
                          CollectionPointName = ccp.CollectionPoint,
                          SubmitGeneralConsentWebsiteObject = new SubmitGeneralConsentWebsiteObject { Id = cw.WebsiteId, Description = cw.Description, UrlHomePage = cw.Url, UrlPolicyPage = cw.Urlpolicy },
                          PurposeGuid = new Guid(cp.Guid),
                          PurposeCode = cp.Code,
                          PurposeDescript = cp.Description,
                          Email = cc.Email,
                          ConsentActive = cct.ConsentActive.ToString(),
                      }).FirstOrDefault();
      

        return result;
    }
}

