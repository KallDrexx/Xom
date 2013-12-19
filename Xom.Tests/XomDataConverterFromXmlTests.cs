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
        private XomReader _xomReader;

        [TestInitialize]
        public void Setup()
        {
            _xomReader = Mock.Create<XomReader>();
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
    }
}
