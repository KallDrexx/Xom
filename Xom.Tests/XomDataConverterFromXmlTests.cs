using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xom.Core;
using Xom.Tests.TestObjects.XomNodeData;
using Telerik.JustMock;

namespace Xom.Tests
{
    [TestClass]
    public class XomDataConverterFromXmlTests
    {
        private IXomReader _xomReader;

        [TestInitialize]
        public void Setup()
        {
            var allXomNodes = new[] { NodeA.XomNode, NodeB.XomNode };
            var onlyNodeBXomNode = new[] { NodeB.XomNode };
            _xomReader = Mock.Create<IXomReader>();
            Mock.Arrange(() => _xomReader.GenerateNodes(typeof(NodeA))).Returns(allXomNodes);
            Mock.Arrange(() => _xomReader.GenerateNodes(typeof(NodeB))).Returns(onlyNodeBXomNode);
        }

        [TestMethod]
        public void Can_Convert_Simple_Xml_Object_To_XomNodeData()
        {
            var converter = new XomDataConverter();
            var xmlObject = new NodeA();

            var nodeData = converter.ConvertToXomNodeData(xmlObject, _xomReader);
            Assert.IsNotNull(nodeData, "Returned node data object was null");
        }

        [TestMethod]
        public void Returns_Null_When_Null_Object_Passed_In()
        {
            var converter = new XomDataConverter();
            var nodeData = converter.ConvertToXomNodeData(null, _xomReader);
            Assert.IsNull(nodeData, "Returned node data wasn't null but should have been");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Exception_Thrown_When_Null_XomReader_Passed_In()
        {
            var inputObject = new object();
            var converter = new XomDataConverter();
            converter.ConvertToXomNodeData(inputObject, null);
        }

        [TestMethod]
        public void Returned_NodeData_Contains_Correct_NodeType()
        {
            var converter = new XomDataConverter();
            var xmlObject = new NodeA();

            var nodeData = converter.ConvertToXomNodeData(xmlObject, _xomReader);
            Assert.AreEqual(NodeA.XomNode, nodeData.NodeType, "Resulting NodeType was incorrect");
        }

        [TestMethod]
        public void Returned_NodeData_Object_Contains_Correct_Attributes()
        {
            const string expectedAttributeName = "Attribute1";
            const string expectedAttributeValue = "Test1";

            var converter = new XomDataConverter();
            var xmlObject = new NodeA
            {
                Attribute1 = expectedAttributeValue
            };

            var nodeData = converter.ConvertToXomNodeData(xmlObject, _xomReader);
            Assert.IsNotNull(nodeData.AttributeData, "Nodes attribute data was null");

            var propertyValue = nodeData.AttributeData
                                        .GetType()
                                        .GetProperties()
                                        .Where(x => x.Name == expectedAttributeName)
                                        .Select(x => x.GetValue(nodeData.AttributeData, null))
                                        .FirstOrDefault();

            Assert.AreEqual(expectedAttributeValue, propertyValue, "Node's Attribute1 value was incorrect");
        }

        [TestMethod]
        public void Returned_NodeData_Object_Contains_Child_Data()
        {
            var converter = new XomDataConverter();
            var xmlObject = new NodeA
            {
                Child1 = new NodeB(),
                CollectionChildren = new List<NodeB>()
                {
                    new NodeB()
                }
            };

            var nodeData = converter.ConvertToXomNodeData(xmlObject, _xomReader);
            Assert.IsNotNull(nodeData.ChildNodes, "Returned data did not have children");
            Assert.AreEqual(2, nodeData.ChildNodes.Length, "Returned data had an incorrect number of children");
            Assert.IsTrue(nodeData.ChildNodes.Any(x => x.Key == "Child1" && x.Value != null), "Returned data did not have a 'Child1' child that wasn't null");
            Assert.IsTrue(nodeData.ChildNodes.Any(x => x.Key == "CollectionChildren" && x.Value != null), "Returned data did not have a 'Child1' child that wasn't null");
        }
    }
}
