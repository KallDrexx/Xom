﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xom.Core;
using Xom.Tests.TestObjects.Attributes;
using Xom.Tests.TestObjects.Nodes;

namespace Xom.Tests
{
    [TestClass]
    public class XomReaderNodeTests
    {
        [TestMethod]
        public void Can_Map_Single_Node_Type()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(SingleNode));

            Assert.IsNotNull(nodes, "Null node enumerable returned");
            Assert.AreEqual(1, nodes.Count(), "Incorrect number of nodes returned");
            Assert.AreEqual(typeof(SingleNode), nodes.First().Type, "Node had an incorrect type");
        }

        [TestMethod]
        public void Returns_Node_And_XmlElement_Child_Node()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(SimpleElementChildNode));

            Assert.AreEqual(2, nodes.Count(), "Incorrect number of nodes returned");
            Assert.IsTrue(nodes.Any(x => x.Type == typeof(SimpleElementChildNode)), "Root node was not returned");
            Assert.IsTrue(nodes.Any(x => x.Type == typeof(SimpleElementChildNode.ChildNode)), "Child node was not returned");
        }

        [TestMethod]
        public void Only_Root_Node_Flagged_As_Root()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(SimpleElementChildNode));
            var root = nodes.FirstOrDefault(x => x.Type == typeof (SimpleElementChildNode));
            var child = nodes.FirstOrDefault(x => x.Type == typeof (SimpleElementChildNode.ChildNode));
            
            Assert.IsTrue(root.IsRoot, "Root node was not marked as root");
            Assert.IsFalse(child.IsRoot, "Child node incorrectly marked as root");
        }

        [TestMethod]
        public void Root_Node_References_XmlElement_Child_Node()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(SimpleElementChildNode));
            var root = nodes.First(x => x.Type == typeof(SimpleElementChildNode));

            Assert.IsNotNull(root.Children, "Root's children array was null");
            Assert.AreEqual(1, root.Children.Count(), "Root has an incorrect number of children");
            Assert.IsNotNull(root.Children.First().AvailableNodes, "Available nodes enumerable was null");
            Assert.AreEqual(1, root.Children.First().AvailableNodes.Count(), "Available child nodes enumerable had an incorrect number of elements");
            Assert.AreEqual(typeof(SimpleElementChildNode.ChildNode), root.Children.First().AvailableNodes.First().Value.Type,
                "Root node references incorrect type of child node");
        }

        [TestMethod]
        public void Nodes_For_Same_Type_Are_Not_Duplicated()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(SelfReferencingNode));

            Assert.AreEqual(1, nodes.Count(), "Incorrect number of nodes returned");
            Assert.AreEqual(typeof(SelfReferencingNode), nodes.First().Type, "Node returned had an incorrect type");
        }

        [TestMethod]
        public void Non_Xml_Attributed_Property_Counted_As_Child()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(NonAttributedNode));
            var root = nodes.First(x => x.Type == typeof(NonAttributedNode));

            Assert.IsNotNull(root.Children, "Root's children array was null");
            Assert.AreEqual(1, root.Children.Count(), "Root has an incorrect number of children");
            Assert.IsNotNull(root.Children.First().AvailableNodes, "Available nodes enumerable was null");
            Assert.AreEqual(1, root.Children.First().AvailableNodes.Count(), "Available child nodes enumerable had an incorrect number of elements");
            Assert.AreEqual(typeof(string), root.Children.First().AvailableNodes.First().Value.Type,
                "Root node references incorrect type of child node");
        }

        [TestMethod]
        public void XmlAttribute_Child_Does_Not_Count_As_Node()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(NodeWithAttribute));
            var root = nodes.First(x => x.Type == typeof(NodeWithAttribute));

            Assert.IsNotNull(root.Children, "Root's children array was null");
            Assert.AreEqual(0, root.Children.Count(), "Root has an incorrect number of children");
        }

        [TestMethod]
        public void XmlIgnored_Child_Does_Not_Count_As_Node()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(IgnoredChildNode));
            var root = nodes.First(x => x.Type == typeof(IgnoredChildNode));

            Assert.IsNotNull(root.Children, "Root's children array was null");
            Assert.AreEqual(0, root.Children.Count(), "Root has an incorrect number of children");
        }

        [TestMethod]
        public void Child_Contains_Name_Of_Property_For_Parent()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(SingleNode));
            var root = nodes.First(x => x.Type == typeof(SingleNode));

            Assert.AreEqual("Child", root.Children.First().PropertyName, "Child's property name was incorrect");
        }

        [TestMethod]
        public void XmlElement_Child_Is_Not_Marked_As_An_Array()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(SingleNode));
            var root = nodes.First(x => x.Type == typeof(SingleNode));
            var child = root.Children.First();

            Assert.IsFalse(child.IsXmlArray, "XmlElement was incorrectly marked as an xml array");
        }

        [TestMethod]
        public void XmlArray_Child_Is_Marked_As_An_Array()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(XmlArrayChildNode));
            var root = nodes.First(x => x.Type == typeof(XmlArrayChildNode));
            var child = root.Children.First();

            Assert.IsTrue(child.IsXmlArray, "XmlArray was not marked as an xml array");
        }

        [TestMethod]
        public void Non_Enumerables_Marked_As_Not_A_Collection()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(SingleNode));
            var root = nodes.First(x => x.Type == typeof(SingleNode));
            var child = root.Children.First();

            Assert.IsFalse(child.IsCollection, "Child was incorrectly marked as a collection");
        }

        [TestMethod]
        public void Array_Children_Have_Type_Set_To_Inner_Type()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(ArrayChildNode));
            var root = nodes.First(x => x.Type == typeof(ArrayChildNode));
            var child = root.Children.First();

            Assert.AreEqual(typeof(int), child.AvailableNodes.First().Value.Type, "Child type was incorrect");
        }

        [TestMethod]
        public void Child_Node_Without_Explicit_Name_Is_Property_Name()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(SingleNode));
            var root = nodes.First(x => x.Type == typeof(SingleNode));
            var child = root.Children.First();

            Assert.AreEqual("Child", child.AvailableNodes.First().Key, "Child node's name was incorrect");
        }

        [TestMethod]
        public void Multiple_XmlElement_Attributes_On_Child_Create_Child_With_All_Names()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(MultipleSimpleChildNode));
            var root = nodes.First(x => x.Type == typeof(MultipleSimpleChildNode));
            var child = root.Children.First();

            Assert.AreEqual(1, root.Children.Count(), "Incorrect number of children on the root");
            Assert.AreEqual(2, child.AvailableNodes.Count(), "Incorrect number of nodes for child");
            Assert.IsTrue(child.AvailableNodes.Any(x => x.Key == "Test1"), "No available node was found with the name Test1");
            Assert.IsTrue(child.AvailableNodes.Any(x => x.Key == "Test2"), "No available node was found with the name Test2");
        }

        [TestMethod]
        public void Multiple_XmlElement_Attributes_Have_Correct_Node_Type_When_Subclasses_Set()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(MultipleTypedElementNode));
            var root = nodes.First(x => x.Type == typeof(MultipleTypedElementNode));
            var child = root.Children.First();

            Assert.AreEqual(2, child.AvailableNodes.Count(), "Incorrect number of nodes for child");
            Assert.IsTrue(child.AvailableNodes.Any(x => x.Key == "ClassB" && x.Value.Type == typeof(MultipleTypedElementNode.ClassB)), "No available node was found with the name ClassB");
            Assert.IsTrue(child.AvailableNodes.Any(x => x.Key == "ClassC" && x.Value.Type == typeof(MultipleTypedElementNode.ClassC)), "No available node was found with the name ClassC");
        }

        [TestMethod]
        public void Child_With_XmlElement_Subclass_Defined_In_Type_Has_Sublass_Set_For_Node()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(XmlElementChildWithSubtypeSetNode));
            var root = nodes.First(x => x.Type == typeof(XmlElementChildWithSubtypeSetNode));
            var child = root.Children.First();

            Assert.AreEqual(typeof(XmlElementChildWithSubtypeSetNode.ClassB), child.AvailableNodes.First().Value.Type,
                "Child's available node had an incorrect type");
        }

        [TestMethod]
        public void Child_With_XmlArrayItem_Subclass_Defined_In_Type_Has_Sublass_Set_For_Node()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(XmlArrayItemChildWithSubtypeSetNode));
            var root = nodes.First(x => x.Type == typeof(XmlArrayItemChildWithSubtypeSetNode));
            var child = root.Children.First();

            Assert.AreEqual(1, child.AvailableNodes.Count(), "Incorrect number of child available nodes");
            Assert.AreEqual(typeof(XmlArrayItemChildWithSubtypeSetNode.ClassB), child.AvailableNodes.First().Value.Type,
                "Child's available node had an incorrect type");
        }

        [TestMethod]
        public void Multiple_XmlArrayItem_Attributes_Have_Correct_Node_Type_When_Subclasses_Set()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(MultipleTypedArrayItemNode));
            var root = nodes.First(x => x.Type == typeof(MultipleTypedArrayItemNode));
            var child = root.Children.First();

            Assert.AreEqual(2, child.AvailableNodes.Count(), "Incorrect number of nodes for child");
            Assert.IsTrue(child.AvailableNodes.Any(x => x.Key == "ClassB" && x.Value.Type == typeof(MultipleTypedArrayItemNode.ClassB)), "No available node was found with the name ClassB");
            Assert.IsTrue(child.AvailableNodes.Any(x => x.Key == "ClassC" && x.Value.Type == typeof(MultipleTypedArrayItemNode.ClassC)), "No available node was found with the name ClassC");
        }

        [TestMethod]
        public void Properties_With_Private_Setter_Are_Not_Counted_As_Elements()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(NodeWithPrivateSetterProperty));
            var root = nodes.First(x => x.Type == typeof(NodeWithPrivateSetterProperty));

            Assert.IsFalse(root.Children.Any(), "More than one child was incorrectly detected");
        }

        [TestMethod]
        public void Properties_Without_Setter_Are_Not_Counted_As_Elements()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(NodeWithoutSetterProperty));
            var root = nodes.First(x => x.Type == typeof(NodeWithoutSetterProperty));

            Assert.IsFalse(root.Children.Any(), "More than one child was incorrectly detected");
        }

        [TestMethod]
        public void Node_With_No_Children_Has_Empty_Children_Enumerable()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(NodeWithoutChildren));
            var root = nodes.First(x => x.Type == typeof(NodeWithoutChildren));

            Assert.IsNotNull(root.Children, "Children enumerable was null");
            Assert.IsFalse(root.Children.Any(), "Children enumerable was not empty");
        }
    }
}
