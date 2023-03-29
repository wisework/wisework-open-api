using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Application.Common.Models;
public class PrimaryKeyConfiguration
{
    public bool ActiveConsentCardNumber { get; set; } = false;
    public bool ActiveConsentCardNumberPK { get; set; } = false;
    public bool ActiveConsentCardNumberRequired { get; set; } = false;
    public bool ActiveConsentName { get; set; } = false;
    public bool ActiveConsentNamePK { get; set; } = false;
    public bool ActiveConsentNameRequired { get; set; } = false;
    public bool ActiveConsentEmail { get; set; } = false;
    public bool ActiveConsentEmailPK { get; set; } = false;
    public bool ActiveConsentEmailRequired { get; set; } = false;
    public bool ActiveConsentTel { get; set; } = false;
    public bool ActiveConsentTelPK { get; set; } = false;
    public bool ActiveConsentTelRequired { get; set; } = false;
    public bool ActiveConsentUID { get; set; } = false;
    public bool ActiveConsentUIDPK { get; set; } = false;
    public bool ActiveConsentUIDRequired { get; set; } = false;
}
