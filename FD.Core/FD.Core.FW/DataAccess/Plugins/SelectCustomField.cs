using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fw.Entity;
using Fw.Extends;
using Fw.Reflection;
using Fw.UserAttributes;


namespace Fw.DataAccess.Plugins
{
    /// <summary>
    /// 查询结果数据处理插件(添加自定义列)
    /// </summary>
    public class SelectAfterCustomField : ISelectAfter
    {
        #region Implementation of ISelectAfter

        public void Execute(IList data, Type type, ActionCommand cmd, BaseDataAccess access)
        {
            return;
            if (cmd.Parameter.FieldFilter == null||cmd.Parameter.FieldFilter.WhiteNames==null||cmd.Parameter.FieldFilter.WhiteNames.Length==0) return;
            var blackName = cmd.Parameter.FieldFilter.BlackNames??new string[]{};
            var whiteName = cmd.Parameter.FieldFilter.WhiteNames;
            //找出当前类型的SP类型
            var propers = ReflectionHelper.GetPropertyByAttributeType<LevcnSPColumnAttribute>(type);

            //过滤黑白名单
            propers = propers.Where(w => whiteName.Contains(w.Name)).Where(w => !blackName.Contains(w.Name)).ToList();

            var primaryKey = ReflectionHelper.GetPrimaryKey(type);
            var ids = ReflectionHelper.GetListItemValues(data, w => ReflectionHelper.GetPropertyValue(primaryKey,w)).Select(w=>w.ToString());
            if (propers.Count > 0)
                {
                    propers.ForEach(
                        w =>
                        {
                            var pwa = ReflectionHelper.GetAttribute<LevcnSPColumnAttribute>(w);
                            AddColumnValue(access, ids, pwa, data, w, primaryKey);
                        });
                }
        }

        public void Execute(IList data, ActionCommand cmd, BaseDataAccess access)
        {
            
        }

        /// <summary>
        /// 给当前查出来的列表添加一个属性值
        /// </summary>
        /// <param name="access"></param>
        /// <param name="ids"></param>
        /// <param name="pwa"></param>
        /// <param name="data">列表</param>
        /// <param name="propertyInfo">要添加属性值的类型</param>
        /// <param name="primaryKey">当前列表类型的主键属性</param>
        private void AddColumnValue(BaseDataAccess access, IEnumerable<string> ids, LevcnSPColumnAttribute pwa, object data, PropertyInfo propertyInfo, PropertyInfo primaryKey)
        {
            var listobj = ReflectionHelper.GetListItems(data);
            //类型,属性字段的类型
            var type = ReflectionHelper.GetTypeByProperty(propertyInfo);
            var prop = type.GetProperty(primaryKey.Name);
            if(prop==null) return;//如果关联属性中没有主表的主键属性,则退出
            var pobj = access.Select(type, pwa.StoredProcedureName, new List<STParamete>{
                new STParamete{Name =pwa.ParameteName,Value = ids.Serialize(",")}});
            //pobj是一个表格,至两列,其中一列的列名是data
            if(pobj!=null)
            {
                var pobjs = ReflectionHelper.GetListItems(pobj);
                listobj.ForEach(
                    w=>
                        {
                            //取出当前记录主键的值
                            var prikeyValue = ReflectionHelper.GetPropertyValue(prop, w).ToString().ToLower();
                            //在存储过程查出的结果中有当前记录的值
                            var val = pobjs.FirstOrDefault(q => ReflectionHelper.GetPropertyValue(prop, q).ToString().ToLower() == prikeyValue);
                            if (val != null)
                            {
                                propertyInfo.SetValue(w,val,null);
                            }
                        });
            }
        }

        #endregion
    }
}