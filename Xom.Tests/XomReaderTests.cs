using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xom.Core;
using Xom.Tests.TestObjects;

namespace Xom.Tests
{
    [TestClass]
    public class XomReaderTests
    {
        [TestMethod]
        public void Can_Map_Single_Node_Type()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof (SimpleAttributeNode));

            Assert.IsNotNull(nodes, "Null node enumerable returned");
            Assert.AreEqual(1, nodes.Count(), "Incorrect number of nodes returned");
            Assert.AreEqual(typeof(SimpleAttributeNode), nodes.First().Type, "Node had an incorrect type");
        }

        [TestMethod]
        public void Generated_Node_Has_The_Correct_Number_Of_Attributes()
        {
            var xom = new XomReader();
            var node = xom.GenerateNodes(typeof (MultipleAttributeNode))
                           .First();

            Assert.IsNotNull(node.Attributes, "Node attributes were null");
            Assert.AreEqual(2, node.Attributes.Count(), "Incorrect number of attributes on node");
        }

        [TestMethod]
        public void Generated_Node_Only_Has_Annotated_Xml_Attributes()
        {
            var xom = new XomReader();
            var node = xom.GenerateNodes(typeof (SimpleAttributeNode))
                          .First();

            Assert.AreEqual(1, node.Attributes.Count(), "Incorrect number of attributes on node");
        }

        [TestMethod]
        public void Generated_Node_Attribute_Has_Same_Name_As_Property()
        {
            var xom = new XomReader();
            var node = xom.GenerateNodes(typeof (SimpleAttributeNode));
            var attribute = node.First().Attributes.First();

            Assert.AreEqual("TestAttribute", attribute.Name, "Attribute name was incorrect");
        }

        [TestMethod]
        public void Generated_Node_Attribute_Has_Correct_Property_Type()
        {
            var xom = new XomReader();
            var node = xom.GenerateNodes(typeof(SimpleAttributeNode));
            var attribute = node.First().Attributes.First();

            Assert.AreEqual(typeof(string), attribute.Type, "Attribute type was incorrect");
        }
    }
}
