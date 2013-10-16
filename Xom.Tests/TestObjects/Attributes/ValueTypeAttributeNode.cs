using System.Xml.Serialization;

namespace Xom.Tests.TestObjects.Attributes
{
    class ValueTypeAttributeNode
    {
        [XmlAttribute]
        public int Attribute { get; set; }
    }
}
