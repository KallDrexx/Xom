using System.Collections.Generic;

namespace Xom.Core.Models
{
    public class NodeChild
    {
        public string PropertyName { get; set; }
        public Dictionary<string, Node> AvailableNodes { get; set; }
    }
}
