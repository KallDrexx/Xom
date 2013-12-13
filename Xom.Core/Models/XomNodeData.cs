using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xom.Core.Models
{
    public class XomNodeData
    {
        public XomNode NodeType { get; set; }
        public KeyValuePair<string, XomNodeData>[] ChildNodes { get; set; }
        public object AttributeData { get; set; }
    }
}
