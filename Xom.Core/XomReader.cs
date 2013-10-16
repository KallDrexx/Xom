using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Xom.Core.Models;

namespace Xom.Core
{
    public class XomReader
    {
        public IEnumerable<Node> GenerateNodes(Type type)
        {
            var node = new Node
            {
                Type = type,
                Attributes = type.GetProperties()
                                 .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(XmlAttributeAttribute)))
                                 .Select(x => new NodeAttribute
                                 {
                                     Name = x.Name,
                                     Type = x.PropertyType
                                 })
                                 .ToArray()
            };

            return new[] {node};
        }
    }
}
