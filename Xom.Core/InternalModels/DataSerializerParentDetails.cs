using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xom.Core.Models;

namespace Xom.Core.InternalModels
{
    class DataSerializerParentDetails
    {
        public object ParentObject { get; set; }
        public XomNode ParentXomNode { get; set; }
        public string ChildNodeName { get; set; }
    }
}
