using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xom.Tests.TestObjects.Nodes
{
    class XmlArrayItemChildWithSubtypeSetNode
    {
        public class ClassA {}
        public class ClassB : ClassA {}

        [XmlArray]
        [XmlArrayItem("Child", typeof(ClassB))]
        public ClassA Child { get; set; }
    }
}
