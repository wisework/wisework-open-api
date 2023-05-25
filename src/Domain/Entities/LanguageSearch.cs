using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class LanguageSearch
{
    public int LanguageID { get; set; } = 0;
    public string LanguageCulture { get; set; } = string.Empty;
}
