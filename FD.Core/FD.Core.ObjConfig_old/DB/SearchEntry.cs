using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLFW.DB
{
    /// <summary>
    /// 搜索条件
    /// </summary>
    public class SearchEntity
    {
        public SearchEntity()
        {
            Flag = " = ";
        }
//        /// <summary>
//        /// 返回表达式
//        /// </summary>
//        /// <typeparam name="TEntity"></typeparam>
//        /// <param name="predicate"></param>
//        /// <returns></returns>
//        static public string Create<TEntity>(Expression<Func<TEntity, bool>> predicate)
//        {
//            ConditionBuilder builder = new ConditionBuilder();
//            builder.Build(predicate.Body);
//            string sqlCondition = builder.Condition;
//            return sqlCondition;
//        }
        public SearchEntity(string columnName, string value, string flag, string groupName = null)
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
