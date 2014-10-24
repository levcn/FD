using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fw.Entity
{
    public class Header
    {
        public string TypeName { get; set; }
        public Dictionary<Guid, string> Operators { get; set; }
        public PageInfo PageInfo { get; set; }

        public string ConditionString { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
    }
}
