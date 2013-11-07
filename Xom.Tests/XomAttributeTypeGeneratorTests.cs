using System;
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
            var attr2 = new XomNodeAttribute {Name = "attr2", Type = typeof (int), IsRequired = true};

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
        public void Null_Name_Creates_Guid_Type_Name()
        {
            var attr1 = new XomNodeAttribute { Name = "attr1", Type = typeof(string) };
            var type = XomAttributeTypeGenerator.GenerateType(new[] { attr1 }, null);
            Guid.Parse(type.Name);
        }

        [TestMethod]
        public void Empty_Name_Creates_Guid_Type_Name()
        {
            var attr1 = new XomNodeAttribute { Name = "attr1", Type = typeof(string) };
            var type = XomAttributeTypeGenerator.GenerateType(new[] { attr1 }, "  ");
            Guid.Parse(type.Name);
        }

        [TestMethod]
        public void Type_With_No_Properties_Returned_If_Null_Attributes_Enumerable_Passed_In()
        {
            var type = XomAttributeTypeGenerator.GenerateType(null, "Test");
            Assert.IsNotNull(type, "Null type returned");
            Assert.IsFalse(type.GetProperties().Any(), "Type incorrectly had one or more properties");
        }

        [TestMethod]
        public void Non_Required_Value_Types_Are_Created_As_Nullable_Properties()
        {
            var attr = new XomNodeAttribute { Name = "attr1", Type = typeof(int), IsRequired = false};
            var type = XomAttributeTypeGenerator.GenerateType(new[] {attr}, "Test");
            var properties = type.GetProperties();
            Assert.IsTrue(properties[0].PropertyType == typeof(int?));
        }
    }
}
