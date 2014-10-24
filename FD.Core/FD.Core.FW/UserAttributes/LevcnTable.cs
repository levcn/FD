using System;


namespace Fw.UserAttributes
{
    public class LevcnTableAttribute : Attribute
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class LevcnColumnAttribute : Attribute
    {
        public bool IsAutoInt { get; set; }
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否忽略,不从数据库中查询
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// 是否是大字段(使用大字段存放一个序列化的类)
        /// </summary>
        public bool IsBigField { get; set; }
        /// <summary>
        /// 是否需要导出模板来来编辑
        /// </summary>
        public bool IsNeedImport { get; set; }
    }
}