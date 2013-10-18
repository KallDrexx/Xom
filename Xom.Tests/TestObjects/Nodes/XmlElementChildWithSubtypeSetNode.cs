using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xom.Tests.TestObjects.Nodes
{
    class XmlElementChildWithSubtypeSetNode
    {
        public class ClassA {}
        public class ClassB : ClassA {}

        [XmlElement("Child", typeof(ClassB))]
        public ClassA Child { get; set; }
    }
}
