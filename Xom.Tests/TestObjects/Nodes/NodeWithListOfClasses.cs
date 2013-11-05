using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xom.Tests.TestObjects.Nodes
{
    class NodeWithListOfClasses
    {
        [XmlArray]
        public List<ChildClass> Children { get; set; }

        public class ChildClass
        {
            public string InnerChild { get; set; }
        }
    }
}
