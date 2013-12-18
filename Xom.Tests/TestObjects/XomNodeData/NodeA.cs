using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xom.Core.Models;

namespace Xom.Tests.TestObjects.XomNodeData
{
    class NodeA
    {
        private int _attribute3 { get; set; }

        public string Attribute1 { get; set; }
        public string Attr2 { get; set; }
        public NodeB Child1 { get; set; }
        public List<NodeB> CollectionChildren { get; set; }
        public bool Attribute3Set { get; set; }

        public int Attribute3
        {
            get { return _attribute3; }
            set 
            { 
                _attribute3 = value;
                Attribute3Set = true;
            }
        }

        public static readonly XomNode XomNode = new XomNode
        {
            Type = typeof(NodeA),
            IsRoot = true,
            Attributes = new XomNodeAttribute[] 
            {
                new XomNodeAttribute { Name = "Attribute1", Type = typeof(string), PropertyName = "Attribute1"},
                new XomNodeAttribute { Name = "Attribute2", Type = typeof(string), PropertyName = "Attr2"},
                new XomNodeAttribute { Name = "Attribute3", Type = typeof(int), PropertyName = "Attribute3"}
            },
            Children = new XomNodeChild[] 
            {
                new XomNodeChild
                {
                    PropertyName = "Child1",
                    AvailableNodes = new Dictionary<string,XomNode>
                    {
                        {"Child1", NodeB.XomNode},
                        {"C1", NodeB.XomNode}
                    }
                },
                new XomNodeChild
                {
                    PropertyName = "CollectionChildren",
                    IsCollection = true,
                    AvailableNodes = new Dictionary<string,XomNode>
                    {
                        {"CollectionChildren", NodeB.XomNode}
                    }
                }
            }
        };
    }
}
