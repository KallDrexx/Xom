using System.Collections.Generic;

namespace Xom.Core.Models
{
    public class XomNodeChild
    {
        public string PropertyName { get; set; }
        public bool IsXmlArray { get; set; }
        public bool IsCollection { get; set; }
        public Dictionary<string, XomNode> AvailableNodes { get; set; }
    }
}
