using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fw.UserAttributes
{
    /// <summary>
    /// 执行存储过程返回的列
    /// </summary>
    public class LevcnSPColumnAttribute : Attribute
    {
        /// <summary>
        /// 存储过程的名子
        /// </summary>
        public string StoredProcedureName { get; set; }

        /// <summary>
        /// 存储过程的参数
        /// </summary>
        public string ParameteName { get; set; }
    }
}
