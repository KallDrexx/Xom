using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xom.Tests.TestObjects
{
    class SimpleAttributeNode
    {
        public string NonXmlAttribute { get; set; }

        [XmlAttribute]
        public string TestAttribute { get; set; }
    }
}
