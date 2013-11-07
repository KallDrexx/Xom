using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xom.Core.Models;

namespace Xom.Core
{
    public static class XomAttributeTypeGenerator
    {
        public const string GeneratedNamespacePrefix = "XomAttributeGeneratedType.";

        public static Type GenerateType(IEnumerable<XomNodeAttribute> xomAttributes, string typeName = null)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                typeName = Guid.NewGuid().ToString();

            // Algorithm based on http://stackoverflow.com/a/3862241/231002
            var typebuilder = GetTypeBuilder(typeName);
            typebuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName |
                                                 MethodAttributes.RTSpecialName);

            xomAttributes = xomAttributes ?? new XomNodeAttribute[0];
            foreach (var xomNodeAttribute in xomAttributes)
                CreateProperty(typebuilder, xomNodeAttribute);

            return typebuilder.CreateType();
        }

        private static TypeBuilder GetTypeBuilder(string typeName)
        {
            var an = new AssemblyName(GeneratedNamespacePrefix + typeName);
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            var typeBuilder = moduleBuilder.DefineType(typeName,
                                                       TypeAttributes.Public |
                                                       TypeAttributes.Class |
                                                       TypeAttributes.AutoClass |
                                                       TypeAttributes.AnsiClass |
                                                       TypeAttributes.BeforeFieldInit |
                                                       TypeAttributes.AutoLayout,
                                                       null);

            return typeBuilder;
        }

        private static void CreateProperty(TypeBuilder typeBuilder, XomNodeAttribute attribute)
        {
            var attributeType = attribute.Type;
            if (attributeType.IsValueType && !attribute.IsRequired)
                attributeType = typeof (Nullable<>).MakeGenericType(attributeType);

            var fieldBuilder = typeBuilder.DefineField("_" + attribute.Name, attributeType, FieldAttributes.Private);
            var propertyBuilder = typeBuilder.DefineProperty(attribute.Name, PropertyAttributes.HasDefault,
                                                             attributeType, null);

            var getPropMthdBldr = typeBuilder.DefineMethod("get_" + attribute.Name,
                                                           MethodAttributes.Public | MethodAttributes.SpecialName |
                                                           MethodAttributes.HideBySig, attributeType, Type.EmptyTypes);

            var getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                typeBuilder.DefineMethod("set_" + attribute.Name,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { attributeType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
}
