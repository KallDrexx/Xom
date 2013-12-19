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
    public class XomDataConverterToXmlObjectTests
    {
        [TestMethod]
        public void Creates_Correct_XML_Serialization_Object()
        {
            var data = new XomNodeData
            {
                NodeType = new XomNode { Type = typeof(NodeA) }
            };
            var serializer = new XomDataConverter();

            object result = serializer.ConvertToXmlObject(data);

            Assert.IsNotNull(result, "Resulting object was null");
            Assert.IsInstanceOfType(result, typeof(NodeA), "Resulting object was an incorrect type");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Exception_Thrown_When_Null_Object_Passed_Into_Seriailzer()
        {
            var serializer = new XomDataConverter();
            serializer.ConvertToXmlObject(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Exception_Thrown_When_Data_Object_Has_Null_XomNode()
        {
            var data = new XomNodeData();
            var serializer = new XomDataConverter();
            serializer.ConvertToXmlObject(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Exception_Thrown_When_Data_Objects_XomNode_Has_Null_Type()
        {
            var data = new XomNodeData { NodeType = new XomNode() };
            var serializer = new XomDataConverter();
            serializer.ConvertToXmlObject(data);
        }

        [TestMethod]
        public void Converts_Simple_Attribute_By_Direct_Name()
        {
            var serializer = new XomDataConverter();
            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                AttributeData = new { Attribute1 = "Test" }
            };

            var result = (NodeA)serializer.ConvertToXmlObject(data);
            Assert.AreEqual("Test", result.Attribute1, "Attribute1's value was incorrect");
        }

        [TestMethod]
        public void Converts_Attribute_By_Xml_Name()
        {
            var serializer = new XomDataConverter();
            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                AttributeData = new { Attribute2 = "Test" }
            };

            var result = (NodeA)serializer.ConvertToXmlObject(data);
            Assert.AreEqual("Test", result.Attr2, "Attr2's value was incorrect");
        }

        [TestMethod]
        [ExpectedException(typeof(XomDataSerializationPropertyTypeMismatchException))]
        public void Exception_Thrown_When_Attribute_Types_Dont_Match()
        {
            var serializer = new XomDataConverter();
            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                AttributeData = new { Attribute2 = 1 }
            };

            var result = (NodeA)serializer.ConvertToXmlObject(data);
        }

        [TestMethod]
        public void Can_Create_Child_Node_With_Correct_Type_From_Data()
        {
            var serializer = new XomDataConverter();
            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                ChildNodes = new KeyValuePair<string, XomNodeData>[]
                {
                    new KeyValuePair<string, XomNodeData>("Child1", new XomNodeData { NodeType = NodeB.XomNode })
                }
            };

            var result = (NodeA)serializer.ConvertToXmlObject(data);
            Assert.IsNotNull(result.Child1, "Child1 was incorrectly null");
        }

        [TestMethod]
        public void Can_Create_Child_Node_With_Name_Not_Matching_Property_Name()
        {
            var serializer = new XomDataConverter();
            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                ChildNodes = new KeyValuePair<string, XomNodeData>[]
                {
                    new KeyValuePair<string, XomNodeData>("C1", new XomNodeData { NodeType = NodeB.XomNode })
                }
            };

            var result = (NodeA)serializer.ConvertToXmlObject(data);
            Assert.IsNotNull(result.Child1, "Child1 was incorrectly null");
        }

        [TestMethod]
        public void Can_Set_Nullable_Attribute_Source_To_Non_Nullable_Target()
        {
            const int testValue = 5;
            var serializer = new XomDataConverter();
            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                AttributeData = new { Attribute3 = (int?)testValue }
            };

            var result = (NodeA)serializer.ConvertToXmlObject(data);
            Assert.AreEqual(testValue, result.Attribute3, "Attribute3 had an incorrect value");
        }

        [TestMethod]
        public void Nullable_Attribute_Source_That_Has_Null_Value_Is_Ignored()
        {
            var serializer = new XomDataConverter();
            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                AttributeData = new { Attribute3 = (int?)null }
            };

            var result = (NodeA)serializer.ConvertToXmlObject(data);
            Assert.AreEqual(0, result.Attribute3, "Attribute3 had an incorrect value");
            Assert.IsFalse(result.Attribute3Set, "Attribute3's value was set when it shouldn't have been");
        }

        [TestMethod]
        public void Can_Serialize_IEnumerable_Data_Nodes_Into_Xml_Object_Collection()
        {
            var serializer = new XomDataConverter();
            var innerData1 = new XomNodeData
            {
                NodeType = NodeB.XomNode
            };

            var data = new XomNodeData
            {
                NodeType = NodeA.XomNode,
                ChildNodes = new KeyValuePair<string, XomNodeData>[]
                {
                    new KeyValuePair<string, XomNodeData>("CollectionChildren", innerData1),
                    new KeyValuePair<string, XomNodeData>("CollectionChildren", innerData1)
                }
            };

            var result = (NodeA)serializer.ConvertToXmlObject(data);
            Assert.AreEqual(2, result.CollectionChildren.Count, "Incorrect number of elements in the collection children node");
        }
    }
}
