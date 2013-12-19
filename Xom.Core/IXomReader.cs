using System;
using System.Collections.Generic;
using Xom.Core.Models;

namespace Xom.Core
{
    public interface IXomReader
    {
        IEnumerable<XomNode> GenerateNodes(Type type);
    }
}
