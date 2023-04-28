using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.CustomField.Commands.CreateCustomField;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Purpose.Commands.CreatePurpose;
public record CreatePurposeCommand : IRequest<PurposeActiveList>
{
    public int PurposeType { get; init; }
    public int CategoryID { get; init; }
    public string Code { get; init; }
    public string Description { get; init; }
    public string KeepAliveData { get; init; }
    public string LinkMoreDetail { get; init; }
    public string Status { get; init; }
    public string TextMoreDetail { get; init; }
    public string WarningDescription { get; init; }

}

public class CreatePurposeCommandHandler : IRequestHandler<CreatePurposeCommand, PurposeActiveList>
{
    private readonly IApplicationDbContext _context;

    public CreatePurposeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurposeActiveList> Handle(CreatePurposeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Consent_Purpose();
        var guid = Guid.NewGuid();
        (string letters, string numbers,DateTime expiredDateTime) = SeparateNumbersAndLetters(request.KeepAliveData);
        entity.Guid = guid.ToString();

        entity.PurposeType = request.PurposeType;
        entity.CategoryID = request.CategoryID;
        entity.Code = request.Code;
        entity.Description = request.Description;
        
        entity.KeepAliveData = request.KeepAliveData;
        entity.LinkMoreDetail = request.LinkMoreDetail; 
        
             
        entity.TextMoreDetail = request.TextMoreDetail; 
        entity.WarningDescription = request.WarningDescription;

        entity.CreateBy = 1;
        entity.UpdateBy = 1;
        entity.CreateDate = DateTime.Now;
        entity.UpdateDate = DateTime.Now;

        entity.Status = Status.Active.ToString();
        entity.Version = 1;
        entity.CompanyId = 1;
        entity.Language = "en";
        entity.ExpiredDateTime = $"{expiredDateTime}";


        



        _context.DbSetConsentPurpose.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        var PurposeInfo = new PurposeActiveList
        {
            GUID = guid,
            PurposeID = entity.PurposeId,
            PurposeType = entity.PurposeType,
            CategoryID = entity.CategoryID,
            Code = entity.Code,
            Description = entity.Description,
            KeepAliveData = entity.KeepAliveData,
            LinkMoreDetail = entity.LinkMoreDetail,
            Status = entity.Status,
            TextMoreDetail = entity.TextMoreDetail,
            WarningDescription = entity.WarningDescription,
            Language = entity.Language,
            ExpiredDateTime = entity.ExpiredDateTime,
        };

        return PurposeInfo;
    }

    public static (string letters, string numbers,DateTime expiredDateTime) SeparateNumbersAndLetters(string input)
    {
        string letters = "";
        string numbers = "";
        DateTime expiredDateTime = DateTime.Now;
        

        foreach (char c in input)
        {
            if (Char.IsLetter(c))
            {
                letters += c;
            }
            else if (Char.IsNumber(c))
            {
                numbers += c;
            }
        }

        

        switch (letters.ToLower()) // Change this value to specify the unit of time to add (days, months, or years)
        {
            case "d":
                expiredDateTime = expiredDateTime.AddDays(Int32.Parse(numbers));
                break;
            case "m":
                expiredDateTime = expiredDateTime.AddMonths(Int32.Parse(numbers));
                break;
            case "y":
                expiredDateTime = expiredDateTime.AddYears(Int32.Parse(numbers));
                break;
            default:
                Console.WriteLine("Invalid time unit specified.");
                break;
        }


        return (letters, numbers, expiredDateTime);
    }
}
