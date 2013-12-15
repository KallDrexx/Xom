using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xom.Core.Exceptions;
using Xom.Core.InternalModels;
using Xom.Core.Models;

namespace Xom.Core
{
    /// <summary>
    /// Serializes XomDataNodes into xml serialization objects
    /// </summary>
    public class XomDataSerializer
    {
        public object Serialize(XomNodeData data)
        {
            return SerializeNodeData(data, null);
        }

        private object SerializeNodeData(XomNodeData data, DataSerializerParentDetails parentDetails)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (data.NodeType == null)
                throw new ArgumentException("Data object has a null xom node type");

            if (data.NodeType.Type == null)
                throw new ArgumentException("Data object's xom node type has a null Type value");

            var instance = Activator.CreateInstance(data.NodeType.Type);
            SerializeAttributeData(data, instance);

            // Attach the instance to the parent
            if (parentDetails != null)
            {
                var xomChildDetails = parentDetails.ParentXomNode
                                                   .Children
                                                   .First(x => x.AvailableNodes.Any(y => y.Key == parentDetails.ChildNodeName));

                var parentProperty = parentDetails.ParentObject
                                                  .GetType()
                                                  .GetProperties()
                                                  .First(x => x.Name == xomChildDetails.PropertyName);

                parentProperty.SetValue(parentDetails.ParentObject, instance);
            }

            // Serialize children
            if (data.ChildNodes != null)
            {
                foreach (var childDataPair in data.ChildNodes)
                {
                    var details = new DataSerializerParentDetails
                    {
                        ParentObject = instance,
                        ParentXomNode = data.NodeType,
                        ChildNodeName = childDataPair.Key
                    };

                    SerializeNodeData(childDataPair.Value, details);
                }
            }

            return instance;
        }

        private void SerializeAttributeData(XomNodeData data, object target)
        {
            if (data.AttributeData == null)
                return;

            var sourceProperties = data.AttributeData
                                       .GetType()
                                       .GetProperties();

            var targetProperties = target.GetType()
                                         .GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                // If the name of the property doesn't have a matching XomNodeAttribute
                // we don't care about it
                var xomAttribute = data.NodeType
                                       .Attributes
                                       .FirstOrDefault(x => x.Name == sourceProperty.Name);
                if (xomAttribute == null)
                    continue;

                // If the target type doesn't have the property for the attribute, we don't care about it either
                var targetProperty = targetProperties.FirstOrDefault(x => x.Name == xomAttribute.PropertyName);
                if (targetProperty == null)
                    continue;

                var sourcePropertyType = sourceProperty.PropertyType;
                if (sourcePropertyType.IsGenericType && sourcePropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    sourcePropertyType = Nullable.GetUnderlyingType(sourcePropertyType);

                if (targetProperty.PropertyType != sourcePropertyType)
                    throw new XomDataSerializationPropertyTypeMismatchException(sourceProperty.PropertyType, sourceProperty.Name, 
                                                                                targetProperty.PropertyType, targetProperty.Name);

                targetProperty.SetValue(target, sourceProperty.GetValue(data.AttributeData));
            }
        }
    }
}
