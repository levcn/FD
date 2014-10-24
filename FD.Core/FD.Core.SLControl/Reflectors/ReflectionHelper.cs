using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using StaffTrain.FwClass.Serializer;
using StaffTrain.FwClass.UserAttributes;


namespace StaffTrain.FwClass.Reflectors
{
    public static partial class ReflectionHelper
    {
        public static List<T> GetListTypeObject<T>(string jsonStr, string typeName)
        {
            Assembly asse = Assembly.GetCallingAssembly();
            var type = asse.GetTypes().FirstOrDefault(w => w.Name == typeName);
            var t = typeof(List<>).MakeGenericType(type);
            return (List<T>)JsonHelper.JsonDeserialize(t, jsonStr);
        }
        /// <summary>
        /// 返回一个空的List
        /// </summary>
        /// <param name="listObj"></param>
        /// <returns></returns>
        public static object GetEmptyList(object listObj)
        {
            var type123 = Type.GetTypeFromHandle(Type.GetTypeHandle(listObj));
            var type = listObj.GetType().GetGenericArguments()[0];
            return typeof (List<>).MakeGenericType(type);
        }
        public static string DefaultAssembly = "FD.Core.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        /// <summary>
        /// 返回指定类型的对象
        /// </summary>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        public static object GetObjectByName(string fullTypeName, Assembly asse = null)
        {
            var type = GetTypeByName(fullTypeName, asse);
            if(type!=null)
            {
                var dd = type.GetConstructor(new Type[]{});
                return dd.Invoke(null);
            }
            return null;
        }

        /// <summary>
        /// 返回指定类型的对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetObjectBySingleName(string name, Assembly asse = null)
        {
            var type = GetTypeBySingleName(name, asse);
            if(type!=null)
            {
                var dd = type.GetConstructor(new Type[]{});
                return dd.Invoke(null);
            }
            return null;
        }

        public static Type GetTypeByName(string fullTypeName, Assembly asse)
        {
            if (asse == null) asse = Assembly.Load(DefaultAssembly);
            var type = asse.GetTypes().FirstOrDefault(w => w.FullName.Equals(fullTypeName, StringComparison.OrdinalIgnoreCase));
            return type;
        }
        public static Type GetTypeBySingleName(string name, Assembly asse)
        {
            if (asse == null) asse = Assembly.Load(DefaultAssembly);
            var type = asse.GetTypes().FirstOrDefault(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return type;
        }
        /// <summary>
        /// 返回指定类型的对象
        /// </summary>
        /// <returns></returns>
        public static object GetObject(Type type)
        {
            if (type != null)
            {
                var dd = type.GetConstructor(new Type[] { });
                var t = dd.Invoke(null);
                return t;
            }
            return null;
        }
        public static Type GetType<T>()
        {
            return typeof (T);
        }

        /// <summary>
        /// 返回一个类的基本类型属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertyType">返回的属性类型 </param>
        public static List<PropertyInfo> GetBaseTypeProperty<T>(PropertyType propertyType)
        {
            var type = GetType<T>();
            return GetBaseTypeProperty(propertyType, type);
        }

        private static List<PropertyInfo> GetBaseTypeProperty(PropertyType propertyType, Type type)
        {
            var pro = type.GetProperties().ToList();
            var baseType = pro.Where(w =>
                                         {
                                             if (propertyType == PropertyType.BaseType)
                                             {
                                                 var isBaseType = (w.PropertyType.FullName.StartsWith("System.Nullable`1[[") || w.PropertyType.FullName.StartsWith("System.") && !w.PropertyType.IsGenericType);
                                                 return propertyType == PropertyType.BaseType ? isBaseType : !isBaseType;
                                             }
                                             else
                                             {
                                                 return (GetAttribute<LevcnAssociationAttribute>(w)) != null;
                                             }
                                         }).ToList();
            return baseType;
        }

        public static List<PropertyInfo> GetBaseTypeProperty(Type type, PropertyType propertyType)
        {
            return GetBaseTypeProperty(propertyType, type);
            ////var type = o.GetType();
            //var pro = type.GetProperties().ToList();
            //var baseType = pro.Where(w =>
            //{
            //    var isBaseType = w.PropertyType.FullName.StartsWith("System.") && !w.PropertyType.IsGenericType;
            //    return propertyType == PropertyType.BaseType ? isBaseType : !isBaseType;
            //}).ToList();
            //return baseType;
        }

        /// <summary>
        /// 返回一个类别的表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTableName<T>()
        {
            var t = typeof (T);
            return GetTableName(t);
        }

        /// <summary>
        /// 返回属性所对应类型的表名
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static string GetTableName(PropertyInfo pi)
        {
            return GetTableName(GetTypeByProperty(pi));
        }

        /// <summary>
        /// 返回一个属性所对类的类型是否是关联表的数据结构
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static bool IsRelationTable(PropertyInfo pi)
        {
            var type = GetTypeByProperty(pi);
            return type.GetProperties().FirstOrDefault(w => GetAttribute<LevcnAssociationAttribute>(w) != null) != null;
        }

        public static string GetTableName(Type t)
        {
            if (t.IsDefined(typeof (LevcnTableAttribute), false))
            {
                object[] attributes = t.GetCustomAttributes(typeof (LevcnTableAttribute), false);
                var studentAttr = (LevcnTableAttribute) attributes[0];
                if (!string.IsNullOrEmpty(studentAttr.Name)) return studentAttr.Name;
            }
            return t.Name;
        }

        /// <summary>
        /// 返回有指定TAttribute的属性列表
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertyInfoByCustomAttribute<TAttribute>(Type type) where TAttribute:Attribute
        {
            var t = typeof (TAttribute);
            return type.GetProperties().Where(w => w.IsDefined(t, false)).ToList();
        }
        /// <summary>
        /// 返回属性所对应的字段名
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetPropertyField(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) return null;
            if (propertyInfo.IsDefined(typeof (LevcnColumnAttribute), false))
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof (LevcnColumnAttribute), false);
                var studentAttr = (LevcnColumnAttribute) attributes[0];
                if (!string.IsNullOrEmpty(studentAttr.Name))return studentAttr.Name;
            }
            return propertyInfo.Name;
        }

        public static T GetAttribute<T>(PropertyInfo propertyInfo) where T : class
        {
            if (propertyInfo.IsDefined(typeof (T), false))
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof (T), false);
                return (T) attributes[0];
            }
            return null;
        }

        /// <summary>
        /// 返回属性的值
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object GetPropertyValue(PropertyInfo propertyInfo, object t)
        {
            return propertyInfo.GetValue(t, null);
        }
        ///// <summary>
        ///// 返回属性的值
        ///// </summary>
        ///// <param name="propertyInfo"></param>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public static object GetPropertyValue(string property, object t)
        //{
        //    var f = GetBaseTypeProperty(t.GetType(), PropertyType.BaseType).FirstOrDefault(w=>w.Name == property);
        //    if (f != null) return GetPropertyValue(f, t);
        //    return null;
        //}
        public static PropertyInfo GetPrimaryKey<T>()
        {
            return GetPrimaryKey(typeof (T));
        }
        public static string GetPrimaryName<T>()
        {
            return GetPrimaryKey(typeof(T)).Name;
        }
        /// <summary>
        /// 返回一个类型的主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static PropertyInfo GetPrimaryKey(Type type)
        {
            var prs = GetBaseTypeProperty(PropertyType.BaseType, type);
            return prs.FirstOrDefault(w =>
                                          {
                                              if (w.IsDefined(typeof (LevcnColumnAttribute), false))
                                              {
                                                  var attributes = w.GetCustomAttributes(typeof (LevcnColumnAttribute),
                                                                                         false);
                                                  var studentAttr = (LevcnColumnAttribute) attributes[0];
                                                  return studentAttr.IsPrimaryKey;
                                              }
                                              return false;
                                          });
        }

        /// <summary>
        /// 返回指定属性中,不是主键的属性列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileds"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetNonPrimaryFields(Type type, List<PropertyInfo> fileds)
        {
            var pi = GetPrimaryKey(type);
            var re = fileds.Where(w => w != pi).ToList();
            return re;
        }

        public static List<PropertyInfo> GetNonPrimaryFields<T>(List<PropertyInfo> fileds)
        {
            return GetNonPrimaryFields(typeof (T), fileds);
        }

        /// <summary>
        /// 返回多表的关联和字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static List<TableAndColumns> GetTableAndColumnsList<T>(ref List<TableRelation> tableNames)
        {
            var re = GetTableAndColumnsList(typeof (T), ref tableNames);
            return re;
        }

        /// <summary>
        /// 返回一个类型的表名及字段名
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tableNames"></param>
        /// <param name="pi">返回特定类型的表名和字段名</param>
        /// <param name="onlyPi">是否只返回特定的表名和字段名</param>
        /// <returns></returns>
        public static List<TableAndColumns> GetTableAndColumnsList(Type type, ref List<TableRelation> tableNames, PropertyInfo pi = null, bool onlyPi = false, int maxDeep = 2)
        {
            List<TableAndColumns> re = new List<TableAndColumns>();
            var baseType = GetBaseTypeProperty(PropertyType.BaseType, type);
            if (maxDeep == -1) return re;
            if (!onlyPi)
            {
                re.Add(new TableAndColumns
                {
                    Type = type,
                    TableName = GetTableName(type),
                    PropertyInfos = baseType
                });
            }
            var associationType = GetBaseTypeProperty(PropertyType.CustomType, type);
            foreach (var propertyInfo in associationType)
            {
                if (pi == null || pi.PropertyType == propertyInfo.PropertyType)
                {
                    Type tt = null;
                    bool isList = false;
                    if (propertyInfo.PropertyType.IsGenericType)
                    {
                        isList = true;
                        tt = propertyInfo.PropertyType.GetGenericArguments()[0];
                    }
                    else
                    {
                        isList = false;
                        tt = propertyInfo.PropertyType;
                    }
                    var deep = maxDeep - 1;
                    if (deep >= 0)
                    {
                        if (!isList) deep = 0;
                        var pRe = GetTableAndColumnsList(tt, ref tableNames, maxDeep: deep);
                        re = re.Concat(pRe).ToList();
                        TableRelation tr = new TableRelation();
                        tr.TableName1 = GetTableName(type);
                        tr.TableName2 = GetTableName(tt);
                        var assoc = GetAttribute<LevcnAssociationAttribute>(propertyInfo);
                        tr.Column1 = assoc.ThisKey;
                        tr.Column2 = assoc.OtherKey;
                        tableNames.Add(tr);
                    }
                }
            }

            return re;
        }

        /// <summary>
        /// 返回一个属性的类型
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static Type GetTypeByProperty(PropertyInfo propertyInfo)
        {
            Type tt;
            if (propertyInfo.PropertyType.IsGenericType)
            {
                tt = propertyInfo.PropertyType.GetGenericArguments()[0];
            }
            else
            {
                tt = propertyInfo.PropertyType;
            }
            return tt;
        }
        /// <summary>
        /// 返回类型是否是List
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static bool IsList(object com)
        {
            Type t = com.GetType();
            return t.IsGenericType;
        }

        /// <summary>
        /// 返回列表的元素个数
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static int GetListCount(object com)
        {
            Type t = com.GetType();
            var countMethod = t.GetMethod("get_Count");
            if (countMethod != null)
            {
                return (int) countMethod.Invoke(com, null);
            }
            return 0;
        }
        /// <summary>
        /// 返回列表的元素个数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static void AddToList(object list,object item)
        {
            Type t = list.GetType();
            var countMethod = t.GetMethod("Add");
            if (countMethod != null)
            {
                countMethod.Invoke(list, new [] { item });
            }
        }
        /// <summary>
        /// 返回列表的元素个数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool ListContainsItem(object list, object item)
        {
            Type t = list.GetType();
            var param = GetPrimaryKey(item.GetType());
            if (param != null)
            {
                var count = GetListCount(list);
                var itemPKValue = param.GetValue(item, null);
                for (int i = 0; i < count; i++)
                {
                    var current = GetListItem(list, i);
                    if (param.PropertyType == typeof (Guid))
                    {
                        if ((Guid)param.GetValue(current, null) == (Guid)itemPKValue)
                            return true;
                    }
                    else
                    {
                        throw new Exception("未设置主键比较,类型:" + param.PropertyType.FullName);
                        if (param.GetValue(current, null) == itemPKValue)
                            return true;
                    }
                }
                return false;
            }
            else
            {
                var countMethod = t.GetMethod("Contains");
                if (countMethod != null)
                {
                    return (bool) countMethod.Invoke(list, new[] {item});
                }
                return false;
            }
        }
        /// <summary>
        /// 返回列表的元素个数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static void RemoveFromList(object list, object item)
        {
            Type t = list.GetType();
            var countMethod = t.GetMethod("Remove");
            if (countMethod != null)
            {
                countMethod.Invoke(list, new [] { item });
            }
        }
        /// <summary>
        /// 返回一个列表中的第i个元素
        /// </summary>
        /// <param name="com"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static object GetListItem(object com, int i)
        {
            Type t = com.GetType();
            var getItemMethod = t.GetMethod("get_Item");
            if (getItemMethod != null)
            {
                return getItemMethod.Invoke(com, new object[] {i});
            }
            return null;
        }
        /// <summary>
        /// 删除List的所有数据
        /// </summary>
        /// <returns></returns>
        public static void ClearList(object com)
        {
            Type t = com.GetType();
            var getItemMethod = t.GetMethod("Clear");
            if (getItemMethod != null)
            {
                getItemMethod.Invoke(com,null);
            }
        }
        /// <summary>
        /// 返回一个类型的主键的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="item"></param>
        public static object GetPrimaryValue(object item)
        {
            var p = GetPrimaryKey(item.GetType());
            if(p!=null)return GetPropertyValue(p, item);
            return null;
        }
        /// <summary>
        /// 返回类型item中,指定属性的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(string name, object item)
        {
            return item.GetType().GetProperties().ToList().FirstOrDefault(w => w.Name == name);
        }
        

        /// <summary>
        /// 返回指定属性的Attribute
        /// </summary>
        /// <param name="dicPrimaryKey"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T GetPropertyAttribute<T>(string dicPrimaryKey, object item) where T : class
        {
            var a = GetProperty(dicPrimaryKey, item);
            if(a!=null)
            {
                return GetAttribute<T>(a);
            }
            return null;
        }
        /// <summary>
        /// 返回类型item中,指定属性的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static object GetPropertyValue(string name, object item)
        {
            var p = item.GetType().GetProperties().ToList().FirstOrDefault(w => w.Name == name);
            if (p != null)
            {
                return GetPropertyValue(p, item);
            }
            return null;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(string name, object item, object value)
        {
            var p = GetProperty(name, item);
            p.SetValue(item,value,null);
        }
        /// <summary>
        /// 返回指定的类型
        /// </summary>
        /// <param name="actionObjectName"></param>
        /// <returns></returns>
        public static Type GetTypeByFullName(string actionObjectName)
        {
            return Type.GetType(actionObjectName, false, true);
        }
        /// <summary>
        /// 返回类型的名称,或List T中T的名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetGenericType(Type type)
        {
            if (type.IsGenericType)
            {
                return type.GetGenericArguments()[0];
            }
            return null;
        }

        /// <summary>
        /// 返回类型的名称,或List T中T的名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeName(Type type)
        {
            if(type.IsGenericType)
            {
                return type.GetGenericArguments()[0].Name;
            }
            else
            {
                return type.Name;
            }
        }
        /// <summary>
        /// 把source的值给target
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static void SetAttributeValues(object target,object source)
        {
            if (target == null || source == null) return;
            Type t = target.GetType();
            var targetp = t.GetProperties().ToList();
            var sourcep = source.GetType().GetProperties();
            targetp.ForEach(
                w=>
                    {
                        var sp1 = sourcep.FirstOrDefault(q => q.Name == w.Name);
                        if (sp1 != null && sp1.CanWrite && sp1.PropertyType==w.PropertyType)
                        {
                            var value = sp1.GetValue(source, null);
                            w.SetValue(target,value,null);
                        }
                    });
        }

        /// <summary>
        /// 返回包括指定类型定义的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="whereFunc"> </param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertyByAttributeType<T>(Type type,Func<T,bool> whereFunc = null) where T : Attribute
        {
            return type.GetProperties().Where(w =>
                                                  {
                                                      if(whereFunc!=null)
                                                      {
                                                          var a = GetAttribute<T>(w);
                                                          return a != null && whereFunc(a);
                                                      }
                                                      else
                                                      {
                                                          return GetAttribute<T>(w) != null;
                                                      }
                                                  }).ToList();
        }
        /// <summary>
        /// 返回对象的属性中,有T定义Attribute的值
        /// </summary>
        /// <param name="obj"></param>
        public static object GetObjectPropValueByAttr<T>(object obj,Func<T,bool> check) where T : Attribute
        {
            var flagProp = GetPropertyByAttributeType<T>(obj.GetType()).FirstOrDefault(w => check(GetAttribute<T>(w)));
            if(flagProp!=null)
            {
                return flagProp.GetValue(obj, null);
            }
            return null;
        }



        public static string GetValuePath(Type type)
        {
            var p = GetPropertyByAttributeType<LevcnColumnAttribute>(type, w => w.IsFlag).FirstOrDefault();
            if (p == null) throw new Exception(string.Format("{0}对象中未定义IsFlag属性", type.FullName));
            return p.Name;
        }
        public static string GetDispalyPath(Type type)
        {
            var p = GetPropertyByAttributeType<LevcnColumnAttribute>(type, w => w.IsDisplayName).FirstOrDefault();
            if (p == null) throw new Exception(string.Format("{0}对象中未定义DispalyPath属性", type.FullName));
            return p.Name;
        }

        /// <summary>
        /// 把List添加到source中
        /// </summary>
        /// <param name="source"></param>
        /// <param name="list"></param>
        public static void ListAddRange(object source, object list)
        {
            int count = GetListCount(list);
            for (int i = 0; i < count; i++)
            {
                var obj = GetListItem(list, i);
                if (!ListContainsItem(source, obj))
                {
                    AddToList(source, obj);
                }
            }
        }
        public static object GetValue(object o,string path)
        {
            var p = GetProperty(path, o);
            if (p != null)
            {
                return GetPropertyValue(p, o);
            }
            return null;
        }
        public static object GetItemByValue(IEnumerable list, object value)
        {
//            var list = itemCollection.ToList();
            string path = null;
            if (list != null)
                foreach (var o in list)
                {
                
                    if (path == null)
                    {
                        path = GetValuePath(o.GetType());
                    }
                    var v = GetValue(o, path);
                    if (value.Equals(v))
                    {
                        return o;
                    }
                }
            return null;
        }

        public static async Task<object> AwaitExecute(Type type,Task task)
        {
            await task;
            var ree = task.GetType().GetProperty("Result").GetValue(task, null);
            return ree;
        }
        public static async Task<T> AwaitExecute<T>(Task task)
        {
                        await task;
                        var ree = task.GetType().GetProperty("Result").GetValue(task, null);
            var re = (T)ree;
            return re;
        }
    }
}
