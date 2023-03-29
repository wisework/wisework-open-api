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

namespace WW.Application.GeneralConsents.Commands;
public record SubmitConsentCommand : IRequest<int>
{
    public string AgeRangeCode { get; init; }
    public string CollectionPointGuid { get; init; }
    public int CompanyId { get; init; }
    public int WebSiteId { get; init; }
    public int Uid { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public string FromBrowser { get; init; }
    public string FromWebsite { get; init; }
    public string VerifyType { get; init; }
    public string EventCode { get; init; }
    public string IdCardNumber { get; init; }
    public DateTimeOffset ExpiredDateTime { get; init; }
    public string ConsentSignature { get; init; }
    public List<SubmitConsentPurpose> Purpose { get; set; }
    public List<SubmitCollectionPointCustomField> CollectionPointCustomField { get; set; }

}
public class SubmitConsentCommandHandler : IRequestHandler<SubmitConsentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public SubmitConsentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<int> Handle(SubmitConsentCommand request, CancellationToken cancellationToken)
    {
        
        
        var result = _context.SubmitConsent(@"[SP_CONSENT_SIGN] 
                         @CompanyID
                        ,@CollectionPointGUID
                        ,@WebsiteID
                        ,@NameSurname
                        ,@Email
                        ,@Tel
                        ,@FromBrowser
                        ,@FromWebsite
                        ,@VerifyType
                        ,@ConsentSignature
                        ,@CardNumber
                        ,@CreateBy
                        ,@Expired
                        ,@EventCode
                        ,@UID
                        ,@AgeRange
                        ,@OutputID OUTPUT"
                        /*,request.CompanyId
                        ,request.CollectionPointGuid
                        ,request.WebSiteId
                        ,request.FullName
                        ,request.Email
                        ,request.PhoneNumber
                        ,request.FromBrowser
                        ,request.FromWebsite
                        ,request.VerifyType
                        ,request.ConsentSignature
                        ,request.IdCardNumber
                        ,1 // create by
                        ,request.ExpiredDateTime
                        ,request.EventCode
                        ,request.Uid
                        ,request.AgeRangeCode*/
                        ,request);



       
        return result;
    }
}

