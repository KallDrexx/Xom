using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xom.Core.Models;

namespace Xom.Tests.TestObjects.XomNodeData
{
    class NodeB
    {
        public string Attribute1 { get; set; }

        public static readonly XomNode XomNode = new XomNode
        {
            Type = typeof(NodeB),
            Attributes = new XomNodeAttribute[] 
            {
                new XomNodeAttribute { Name = "Attribute1", Type = typeof(string), PropertyName = "Attribute1"}
            }
        };
    }
}
