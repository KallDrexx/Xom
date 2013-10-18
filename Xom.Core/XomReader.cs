using System;
using System.Collections;
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
            var nodes = new List<Node>();
            CreateNodesForType(type, nodes, true);

            return nodes.ToArray();
        }

        private Node CreateNodesForType(Type type, ICollection<Node> foundNodes, bool isRoot = false)
        {
            var node = foundNodes.FirstOrDefault(x => x.Type == type);
            if (node != null)
                return node;

            node = new Node
            {
                Type = GetInnerType(type),
                IsRoot =  isRoot,
                Attributes = type.GetProperties()
                                 .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(XmlAttributeAttribute)))
                                 .Select(x => new NodeAttribute
                                 {
                                     Name = GetAttributeName(x),
                                     Type = x.PropertyType,
                                     IsRequired = AttributeTypeRequired(x, type)
                                 })
                                 .ToArray()
            };

            foundNodes.Add(node);

            var children = type.GetProperties()
                                 .Where(PropertyIsChildElement);

            var childNodes = new List<NodeChild>();
            foreach (var child in children)
            {
                childNodes.Add(new NodeChild
                {
                    PropertyName = child.Name,
                    IsXmlArray = child.CustomAttributes.Any(x => x.AttributeType == typeof(XmlArrayAttribute)),
                    IsCollection = typeof(IEnumerable).IsAssignableFrom(child.PropertyType),
                    AvailableNodes = CreateAvailableNodesForProperty(child, foundNodes)
                });
            }

            node.Children = childNodes;
            return node;
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

        private bool PropertyIsChildElement(PropertyInfo property)
        {
            if (property.CustomAttributes.Any(x => x.AttributeType == typeof(XmlAttributeAttribute)))
                return false;

            if (property.CustomAttributes.Any(x => x.AttributeType == typeof (XmlIgnoreAttribute)))
                return false;

            return true;
        }

        private Type GetInnerType(Type type)
        {
            if (typeof (IEnumerable).IsAssignableFrom(type))
            {
                if (type.IsGenericType)
                    return type.GetGenericArguments()[0];

                if (type.IsArray)
                    return type.GetElementType();
            }

            return type;
        }

        private Dictionary<string, Node> CreateAvailableNodesForProperty(PropertyInfo property, ICollection<Node> foundNodes)
        {
            bool atLeastOneElementNameFound = false;
            var availableNodes = new Dictionary<string, Node>();
            var childNode = CreateNodesForType(property.PropertyType, foundNodes);
            var attributes = property.GetCustomAttributes(false)
                                     .Select(x => new {Type = x.GetType(), Attribute = x})
                                     .ToArray();

            var xmlArrayAttribute = attributes.Where(x => x.Type == typeof(XmlArrayAttribute))
                                              .Select(x => x.Attribute as XmlArrayAttribute)
                                              .FirstOrDefault();

            if (xmlArrayAttribute != null && !string.IsNullOrWhiteSpace(xmlArrayAttribute.ElementName))
            {
                availableNodes.Add(xmlArrayAttribute.ElementName, childNode);
                atLeastOneElementNameFound = true;
            }

            var xmlElementAttributes = attributes.Where(x => x.Type == typeof (XmlElementAttribute))
                                                 .Select(x => x.Attribute as XmlElementAttribute)
                                                 .ToArray();

            foreach (var xmlElementAttribute in xmlElementAttributes)
            {
                if (!string.IsNullOrWhiteSpace(xmlElementAttribute.ElementName))
                    availableNodes.Add(xmlElementAttribute.ElementName, childNode);
                else
                    availableNodes.Add(property.Name, childNode);

                atLeastOneElementNameFound = true;
            }

            if (!atLeastOneElementNameFound)
                availableNodes.Add(property.Name, childNode);

            return availableNodes;
        }

        private string GetAttributeName(PropertyInfo property)
        {
            var attributeDetails = property.GetCustomAttributes(typeof (XmlAttributeAttribute), false)
                                           .Cast<XmlAttributeAttribute>()
                                           .First();

            return !string.IsNullOrWhiteSpace(attributeDetails.AttributeName)
                       ? attributeDetails.AttributeName
                       : property.Name;
        }
    }
}
