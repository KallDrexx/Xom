using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xom.Core;
using Xom.Core.Models;
using Xom.Tests.TestObjects.XomNodeData;
using Xom.Core.Exceptions;

namespace Xom.Tests
{
    [TestClass]
    public class XomDataSerializerTests
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

        [TestMethod]
        public void Serializes_Simple_Attribute_By_Direct_Name()
        {
            var serializer = new XomDataSerializer();
            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                AttributeData = new { Attribute1 = "Test" }
            };

            var result = (NodeA)serializer.Serialize(data);
            Assert.AreEqual("Test", result.Attribute1, "Attribute1's value was incorrect");
        }

        [TestMethod]
        public void Serializes_Attribute_By_Xml_Name()
        {
            var serializer = new XomDataSerializer();
            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                AttributeData = new { Attribute2 = "Test" }
            };

            var result = (NodeA)serializer.Serialize(data);
            Assert.AreEqual("Test", result.Attr2, "Attr2's value was incorrect");
        }

        [TestMethod]
        [ExpectedException(typeof(XomDataSerializationPropertyTypeMismatchException))]
        public void Exception_Thrown_When_Attribute_Types_Dont_Match()
        {
            var serializer = new XomDataSerializer();
            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                AttributeData = new { Attribute2 = 1 }
            };

            var result = (NodeA)serializer.Serialize(data);
        }

        [TestMethod]
        public void Can_Create_Child_Node_With_Correct_Type_From_Data()
        {
            var serializer = new XomDataSerializer();
            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                ChildNodes = new KeyValuePair<string, XomNodeData>[]
                {
                    new KeyValuePair<string, XomNodeData>("Child1", new XomNodeData { NodeType = NodeB.XomNode })
                }
            };

            var result = (NodeA)serializer.Serialize(data);
            Assert.IsNotNull(result.Child1, "Child1 was incorrectly null");
        }

        [TestMethod]
        public void Can_Create_Child_Node_With_Name_Not_Matching_Property_Name()
        {
            var serializer = new XomDataSerializer();
            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                ChildNodes = new KeyValuePair<string, XomNodeData>[]
                {
                    new KeyValuePair<string, XomNodeData>("C1", new XomNodeData { NodeType = NodeB.XomNode })
                }
            };

            var result = (NodeA)serializer.Serialize(data);
            Assert.IsNotNull(result.Child1, "Child1 was incorrectly null");
        }
    }
}
