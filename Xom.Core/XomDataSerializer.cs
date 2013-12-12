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
            var result = Activator.CreateInstance(data.NodeType.Type);
            return result;
        }
    }
}
