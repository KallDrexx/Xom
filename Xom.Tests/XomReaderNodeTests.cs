using System;
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
            var root = nodes.FirstOrDefault(x => x.Type == typeof(SimpleElementChildNode));

            Assert.IsNotNull(root.Children, "Root's children array was null");
            Assert.AreEqual(1, root.Children.Count(), "Root has an incorrect number of children");
            Assert.IsNotNull(root.Children.First().AvailableNodes, "Available nodes enumerable was null");
            Assert.AreEqual(1, root.Children.First().AvailableNodes.Count(), "Available child nodes enumerable had an incorrect number of elements");
            Assert.AreEqual(typeof(SimpleElementChildNode.ChildNode), root.Children.First().AvailableNodes.First().Value.Type,
                "Root node references incorrect type of child node");
        }
    }
}
