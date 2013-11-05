using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xom.Tests.TestObjects.Nodes
{
    class MultipleTypedArrayItemNode
    {
        public class ClassA { }
        public class ClassB : ClassA {}
        public class ClassC : ClassA {}

        [XmlArray]
        [XmlArrayItem("ClassB", typeof(ClassB))]
        [XmlArrayItem("ClassC", typeof(ClassC))]
        public ClassA Child { get; set; }
    }
}
