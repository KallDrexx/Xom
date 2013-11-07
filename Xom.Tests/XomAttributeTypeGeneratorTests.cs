﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xom.Core;
using Xom.Core.Models;

namespace Xom.Tests
{
    [TestClass]
    public class XomAttributeTypeGeneratorTests
    {
        [TestMethod]
        public void Can_Generate_Class_Based_On_Xom_Attributes()
        {
            var attr1 = new XomNodeAttribute {Name = "attr1", Type = typeof (string)};
            var attr2 = new XomNodeAttribute {Name = "attr2", Type = typeof (int)};

            Type type = XomAttributeTypeGenerator.GenerateType(new[] {attr1, attr2}, "TestName");

            Assert.IsNotNull(type, "Returned type was null");

            var properties = type.GetProperties();
            Assert.AreEqual(2, properties.Length, "Incorrect number of properties returned");
            Assert.IsTrue(properties.Any(x => x.PropertyType == typeof(string) && x.Name == attr1.Name), "No string property exists with the name attr1");
            Assert.IsTrue(properties.Any(x => x.PropertyType == typeof(int) && x.Name == attr2.Name), "No int property exists with the name attr2");
        }

        [TestMethod]
        public void Can_Specify_Name_For_Generated_Type()
        {
            var name = "TestName";
            var attr1 = new XomNodeAttribute { Name = "attr1", Type = typeof(string) };
            Type type = XomAttributeTypeGenerator.GenerateType(new[] {attr1}, name);

            Assert.AreEqual(name, type.Name, "Type's name was incorrect");
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void Exception_Thrown_When_Null_Name_Specified()
        {
            var attr1 = new XomNodeAttribute { Name = "attr1", Type = typeof(string) };
            XomAttributeTypeGenerator.GenerateType(new[] { attr1 }, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Exception_Thrown_When_Empty_Name_Specified()
        {
            var attr1 = new XomNodeAttribute { Name = "attr1", Type = typeof(string) };
            XomAttributeTypeGenerator.GenerateType(new[] { attr1 }, "  ");
        }

        [TestMethod]
        public void Type_With_No_Properties_Returned_If_Null_Attributes_Enumerable_Passed_In()
        {
            var type = XomAttributeTypeGenerator.GenerateType(null, "Test");
            Assert.IsNotNull(type, "Null type returned");
            Assert.IsFalse(type.GetProperties().Any(), "Type incorrectly had one or more properties");
        }
    }
}
