using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                                     Type = x.PropertyType,
                                     IsRequired = AttributeTypeRequired(x, type)
                                 })
                                 .ToArray()
            };

            return new[] {node};
        }

        private bool AttributeTypeRequired(PropertyInfo attributeProperty, Type containingType)
        {
            // Non-value types are never required
            if (!attributeProperty.PropertyType.IsValueType)
                return false;

            // XML Serializer systems have a convention that if the property's name
            // ends in "Value" and there is a "*ValueSpecified" bool property available
            // then that means the value type property is not required.  So check for that.
            const string valueString = "Value";
            const string valueSpecifiedString = "ValueSpecified";
            if (attributeProperty.Name.EndsWith(valueString))
            {
                var valueIndex = attributeProperty.Name.IndexOf(valueString, StringComparison.Ordinal);
                var prefix = attributeProperty.Name.Remove(valueIndex);
                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    var specifiedName = prefix + valueSpecifiedString;
                    var specifiedProperty = containingType.GetProperties()
                                                          .Where(x => x.Name == specifiedName)
                                                          .FirstOrDefault(x => x.PropertyType == typeof (bool));

                    if (specifiedProperty != null)
                        return false;
                }
            }


            return true;
        }
    }
}
