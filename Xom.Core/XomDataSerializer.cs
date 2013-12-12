using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (data == null)
                throw new ArgumentNullException("data");

            if (data.NodeType == null)
                throw new ArgumentException("Data object has a null xom node type");

            if (data.NodeType.Type == null)
                throw new ArgumentException("Data object's xom node type has a null Type value");

            var result = Activator.CreateInstance(data.NodeType.Type);
            return result;
        }
    }
}
