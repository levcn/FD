using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fw.Entity
{
    public class StoredProcedureParams
    {
        public string StoredProcedureName { get; set; }
//        public List<string> ParamsName { get; set; }
//        public List<string> ParamsValue { get; set; }

        public List<STParamete> Params { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<OutputValue> OutputValues { get; set; }
    }
    /// <summary>
    /// 存储过程的参数名和值
    /// </summary>
    public class STParamete
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
