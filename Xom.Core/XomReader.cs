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
        public IEnumerable<XomNode> GenerateNodes(Type type)
        {
            var nodes = new List<XomNode>();
            CreateNodesForType(type, nodes, true);

            return nodes.ToArray();
        }

        private XomNode CreateNodesForType(Type type, ICollection<XomNode> foundNodes, bool isRoot = false)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var node = foundNodes.FirstOrDefault(x => x.Type == type);
            if (node != null)
                return node;

            node = new XomNode
            {
                Type = GetInnerType(type),
                IsRoot =  isRoot,
                Attributes = type.GetProperties()
                                 .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(XmlAttributeAttribute)))
                                 .Select(x => new XomNodeAttribute
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

            var childNodes = new List<XomNodeChild>();
            foreach (var child in children)
            {
                childNodes.Add(new XomNodeChild
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

        private Dictionary<string, XomNode> CreateAvailableNodesForProperty(PropertyInfo property, ICollection<XomNode> foundNodes)
        {
            bool atLeastOneElementNameFound = false;
            var availableNodes = new Dictionary<string, XomNode>();
            var attributes = property.GetCustomAttributes(false)
                                     .ToArray();

            AddAvailableNodesForXmlArray(attributes, property, foundNodes, availableNodes, ref atLeastOneElementNameFound);
            AddAvailableNodesForXmlElement(attributes, property, foundNodes, availableNodes, ref atLeastOneElementNameFound);

            if (!atLeastOneElementNameFound)
            {
                var childNode = CreateNodesForType(property.PropertyType, foundNodes);
                availableNodes.Add(property.Name, childNode);
            }

            return availableNodes;
        }

        private void AddAvailableNodesForXmlArray(object[] attributes, PropertyInfo property,
                                                    ICollection<XomNode> foundNodes, Dictionary<string, XomNode> availableNodes,
                                                    ref bool atLeastOneElementNameFound)
        {
            var xmlArrayAttribute = attributes.OfType<XmlArrayAttribute>().FirstOrDefault();
            var xmlArrayItemAttributes = attributes.OfType<XmlArrayItemAttribute>().ToArray();

            if (xmlArrayAttribute != null)
            {
                if (!string.IsNullOrWhiteSpace(xmlArrayAttribute.ElementName))
                {
                    var childNode = CreateNodesForType(property.PropertyType, foundNodes);
                    availableNodes.Add(xmlArrayAttribute.ElementName, childNode);
                    atLeastOneElementNameFound = true;
                }
                else if (xmlArrayItemAttributes.Length > 0)
                {
                    foreach (var xmlArrayItemAttribute in xmlArrayItemAttributes)
                    {
                        var childNode = CreateNodesForType(xmlArrayItemAttribute.Type, foundNodes);
                        availableNodes.Add(xmlArrayItemAttribute.ElementName, childNode);
                    }

                    atLeastOneElementNameFound = true;
                }
            }
        }

        private void AddAvailableNodesForXmlElement(IEnumerable<object> attributes, PropertyInfo property,
                                                    ICollection<XomNode> foundNodes, Dictionary<string, XomNode> availableNodes,
                                                    ref bool atLeastOneElementNameFound)
        {
            var xmlElementAttributes = attributes.Where(x => x.GetType() == typeof(XmlElementAttribute))
                                                 .Select(x => (XmlElementAttribute)x)
                                                 .ToArray();

            foreach (var xmlElementAttribute in xmlElementAttributes)
            {
                var type = xmlElementAttribute.Type ?? property.PropertyType;
                var childNode = CreateNodesForType(type, foundNodes);

                if (!string.IsNullOrWhiteSpace(xmlElementAttribute.ElementName))
                    availableNodes.Add(xmlElementAttribute.ElementName, childNode);
                else
                    availableNodes.Add(property.Name, childNode);

                atLeastOneElementNameFound = true;
            }
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
