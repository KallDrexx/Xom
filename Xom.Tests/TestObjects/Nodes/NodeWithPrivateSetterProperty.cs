using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xom.Tests.TestObjects.Nodes
{
    public class NodeWithPrivateSetterProperty
    {
        public string Child { get; private set; }
    }
}
