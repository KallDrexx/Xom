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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Exception_Thrown_When_Null_Object_Passed_Into_Seriailzer()
       { 
            var serializer = new XomDataSerializer();
            serializer.Serialize(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Exception_Thrown_When_Data_Object_Has_Null_XomNode()
        {
            var data = new XomNodeData();
            var serializer = new XomDataSerializer();
            serializer.Serialize(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Exception_Thrown_When_Data_Objects_XomNode_Has_Null_Type()
        {
            var data = new XomNodeData { NodeType = new XomNode() };
            var serializer = new XomDataSerializer();
            serializer.Serialize(data);
        }
    }
}
