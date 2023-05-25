using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class ResultDisplay
{
    public int RS { get; set; } = 0;
    public string MC { get; set; } = string.Empty;
    public string MS { get; set; } = string.Empty;
    public int ID { get; set; } = 0;
    public int VS { get; set; } = 0;
    public string GUID { get; set; } = string.Empty;
    public string CaseNumber { get; set; } = string.Empty;
    public string ConsentCookieScript { get; set; } = string.Empty;

}
