using System;
using System.Linq.Expressions;
//using Fw.Extends.Table;
//using StaffTrain.FwClass.Extends.Linq;
using Fw.Extends.Table;


namespace StaffTrain.FwClass.DataClientTools
{
    /// <summary>
    /// ��������
    /// </summary>
    public class SearchEntry
    {
        public SearchEntry()
        {
            Flag = " = ";
        }
        /// <summary>
        /// ���ر��ʽ
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
        public string ColumnName; //����
        public string value;

        /// <summary>
        /// Ĭ���� = 
        /// </summary>
        public string Flag { get; set; }
    }
}