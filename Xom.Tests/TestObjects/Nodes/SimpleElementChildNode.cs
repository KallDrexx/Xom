using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xom.Tests.TestObjects.Nodes
{
    class SimpleElementChildNode
    {
        [XmlElement]
        public ChildNode Child { get; set; }

        public class ChildNode
        {
            
        }
    }
}
