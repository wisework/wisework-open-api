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
        
        
        //var result = _context.SubmitConsent(@"[SP_CONSENT_SIGN] 
        //                 @CompanyID
        //                ,@CollectionPointGUID
        //                ,@WebsiteID
        //                ,@NameSurname
        //                ,@Email
        //                ,@Tel
        //                ,@FromBrowser
        //                ,@FromWebsite
        //                ,@VerifyType
        //                ,@ConsentSignature
        //                ,@CardNumber
        //                ,@CreateBy
        //                ,@Expired
        //                ,@EventCode
        //                ,@UID
        //                ,@AgeRange
        //                ,@OutputID OUTPUT"
        //                /*,request.CompanyId
        //                ,request.CollectionPointGuid
        //                ,request.WebSiteId
        //                ,request.FullName
        //                ,request.Email
        //                ,request.PhoneNumber
        //                ,request.FromBrowser
        //                ,request.FromWebsite
        //                ,request.VerifyType
        //                ,request.ConsentSignature
        //                ,request.IdCardNumber
        //                ,1 // create by
        //                ,request.ExpiredDateTime
        //                ,request.EventCode
        //                ,request.Uid
        //                ,request.AgeRangeCode*/
        //                ,request);


        //var re = new SubmitGeneralConsentResponse();

            // Retrieve CollectionPointID and ActiveConsent settings

            var collectionPoint = _context.DbSetConsentCollectionPoints.Where
                (c => c.Guid == request.CollectionPointGuid 
                && c.CompanyId == request.CompanyId && c.Status == "Active").FirstOrDefault();

            if (collectionPoint != null)
            {
                int CollectionPointID = collectionPoint.CollectionPointId;
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

                    if ((bool)ActiveConsentUIDPK)
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
            var newConsent = new Consent_Consent
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

                CreateBy = 1,
                CreateDate = DateTimeOffset.Now,
                UpdateBy = 1,
                UpdateDate = DateTimeOffset.Now,
                };

                _context.DbSetConsent.Add(newConsent);
            await _context.SaveChangesAsync(cancellationToken);


            var consentID = newConsent.ConsentId;
                Console.WriteLine(consentID); // Output the generated ConsentID value
            }

        var responseResult = new SubmitGeneralConsentResponse();
        responseResult.CollectionPointName = "";
        responseResult.SubmitGeneralConsentWebsiteObject = new SubmitGeneralConsentWebsiteObject();
        responseResult.PurposeGuid = new Guid("");
        responseResult.PurposeCode = "";
        responseResult.PurposeDescript = "";
        responseResult.Email = "";
        responseResult.ConsentActive =  "";

        return responseResult;
    }
}

