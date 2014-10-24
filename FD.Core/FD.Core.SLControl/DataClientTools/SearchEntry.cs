using System;
using System.Linq.Expressions;
//using Fw.Extends.Table;
//using StaffTrain.FwClass.Extends.Linq;
using Fw.Extends.Table;


namespace StaffTrain.FwClass.DataClientTools
{
    /// <summary>
    /// 搜索条件
    /// </summary>
    public class SearchEntry
    {
        public SearchEntry()
        {
            Flag = " = ";
        }
        /// <summary>
        /// 返回表达式
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        static public string Create<TEntity>(Expression<Func<TEntity, bool>> predicate)
        {
            ConditionBuilder builder = new ConditionBuilder();
            builder.Build(predicate.Body);
            string sqlCondition = builder.Condition;
            return sqlCondition;
        }
        public SearchEntry(string columnName, string value, string flag, string groupName = null)
        {
            GroupName = groupName;
            ColumnName = columnName;
            this.Flag = flag;
            this.value = value;
        }

        public string GroupName;
        public string ColumnName; //列名
        public string value;

        /// <summary>
        /// 默认是 = 
        /// </summary>
        public string Flag { get; set; }
    }
}