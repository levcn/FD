using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fw.Entity;

namespace Fw.DataAccess
{
    public class TranParam
    {
        /// <summary>
        /// 执行数据库操作的对象
        /// </summary>
        public object EntityObject { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public SqlOperType OperType { get; set; }
        /// <summary>
        /// Where条件
        /// </summary>
        public List<SearchEntry> Where { get; set; }
    }
}
