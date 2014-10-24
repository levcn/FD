using System;


namespace Fw.Entity
{
    /// <summary>
    /// 搜索条件
    /// </summary>
    public class SearchEntry
    {
        /// <summary>
        /// 查询分组(组名相同则使用Or,不同则使用And,未设置组名默认为单独一组)
        /// </summary>
        public string GroupName;
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName; //列名
        /// <summary>
        /// 逻辑符号
        /// </summary>
        public string Flag = " = "; //逻辑符号
        public string value;
        public string GetSearchValue()
        {
            if (Flag.IndexOf("in", StringComparison.OrdinalIgnoreCase) == -1)
            {
                var v = value;
                bool removed = false;
                if (value.Length > 2 && value.StartsWith("'") && value.EndsWith("'"))
                {
                    v = value.Substring(1, value.Length - 2);
                    removed = true;
                }
                v = v.Replace("'", "''");
                if (removed)
                {
                    v = string.Format("'{0}'",v);
                }
                return v;
            }
            return value;
        }
    }
}