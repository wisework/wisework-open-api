using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class LocalStringResourceDisplay
{
    public int LocalStringResourceID { get; set; } = 0;
    public string ResourceKey { get; set; } = string.Empty;
    public string ResourceValue { get; set; } = string.Empty;
    public string LanguageCulture { get; set; } = string.Empty;
}
