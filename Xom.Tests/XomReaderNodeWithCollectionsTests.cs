using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xom.Core;
using Xom.Tests.TestObjects.Nodes;

namespace Xom.Tests
{
    [TestClass]
    public class XomReaderNodeWithCollectionsTests
    {
        [TestMethod]
        public void XmlArray_Child_Node_Name_Is_Explicitly_Stated_Name()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(ExplicitlyNamedXmlArrayNode));
            var root = nodes.First(x => x.Type == typeof(ExplicitlyNamedXmlArrayNode));
            var child = root.Children.First();

            Assert.AreEqual("Name", child.AvailableNodes.First().Key, "Child node's name was incorrect");
        }

        [TestMethod]
        public void Enumerables_Are_Marked_As_Being_A_Collection()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(XmlArrayChildNode));
            var root = nodes.First(x => x.Type == typeof(XmlArrayChildNode));
            var child = root.Children.First();

            Assert.IsTrue(child.IsCollection, "Child was marked as not a collection");
        }

        [TestMethod]
        public void Enumerables_Children_Have_Type_Set_To_Inner_Type()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(XmlArrayChildNode));
            var root = nodes.First(x => x.Type == typeof(XmlArrayChildNode));
            var child = root.Children.First();

            Assert.AreEqual(typeof(string), child.AvailableNodes.First().Value.Type, "Child type was incorrect");
        }

        [TestMethod]
        public void Enumerable_Children_Have_Child_Nodes_For_Inner_Type()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(NodeWithListOfClasses));
            var root = nodes.First(x => x.Type == typeof(NodeWithListOfClasses));
            var child = root.Children.First().AvailableNodes.First().Value;

            Assert.AreEqual(1, child.Children.Count(), "Incorrect number of inner children elements");
            Assert.AreEqual(1, child.Children.First().AvailableNodes.Count(), "Incorrect number of inner children available nodes");
            Assert.AreEqual("InnerChild", child.Children.First().AvailableNodes.First().Key, "Inner child available node has an incorrect name");
            Assert.AreEqual(typeof(string), child.Children.First().AvailableNodes.First().Value.Type, "Inner child node type was incorrect");
        }
    }
}
