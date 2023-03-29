using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Application.GeneralConsents.Queries;
public class CollectionPointCustomField
{
       public int CollectionPointCustomFieldID { get; set; } = 0;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
        public int CollectionPointCustomFieldConfigID { get; set; } = 0;
        public string CollectionPointGuid { get; set; } = string.Empty;
        public int Sequence { get; set; } = 0;
        public bool Required { get; set; } = false;
        public string AgeRange { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool Have { get; set; } = false;
}
