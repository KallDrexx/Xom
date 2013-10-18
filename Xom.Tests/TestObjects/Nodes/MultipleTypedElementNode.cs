using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xom.Tests.TestObjects.Nodes
{
    class MultipleTypedElementNode
    {
        public class ClassA { }
        public class ClassB : ClassA {}
        public class ClassC : ClassA {}

        [XmlElement("ClassB", typeof(ClassB))]
        [XmlElement("ClassC", typeof(ClassC))]
        public ClassA Child { get; set; }
    }
}
