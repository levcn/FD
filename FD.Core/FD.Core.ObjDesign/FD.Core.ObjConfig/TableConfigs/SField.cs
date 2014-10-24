using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STComponse.TableConfigs
{
    public class SField
    {
        /// <summary>
        /// 表名(中文)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表名(数据库表名)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public string MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public string MinValue { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        public string DataLength { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Comment { get; set; }
    }
}
