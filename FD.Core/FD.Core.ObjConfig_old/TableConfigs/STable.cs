using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.SqlServer.Server;


namespace STComponse.TableConfigs
{
    /// <summary>
    /// 数据表
    /// </summary>
    public class STable
    {
        /// <summary>
        /// 表名(中文)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表名(数据库表名)
        /// </summary>
        public string Code { get; set; }

        public List<SField> Fields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Comment { get; set; }
    }
}
