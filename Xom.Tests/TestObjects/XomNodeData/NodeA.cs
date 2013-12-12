using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xom.Tests.TestObjects.XomNodeData
{
    class NodeA
    {
        [XmlAttribute]
        public string Attribute1 { get; set; }
    }
}
