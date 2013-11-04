using System.Xml.Serialization;

namespace Xom.Tests.TestObjects.Attributes
{
    class SimpleAttributeNode
    {
        [XmlIgnore]
        public string NonXmlAttribute { get; set; }

        [XmlAttribute]
        public string TestAttribute { get; set; }
    }
}
