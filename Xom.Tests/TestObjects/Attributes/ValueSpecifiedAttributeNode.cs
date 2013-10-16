using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xom.Tests.TestObjects.Attributes
{
    class ValueSpecifiedAttributeNode
    {
        public bool TestValueSpecified { get; set; }

        [XmlAttribute]
        public int TestValue { get; set; }
    }
}
