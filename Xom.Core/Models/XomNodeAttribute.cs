using System;

namespace Xom.Core.Models
{
    public class XomNodeAttribute
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsRequired { get; set; }
    }
}
