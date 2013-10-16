using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xom.Core;
using Xom.Tests.TestObjects.Attributes;

namespace Xom.Tests
{
    [TestClass]
    public class XomReaderNodeTests
    {
        [TestMethod]
        public void Can_Map_Single_Node_Type()
        {
            var xom = new XomReader();
            var nodes = xom.GenerateNodes(typeof(SimpleAttributeNode));

            Assert.IsNotNull(nodes, "Null node enumerable returned");
            Assert.AreEqual(1, nodes.Count(), "Incorrect number of nodes returned");
            Assert.AreEqual(typeof(SimpleAttributeNode), nodes.First().Type, "Node had an incorrect type");
        }
    }
}
