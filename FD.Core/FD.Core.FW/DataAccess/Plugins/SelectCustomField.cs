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
    /// ��ѯ������ݴ�����(����Զ�����)
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
            //�ҳ���ǰ���͵�SP����
            var propers = ReflectionHelper.GetPropertyByAttributeType<LevcnSPColumnAttribute>(type);

            //���˺ڰ�����
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
        /// ����ǰ��������б����һ������ֵ
        /// </summary>
        /// <param name="access"></param>
        /// <param name="ids"></param>
        /// <param name="pwa"></param>
        /// <param name="data">�б�</param>
        /// <param name="propertyInfo">Ҫ�������ֵ������</param>
        /// <param name="primaryKey">��ǰ�б����͵���������</param>
        private void AddColumnValue(BaseDataAccess access, IEnumerable<string> ids, LevcnSPColumnAttribute pwa, object data, PropertyInfo propertyInfo, PropertyInfo primaryKey)
        {
            var listobj = ReflectionHelper.GetListItems(data);
            //����,�����ֶε�����
            var type = ReflectionHelper.GetTypeByProperty(propertyInfo);
            var prop = type.GetProperty(primaryKey.Name);
            if(prop==null) return;//�������������û���������������,���˳�
            var pobj = access.Select(type, pwa.StoredProcedureName, new List<STParamete>{
                new STParamete{Name =pwa.ParameteName,Value = ids.Serialize(",")}});
            //pobj��һ�����,������,����һ�е�������data
            if(pobj!=null)
            {
                var pobjs = ReflectionHelper.GetListItems(pobj);
                listobj.ForEach(
                    w=>
                        {
                            //ȡ����ǰ��¼������ֵ
                            var prikeyValue = ReflectionHelper.GetPropertyValue(prop, w).ToString().ToLower();
                            //�ڴ洢���̲���Ľ�����е�ǰ��¼��ֵ
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