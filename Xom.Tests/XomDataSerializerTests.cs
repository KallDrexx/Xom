using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xom.Core;
using Xom.Core.Models;
using Xom.Tests.TestObjects.XomNodeData;

namespace Xom.Tests
{
    [TestClass]
    class XomDataSerializerTests
    {
        [TestMethod]
        public void Creates_Correct_XML_Serialization_Object()
        {
            var data = new XomNodeData
            {
                NodeType = new XomNode { Type = typeof(NodeA) }
            };
            var serializer = new XomDataSerializer();

            object result = serializer.Serialize(data);

            Assert.IsNotNull(result, "Resulting object was null");
            Assert.IsInstanceOfType(result, typeof(NodeA), "Resulting object was an incorrect type");
        }
    }
}
