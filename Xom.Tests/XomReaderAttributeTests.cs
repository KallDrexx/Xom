using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xom.Core;
using Xom.Tests.TestObjects.Attributes;

namespace Xom.Tests
{
    [TestClass]
    public class XomReaderAttributeTests
    {
        [TestMethod]
        public void Generated_Node_Has_The_Correct_Number_Of_Attributes()
        {
            var xom = new XomReader();
            var node = xom.GenerateNodes(typeof(MultipleAttributeNode))
                           .First();

            Assert.IsNotNull(node.Attributes, "Node attributes were null");
            Assert.AreEqual(2, node.Attributes.Count(), "Incorrect number of attributes on node");
        }

        [TestMethod]
        public void Generated_Node_Only_Has_Annotated_Xml_Attributes()
        {
            var xom = new XomReader();
            var node = xom.GenerateNodes(typeof(SimpleAttributeNode))
                          .First();

            Assert.AreEqual(1, node.Attributes.Count(), "Incorrect number of attributes on node");
        }

        [TestMethod]
        public void Generated_Node_Attribute_Has_Same_Name_As_Property()
        {
            var xom = new XomReader();
            var node = xom.GenerateNodes(typeof(SimpleAttributeNode));
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

        [TestMethod]
        public void Attribute_Is_Not_Required_When_Type_Is_Not_A_Value_Type()
        {
            var xom = new XomReader();
            var node = xom.GenerateNodes(typeof(SimpleAttributeNode));
            var attribute = node.First().Attributes.First();

            Assert.IsFalse(attribute.IsRequired, "String attribute was incorrectly marked as required");
        }

        [TestMethod]
        public void Attribute_Is_Required_For_Value_Types()
        {
            var xom = new XomReader();
            var node = xom.GenerateNodes(typeof(ValueTypeAttributeNode));
            var attribute = node.First().Attributes.First();

            Assert.IsTrue(attribute.IsRequired, "Int attribute was incorrectly marked as not required");
        }

        [TestMethod]
        public void Attribute_Is_Not_Required_If_Value_Specified_Property_Exists()
        {
            var xom = new XomReader();
            var node = xom.GenerateNodes(typeof(ValueSpecifiedAttributeNode));
            var attribute = node.First().Attributes.First();

            Assert.IsFalse(attribute.IsRequired, "Value specified attribute was incorrectly marked as required");
        }
    }
}
