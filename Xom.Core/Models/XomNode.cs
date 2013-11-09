using System;
using System.Collections.Generic;

namespace Xom.Core.Models
{
    public class XomNode
    {
        public Type Type { get; set; }
        public bool IsRoot { get; set; }
        public IEnumerable<XomNodeChild> Children { get; set; }
        public IEnumerable<XomNodeAttribute> Attributes { get; set; }

        public override string ToString()
        {
            string typeName = Type != null
                                  ? Type.Name
                                  : "null";

            return "XomNode: " + typeName;
        }
    }
}
