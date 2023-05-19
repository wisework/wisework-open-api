using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public  class Consent_ConsentTheme: Base 
{
    public int ThemeId { get; set; }
    public string? ThemeTitle { get; set; }
    public string? HeaderTextColor { get; set; }
    public string? HeaderBackgroundColor { get; set; }
    public string? BodyBackgroudColor { get; set; }
    public string? TopDescriptionTextColor { get; set; }
    public string? BottomDescriptionTextColor { get; set; }
    public string? AcceptionButtonColor { get; set; }
    public string? AcceptionConsentTextColor { get; set; }
    public string? CancelButtonColor { get; set; }
    public string? CancelTextButtonColor { get; set; }
    public string? LinkToPolicyTextColor { get; set; }
    public string? PolicyUrlTextColor { get; set; }
    public string? Status { get; set; }
    public int? CompanyId { get; set; }
}