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

namespace WW.Application.ConsentPageSetting.Commands.CreateConsentTheme;

public record CreateConsentThemeCommand : IRequest<ConsentTheme>
{
    public string ThemeTitle { get; init; }
    public string HeaderTextColor { get; init; }
    public string HeaderBackgroundColor { get; init; }
    public string BodyBackgroudColor { get; init; }
    public string TopDescriptionTextColor { get; init; }
    public string BottomDescriptionTextColor { get; init; }
    public string AcceptionButtonColor { get; init; }
    public string AcceptionConsentTextColor { get; init; }
    public string CancelButtonColor { get; init; }
    public string CancelTextButtonColor { get; init; }
    public string LinkToPolicyTextColor { get; init; }
    public string PolicyUrlTextColor { get; init; }
}

public class CreateConsentThemeCommandHandler : IRequestHandler<CreateConsentThemeCommand, ConsentTheme>
{
    private readonly IApplicationDbContext _context;

    public CreateConsentThemeCommandHandler(IApplicationDbContext context, iUplad contetx)
    {
        _context = context;
    }

    public async Task<ConsentTheme> Handle(CreateConsentThemeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Consent_ConsentTheme();
        
        entity.ThemeTitle = request.ThemeTitle;
        entity.HeaderTextColor = request.HeaderTextColor;
        entity.HeaderBackgroundColor = request.HeaderBackgroundColor;
        entity.BodyBackgroudColor = request.BodyBackgroudColor;
        entity.TopDescriptionTextColor = request.TopDescriptionTextColor;
        entity.BottomDescriptionTextColor = request.BottomDescriptionTextColor;
        entity.AcceptionButtonColor = request.AcceptionButtonColor;
        entity.AcceptionConsentTextColor = request.AcceptionConsentTextColor;
        entity.CancelButtonColor = request.CancelButtonColor;
        entity.CancelTextButtonColor = request.CancelTextButtonColor;
        entity.LinkToPolicyTextColor = request.LinkToPolicyTextColor;
        entity.PolicyUrlTextColor = request.PolicyUrlTextColor;

        entity.CreateBy = 1;
        entity.UpdateBy = 1;
        entity.CreateDate = DateTime.Now;
        entity.UpdateDate = DateTime.Now;

        entity.Status = Status.Active.ToString();
        entity.Version = 1;
        entity.CompanyId = 1;

        _context.DbSetConsentTheme.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        var consentThemeInfo = new ConsentTheme{
            ThemeId = entity.ThemeId,
            ThemeTitle = entity.ThemeTitle,
            HerderTextColor = entity.HeaderTextColor,
            HeaderBackgroundColor = entity.HeaderBackgroundColor,
            BodyBackgroudColor = entity.BodyBackgroudColor,
            TopDescriptionTextColor = entity.TopDescriptionTextColor,
            BottomDescriptionTextColor = entity.BottomDescriptionTextColor,
            AcceptionButtonColor = entity.AcceptionButtonColor,
            AcceptionConsentTextColor = entity.AcceptionConsentTextColor,
            CancelButtonColor = entity.CancelButtonColor,
            CancelTextButtonColor = entity.CancelTextButtonColor,
            LinkToPolicyTextColor = entity.LinkToPolicyTextColor,
            PolicyUrlTextColor = entity.PolicyUrlTextColor,
            Status = entity.Status,
            CompanyId = entity.CompanyId,
        };

        return consentThemeInfo;
    }
}
