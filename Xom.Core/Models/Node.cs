using System;
using System.Collections.Generic;

namespace Xom.Core.Models
{
    public class Node
    {
        public Type Type { get; set; }
        public bool IsRoot { get; set; }
        public IEnumerable<NodeChild> Children { get; set; }
        public IEnumerable<NodeAttribute> Attributes { get; set; } 
    }
}
