using System.Collections.Generic;

namespace Xom.Core.Models
{
    public class NodeChild
    {
        public string PropertyName { get; set; }
        public KeyValuePair<string, Node> PossibleNodes { get; set; }
    }
}
