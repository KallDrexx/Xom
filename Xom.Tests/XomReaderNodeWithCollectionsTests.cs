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
    }
}
