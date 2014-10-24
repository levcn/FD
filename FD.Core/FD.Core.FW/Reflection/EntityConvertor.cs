using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Fw.DataAccess;
using Fw.Entity;
using Fw.Serializer;
using Fw.UserAttributes;
using STComponse.CFG;
using STComponse.GCode;


namespace Fw.Reflection
{
    /// <summary>
    /// 数据表转为实体
    /// </summary>
    public static class EntityConvertor
    {
        public static List<T> GetEntity<T>(DataTable dt) where T : new()
        {
            List<T> re = new List<T>();
            dt.AsEnumerable().ToList().ForEach(w => re.Add((T)GetEntity(typeof(T),w)));
            return re;
        }
        /// <summary>
        /// 根据数据表返回一个对象集合List type
        /// </summary>
        /// <param name="includeTableName"></param>
        /// <param name="type1"></param>
        /// <param name="ds"></param>
        /// <param name="isByProcedure">是否已经在存储过程中完成分页</param>
        /// <returns></returns>
        public static IList GetEntity(Assembly asse,EDataObject type,FwConfig fc, DataSet ds, ref PageInfo pageInfo, bool includeTableName = false, bool isByProcedure = false, string[] white = null, string[] black = null)
        {
            var types = asse.GetTypes();
            var objs = fc.DataObjects.ToList();
            objs.AddRange(fc.DictObject);
            var type2 = types.FirstOrDefault(w => w.Name == type.ObjectCode);
            if (ds.Tables.Count == 0)
            {
                return ReflectionHelper.GetObject(typeof(List<>).MakeGenericType(type2)) as IList;
            }
            var com = GetEntity1(fc,type2, ds.Tables[0], includeTableName) as IList;
            var associationType = type.Relation.ToList();//Where(w=>w.RelationType != Relation.字典).
            associationType = ReflectionHelper.GetBlack(associationType, black);
            associationType = ReflectionHelper.GetWhite(associationType, white);

            for (int i = 0; com != null && i < com.Count; i++)//遍历对象集合
            {
                int index = 1;
                associationType.ForEach(//遍历关联属性集合.
                        w =>
                        {
                            object item = com[i];//返回列表中的第i条记录
                            //                                object item = ReflectionHelper.GetListItem(com, i);//返回列表中的第i条记录
                            //var masterPrimaryKey = ReflectionHelper.GetPropertyField(ReflectionHelper.GetPrimaryKey(item.GetType()));//主表中的主键字段名
                            //var dicPrimaryKey = ReflectionHelper.GetPropertyField(ReflectionHelper.GetPrimaryKey(ReflectionHelper.GetTypeByProperty(w)));//字典表中的主键字段名
                            var thisKey = type.GetPrimary().Code;
                            var relConfig = w.RelConfig;
                            var otherKey = relConfig.RelMasertKey;
                            object v = ReflectionHelper.GetPropertyValue(thisKey, item);
                            //var masterPrimaryValue = ReflectionHelper.GetPrimaryValue(item);//当前记录主键的值
                            var relTableName = relConfig.RelTableName;

                            //var prop = w.GetValue(item, null);//返回该记录的当前属性的值
                            if ((w.RelationType == Relation.复杂关联 || w.RelationType == Relation.简单关联)
                                && index < ds.Tables.Count)//如果当前属性是列表
                            {
                                var masterKey = SqlFactory.GetColumnsRename(relTableName, otherKey);//在关联表中的字段名
                                var currentItemSubPropRows = ds.Tables[index]
                                    .AsEnumerable()
                                    .Where(q => q[masterKey] != null && q[masterKey].ToString() == v.ToString())//找出当前对象的行数据
                                    .ToList();
                                var typeCurrent = type2.GetProperty(w.ObjectPorertity);
                                if (w.RelationType == Relation.简单关联)
                                {
                                    var relType = types.FirstOrDefault(z => z.Name == relConfig.DictTableName);
                                    var value = GetEntity1(fc,w,relType, currentItemSubPropRows, true);
                                    typeCurrent.SetValue(item, value, null);
                                }
                                else if (w.RelationType == Relation.复杂关联)
                                {
                                    var relType = types.FirstOrDefault(z => z.Name == relConfig.RelTableName);
                                    var value = GetEntity1(fc,w,relType, currentItemSubPropRows, true);
                                    typeCurrent.SetValue(item, value, null);
                                }
                            }
                            else//字典
                            {
//                                thisKey = type.Code;
                                v = ReflectionHelper.GetPropertyValue(w.ObjectPorertity, item);
                                if (v != null && index < ds.Tables.Count)
                                {
                                    var otherKeyFullName = SqlFactory.GetColumnsRename(relConfig.DictTableName, relConfig.DictKey);
                                    //在关联表中的字段名
                                    var currentItemSubPropRows = ds.Tables[index]
                                            .AsEnumerable()
                                            .Where(q => q[otherKeyFullName].ToString() == v.ToString()).FirstOrDefault();
                                    var typeCurrent = types.FirstOrDefault(z => z.Name == relConfig.DictTableName);
                                    if (currentItemSubPropRows != null)
                                    {
                                        bool haveValue;
                                        var value = GetEntity1(fc,null,typeCurrent,currentItemSubPropRows, true,out haveValue);
                                        if (value != null)
                                        {
                                            var property = type2.GetProperty(relConfig.DictTableName);
                                            property.SetValue(item, value, null);
                                        }
                                    }
                                }
                            }
                            index++;
                        });
            }
            if (!isByProcedure)
            {//如果分页,而且有总记录数的表 不是在存储过程内分页
                if (pageInfo != null && ds.Tables.Count == associationType.Count + 2)
                {
                    var countTable = ds.Tables[associationType.Count + 1];
                    if (countTable.Rows.Count > 0)
                    {
                        pageInfo.TotalRecord = countTable.Rows[0].Field<int>(0);
                        pageInfo.TotalPage = (int)Math.Ceiling(pageInfo.TotalRecord / (float)pageInfo.PageSize);
                    }
                }
                else
                {
                    if (pageInfo != null)
                    {
                        if (com != null) pageInfo.TotalRecord = com.Count;
                        //                        pageInfo.TotalRecord = ReflectionHelper.GetListCount(com);
                    }
                }
                if (pageInfo != null)
                {
                    if (pageInfo.StartRecord > pageInfo.TotalRecord)
                    {
                        pageInfo.StartRecord = pageInfo.TotalRecord;
                        pageInfo.EndRecord = pageInfo.TotalRecord;
                    }
                    else
                    {
                        if (pageInfo.EndRecord > pageInfo.TotalRecord)
                        {
                            pageInfo.EndRecord = pageInfo.TotalRecord;
                        }
                    }
                }
            }
            //var p = type1.GetProperty("Header");

            //var addMethod = com.GetType().GetMethod("Add", new Type[] { type });
            //dt.Tables[""].AsEnumerable().ToList().ForEach(w =>
            //{
            //    addMethod.Invoke(com, new object[] { GetEntity(type, w) });
            //});
            return com;
        }
        /// <summary>
        /// 根据数据表返回一个对象集合List type
        /// </summary>
        /// <param name="includeTableName"></param>
        /// <param name="type1"></param>
        /// <param name="ds"></param>
        /// <param name="isByProcedure">是否已经在存储过程中完成分页</param>
        /// <returns></returns>
        public static IList GetEntity(Type type1, DataSet ds, ref PageInfo pageInfo, bool includeTableName = false, bool isByProcedure = false, string[] white = null, string[] black = null)
        {
            if (ds.Tables.Count == 0 )
            {
                return ReflectionHelper.GetObject(typeof(List<>).MakeGenericType(type1)) as IList;
            }
            var com = GetEntity(type1, ds.Tables[0], includeTableName) as IList;
            var associationType =
                    ReflectionHelper.GetBaseTypeProperty(type1, PropertyType.CustomType)
                    .Where(w => ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(w) != null)
                    .ToList();
            associationType = ReflectionHelper.GetBlack(associationType, black);
            associationType = ReflectionHelper.GetWhite(associationType, white);

            for (int i = 0; com!=null && i < com.Count; i++)
            {
                int index = 1;
                associationType.ForEach(
                        w =>
                            {
                                object item = com[i];//返回列表中的第i条记录
//                                object item = ReflectionHelper.GetListItem(com, i);//返回列表中的第i条记录
                                //var masterPrimaryKey = ReflectionHelper.GetPropertyField(ReflectionHelper.GetPrimaryKey(item.GetType()));//主表中的主键字段名
                                //var dicPrimaryKey = ReflectionHelper.GetPropertyField(ReflectionHelper.GetPrimaryKey(ReflectionHelper.GetTypeByProperty(w)));//字典表中的主键字段名
                                var thisKey = ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(w).ThisKey;
                                var otherKey = ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(w).OtherKey;
                                object v = ReflectionHelper.GetPropertyValue(thisKey, item);
                                //var masterPrimaryValue = ReflectionHelper.GetPrimaryValue(item);//当前记录主键的值
                                var relTableName = ReflectionHelper.GetTableName(w);
                                
                                //var prop = w.GetValue(item, null);//返回该记录的当前属性的值
                                if (w.PropertyType.IsGenericType && index < ds.Tables.Count)//如果当前属性是列表
                                {
                                    var masterKey = SqlFactory.GetColumnsRename(relTableName, otherKey);//在关联表中的字段名
                                    var currentItemSubPropRows = ds.Tables[index]
                                        .AsEnumerable()
                                        .Where(q => q[masterKey]!=null&& q[masterKey].ToString() == v.ToString()).ToList();
                                    var value = GetEntity(ReflectionHelper.GetTypeByProperty(w), currentItemSubPropRows,true);
                                    w.SetValue(item, value, null);
                                }
                                else//如果当前属性不是列表
                                {
                                    if (v != null && index < ds.Tables.Count)
                                    {
                                        var otherKeyFullName = SqlFactory.GetColumnsRename(relTableName, otherKey);
                                                //在关联表中的字段名
                                        var currentItemSubPropRows = ds.Tables[index]
                                                .AsEnumerable()
                                                .Where(q => q[otherKeyFullName].ToString() == v.ToString()).ToList();
                                        var value = GetEntity(ReflectionHelper.GetTypeByProperty(w),
                                                              currentItemSubPropRows,true) as IList;
                                        if (value != null && value.Count > 0)
                                        {
                                            w.SetValue(item, value[0], null);
//                                            w.SetValue(item, ReflectionHelper.GetListItem(value, 0), null);
                                        }
                                    }
                                }
                                index++;
                            });
            }
            if (!isByProcedure)
            {//如果分页,而且有总记录数的表 不是在存储过程内分页
                if (pageInfo != null && ds.Tables.Count == associationType.Count + 2)
                {
                    var countTable = ds.Tables[associationType.Count + 1];
                    if (countTable.Rows.Count > 0)
                    {
                        pageInfo.TotalRecord = countTable.Rows[0].Field<int>(0);
                        pageInfo.TotalPage = (int)Math.Ceiling(pageInfo.TotalRecord / (float)pageInfo.PageSize);
                    }
                }
                else
                {
                    if (pageInfo != null)
                    {
                        if (com != null) pageInfo.TotalRecord = com.Count;
                        //                        pageInfo.TotalRecord = ReflectionHelper.GetListCount(com);
                    }
                }
                if (pageInfo != null)
                {
                    if (pageInfo.StartRecord > pageInfo.TotalRecord)
                    {
                        pageInfo.StartRecord = pageInfo.TotalRecord;
                        pageInfo.EndRecord = pageInfo.TotalRecord;
                    }
                    else
                    {
                        if (pageInfo.EndRecord > pageInfo.TotalRecord)
                        {
                            pageInfo.EndRecord = pageInfo.TotalRecord;
                        }
                    }
                }
            }            
            //var p = type1.GetProperty("Header");
            
            //var addMethod = com.GetType().GetMethod("Add", new Type[] { type });
            //dt.Tables[""].AsEnumerable().ToList().ForEach(w =>
            //{
            //    addMethod.Invoke(com, new object[] { GetEntity(type, w) });
            //});
            return com;
        }



        /// <summary>
        /// 根据数据表返回一个对象集合List type
        /// </summary>
        /// <param name="type"> </param>
        /// <param name="dt"></param>
        /// <param name="includeTableName">字段中是否包含表名</param>
        /// <returns></returns>
        public static object GetEntity(Type type, DataTable dt, bool includeTableName = true)
        {
            return GetEntity(type, dt.AsEnumerable().ToList(), includeTableName);
        }
         static object GetEntity(Type type, List<DataRow> drs, bool includeTableName = true)
         {
             try
             {
                 var com = ReflectionHelper.GetObject(typeof (List<>).MakeGenericType(type)); //.GetConstructor(new Type[]{}).Invoke(null);
                 var addMethod = com.GetType().GetMethod("Add", new [] {type});
                 drs.ForEach(w => addMethod.Invoke(com, new[] {GetEntity(type, w, includeTableName)}));
                 return com;
             }
             catch (Exception e)
             {
                 throw e;
             }
         }
         /// <summary>
         /// 根据数据表返回一个对象集合List type
         /// </summary>
         /// <param name="type"> </param>
         /// <param name="dt"></param>
         /// <param name="includeTableName">字段中是否包含表名</param>
         /// <returns></returns>
         public static object GetEntity1(FwConfig fc,Type type, DataTable dt, bool includeTableName = true)
         {
             return GetEntity1(fc,null,type, dt.AsEnumerable().ToList(), includeTableName);
         }
         static object GetEntity1(FwConfig fc,Relation rel,Type type, List<DataRow> drs, bool includeTableName = true)
         {
             try
             {
                 var com = ReflectionHelper.GetObject(typeof(List<>).MakeGenericType(type)); //.GetConstructor(new Type[]{}).Invoke(null);
                 var addMethod = com.GetType().GetMethod("Add", new[] { type });
                 drs.ForEach(w => addMethod.Invoke(com, new[] { GetEntity1(fc,rel,type, w, includeTableName) }));
                 return com;
             }
             catch (Exception e)
             {
                 throw e;
             }
         }
         public static object GetEntity1(FwConfig fc,Relation rel,Type type2, DataRow dr, bool includeTableName = true)
         {
             bool _ha;
             return GetEntity1(fc,rel,type2, dr, includeTableName, out _ha);
         }
         public static object GetEntity(Type type2, DataRow dr, bool includeTableName = true)
        {
            bool _ha;
            return GetEntity(type2, dr, includeTableName, out _ha);
        }
         
        /// <summary>
        /// 根据数据列返回一个对象
        /// </summary>
        /// <param name="type2">返回一个该类型的数据</param>
        /// <param name="dr"></param>
        /// <param name="includeTableName">字段中是否包含表名 </param>
        /// <param name="haveValue">返回是否有值</param>
        /// <param name="parentType">父级的类型 </param>
        /// <param name="maxDeep">最多初始化的深度 </param>
        /// <returns></returns>
        public static object GetEntity(Type type2,DataRow dr,bool includeTableName ,out bool haveValue,Type parentType = null,int maxDeep = 3)
        {
            --maxDeep;
            var _haveValue = false;
            var type123 = typeof(BigFieldValue);
            var re = ReflectionHelper.GetObject(type2);
            if (type2 == typeof(string) || type2 == typeof(int))
            {
                haveValue = true;
                return dr[0];
            }
            var prop = ReflectionHelper.GetBaseTypeProperty(type2,PropertyType.BaseType,false); 
            var tableName = ReflectionHelper.GetTableName(type2);
            prop.ForEach(w =>
                             {
                                 var columnName = "";
                                 if (includeTableName)
                                     columnName = SqlFactory.GetColumnsRename(tableName, ReflectionHelper.GetPropertyField(w));
                                 else
                                     columnName = ReflectionHelper.GetPropertyField(w);
                                 if (dr.Table.Columns.Contains(columnName))
                                 {
                                     var d = dr[columnName];
                                     if(DBNull.Value != d)
                                     {
                                         _haveValue = true;
                                         var attr = ReflectionHelper.GetAttribute<LevcnColumnAttribute>(w);
                                         if ((attr != null && attr.IsBigField) || w.PropertyType == type123)
                                         {
                                             var value = JsonHelper.JsonDeserialize((string) d, w.PropertyType);
                                             w.SetValue(re,value,null);
                                         }
                                         else
                                         {
                                             setFieldValue(w, ref re, d);
                                         }

                                         //if(w.PropertyType == typeof(int?))
                                         //{
                                         //    w.SetValue(re, d, null);
                                         //}
                                         //else if (w.PropertyType == typeof(DateTime?))
                                         //{
                                         //    //var val = Convert.ChangeType(d, w.PropertyType);
                                         //    w.SetValue(re, d, null);
                                         //}
                                         //else{
                                         //    var val = Convert.ChangeType(d, w.PropertyType);
                                         //    w.SetValue(re, val, null);
                                         //}
                                     }
                                 }
                             });
            if (maxDeep >= 0)
            {
                var associationType = ReflectionHelper.GetBaseTypeProperty(type2, PropertyType.CustomType)
                    .Where(w => ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(w) != null && ReflectionHelper.GetTypeByProperty(w) != parentType).ToList();
                associationType.ForEach(
                        w =>
                            {
                                object subProp = null;
                                if (w.PropertyType.IsGenericType) //如果属性是个List
                                {
                                    var type = w.PropertyType;
                                    subProp = ReflectionHelper.GetObject(w.PropertyType);//.GetConstructor(new Type[] {}).Invoke(null));
                                    w.SetValue(re, subProp, null);
                                    var addMethod = type.GetMethod("Add");
                                    var subType = type.GetGenericArguments()[0];
                                    bool _ha;
                                    object subPropItem = GetEntity(subType, dr, true, out _ha, type2, maxDeep);
                                    if (_ha) addMethod.Invoke(subProp, new[] {subPropItem});
                                }
                                else //如果属性不是List
                                {
                                    //subProp = w.PropertyType.GetConstructor(null).Invoke(null);
                                    bool _ha;
                                    subProp = GetEntity(w.PropertyType, dr, true, out _ha, type2, maxDeep);
                                    if (_ha) w.SetValue(re, subProp, null);
                                }
                            });
            }
            haveValue = _haveValue;
            return re;
        }
        /// <summary>
        /// 根据数据列返回一个对象
        /// </summary>
        /// <param name="type2">返回一个该类型的数据</param>
        /// <param name="dr"></param>
        /// <param name="includeTableName">字段中是否包含表名 </param>
        /// <param name="haveValue">返回是否有值</param>
        /// <param name="parentType">父级的类型 </param>
        /// <param name="maxDeep">最多初始化的深度 </param>
        /// <returns></returns>
        public static object GetEntity1(FwConfig fc,Relation rel,Type type2, DataRow dr, bool includeTableName, out bool haveValue, Type parentType = null)
        {
//            --maxDeep;
            var _haveValue = false;

            var re = ReflectionHelper.GetObject(type2);
            if (type2 == typeof(string) || type2 == typeof(int))
            {
                haveValue = true;
                return dr[0];
            }
            var type = typeof(BigFieldValue);

            var prop = ReflectionHelper.GetBaseTypeProperty(type2, PropertyType.BaseType, false);
            var tableName = ReflectionHelper.GetTableName(type2);
            prop.ForEach(w =>
            {
                var columnName = "";
                if (includeTableName)
                    columnName = SqlFactory.GetColumnsRename(tableName, ReflectionHelper.GetPropertyField(w));
                else
                    columnName = ReflectionHelper.GetPropertyField(w);
                if (dr.Table.Columns.Contains(columnName))
                {
                    var d = dr[columnName];
                    if (DBNull.Value != d)
                    {
                        _haveValue = true;
                        var attr = ReflectionHelper.GetAttribute<LevcnColumnAttribute>(w);
                        if ((attr != null && attr.IsBigField)|| w.PropertyType == type)
                        {
                            var value = JsonHelper.JsonDeserialize((string)d, w.PropertyType);
                            w.SetValue(re, value, null);
                        }
                        else
                        {
                            setFieldValue(w, ref re, d);
                        }

                        //if(w.PropertyType == typeof(int?))
                        //{
                        //    w.SetValue(re, d, null);
                        //}
                        //else if (w.PropertyType == typeof(DateTime?))
                        //{
                        //    //var val = Convert.ChangeType(d, w.PropertyType);
                        //    w.SetValue(re, d, null);
                        //}
                        //else{
                        //    var val = Convert.ChangeType(d, w.PropertyType);
                        //    w.SetValue(re, val, null);
                        //}
                    }
                }
            });
//            if (maxDeep >= 0)
            if(rel!=null&&rel.RelationType == Relation.复杂关联)
            {
                var relConfig = rel.RelConfig;
//                var w = relConfig.DictTableName;
//                var relDictKey = relConfig.RelDictKey;
//                var obj = fc.FindObject(z => z.ObjectCode == type2.Name);
                //var subRel = obj.Relation.FirstOrDefault(z => z.ObjectPorertity == relDictKey);
                
//                obj.
                var p = type2.GetProperty(relConfig.DictTableName);
//                associationType.ForEach(
//                        w =>
//                        {
                            object subProp = null;
//                            if (w.PropertyType.IsGenericType) //如果属性是个List
//                            {
//                                var type = w.PropertyType;
//                                subProp = ReflectionHelper.GetObject(w.PropertyType);//.GetConstructor(new Type[] {}).Invoke(null));
//                                w.SetValue(re, subProp, null);
//                                var addMethod = type.GetMethod("Add");
//                                var subType = type.GetGenericArguments()[0];
//                                bool _ha;
//                                object subPropItem = GetEntity1(rel,subType, dr, true, out _ha, type2);
//                                if (_ha) addMethod.Invoke(subProp, new[] { subPropItem });
//                            }
//                            else //如果属性不是List
//                            {
                                //subProp = w.PropertyType.GetConstructor(null).Invoke(null);
                                bool _ha;
                                subProp = GetEntity1(fc,null, p.PropertyType, dr, true, out _ha, type2);
                                if (_ha) p.SetValue(re, subProp, null);
//                            }
//                        });
            }
            haveValue = _haveValue;
            return re;
        }
        private static void setFieldValue(PropertyInfo property,ref object obj, object value)
        {
            try
            {
                if (property.PropertyType == typeof (ushort) && value is short)
                {
                    var o = Convert.ChangeType(value, typeof (ushort));
                    property.SetValue(obj, o, null);
                }
                else
                {
                    property.SetValue(obj, value, null);
                }
            }
            catch (Exception e)
            {
                var m = e.Message;
                m = string.Format("{0}.{1};{2}", property.DeclaringType, property.Name,m);
                throw new Exception(m);
                
//                property.Name
            }
            return;
            var propertyType = property.PropertyType;
            if (value.GetType() != property.PropertyType)
            {
                if (propertyType.IsGenericType)
                {
                    var genericArgument = propertyType.GetGenericArguments()[0];
                    value = Convert.ChangeType(value, genericArgument);
                    property.SetValue(obj, value, null);
                }
                else
                {
                    value = Convert.ChangeType(value, propertyType);
                    property.SetValue(obj, value, null);
                }

            }
            else
            {
                property.SetValue(obj, value, null);
            }
        }
    }
}
