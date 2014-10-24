using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using STComponse.CFG;


namespace Fw.Reflection
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
    public class TableAndColumnsFC
    {
        public EDataObject Type { get; set; }
        /// <summary>
        /// 当前对象的表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 当前对象的属性列表
        /// </summary>
        public ObservableCollection<Property> PropertyInfos { get; set; }
    }
}
