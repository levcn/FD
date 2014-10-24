using System;
using System.Collections.Generic;
using System.Reflection;


namespace StaffTrain.FwClass
{
    public class TableAndColumns
    {
        public Type Type { get; set; }
        /// <summary>
        /// 当前对象的表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 当前对象的属性列表
        /// </summary>
        public List<PropertyInfo> PropertyInfos { get; set; }
    }
}
