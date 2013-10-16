using System.Xml.Serialization;

namespace Xom.Tests.TestObjects.Attributes
{
    class MultipleAttributeNode
    {
        [XmlAttribute]
        public string Attribute1 { get; set; }

        [XmlAttribute]
        public string Attribute2 { get; set; }
    }
}
