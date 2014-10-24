using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCreater.DB
{
    /// <summary>
    ///
    /// </summary>
    public class ColumnInfo
    {
        public ColumnInfo()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 是否允许空
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// 1 标识
        /// </summary>
        public int Identity { get; set; }

        /// <summary>
        /// 1 标识种子
        /// </summary>
        public int IdentitySeed { get; set; }

        /// <summary>
        /// 标识增量
        /// </summary>
        public int IdentityIncrement { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string ColumnDesc { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

    }
    public class TableInfo
    {
        public string TableName { get; set; }
        public string Comm { get; set; }
        public List<ColumnInfo> ColumnInfos { get; set; }
    }
}
