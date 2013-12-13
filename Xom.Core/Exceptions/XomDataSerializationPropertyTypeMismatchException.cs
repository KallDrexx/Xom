using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xom.Core.Exceptions
{
    public class XomDataSerializationPropertyTypeMismatchException : XomDataSerializationException
    {
        public Type SourceType { get; private set; }
        public string SourceName { get; private set; }
        public Type TargetType { get; private set; }
        public string TargetName { get; private set; }

        public XomDataSerializationPropertyTypeMismatchException(Type sourceType, string sourceName, Type targetType, string targetName)
            : base(string.Format("The source property {0} ({1}) has an incompatible type with the target property {2} ({3})",
                                    sourceName, sourceType, targetName, targetType))
        {
            SourceName = sourceName;
            TargetName = targetName;
            SourceType = SourceType;
            TargetType = targetType;
        }
    }
}
