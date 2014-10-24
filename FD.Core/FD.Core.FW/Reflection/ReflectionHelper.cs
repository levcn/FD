using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Fw.ActionMethod;
using Fw.Serializer;
using Fw.UserAttributes;
using STComponse.GCode;
using ServerFw.Reflection;
using STComponse.CFG;


namespace Fw.Reflection
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// 返回一个空的List
        /// </summary>
        /// <param name="listObj"></param>
        /// <returns></returns>
        public static object GetEmptyList(object listObj)
        {
            var type1 = listObj.GetType();
            return RCache<Type, object>.GetValue(type1, "GetEmptyListMakeGenericType", () =>
            {
                var type = type1.GetGenericArguments()[0];
                return typeof (List<>).MakeGenericType(type);
            });
        }
        /// <summary>
        /// 默认反射类型的程序集
        /// </summary>
        public static string DefaultAssemblyName = "StaffTrain.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        public static List<string> DefaultAssemblyNames = new List<string>();
        public static string DefaultAssemblyNamePrefix;

        /// <summary>
        /// 添加默认反射程序集
        /// </summary>
        /// <param name="name"></param>
        public static void AddAssemblyName(string name)
        {
            lock (DefaultAssemblyNames)
            {
                if (DefaultAssemblyNames.ToList().All(w => w != name))
                {
                    DefaultAssemblyNames.Add(name);
                }
            }
        }
        /// <summary>
        /// 执行指定的方法,并返回结果
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="parames"></param>
        /// <param name="paramTypes"></param>
        /// <returns></returns>
        public static object ExecuteMethod(string className, string methodName, List<string> parames)
        {
            return ExecuteMethod(DefaultAssemblyNames, className, methodName, parames);
        }
        public static object ExecuteMethod(string className, string methodName, List<object> parames)
        {
            return ExecuteMethod(DefaultAssemblyNames, className, methodName, parames);
        }

        /// <summary>
        /// 执行指定的方法
        /// </summary>
        /// <param name="assemblyName">程序集</param>
        /// <param name="className">完整类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="parames">参数</param>
        /// <param name="isXml">参数是否是xml(要么是json)</param>
        /// <returns></returns>
        public static object ExecuteMethod(List<string> assemblyName, string className, string methodName, List<string> parames, bool isXml = true)
        {
            
            Type type = GetTypeByFullName(assemblyName, className);
            return ExecuteMethod(type, methodName, parames, isXml);
        }
        public static object ExecuteClientMethod(string className, string methodName, List<string> parames)
        {
            if (DefaultAssemblyNames.Count == 0) throw new Exception("未设置默认应用程序集.(ReflectionHelper.AddAssemblyName)");
            return ExecuteClientMethod(DefaultAssemblyNames, className, methodName, parames);
        }
        public static object ExecuteClientMethod(List<string> assemblyName, string className, string methodName, List<string> parames, bool isXml = true)
        {
//            className += "1";
            Type type = GetTypeByTypeAndName<BaseController>(assemblyName, className);
            if(type==null)throw new Exception("未找到指定的类:"+className);
            return ExecuteMethod(type, methodName, parames, isXml);
        }
        public static object ExecuteMethod(Type type, string methodName, List<string> parames, bool isXml = true)
        {
            var key = string.Concat(type.FullName, methodName);
            MethodInfo method;
            method = RCache<string, MethodInfo>.GetValue(key, () =>
            {
                var methods = RCache<Type, MethodInfo[]>.GetValue(type, type.GetMethods);
                //查找定义方法
                method = methods.Select(w => new { method = w, attr = GetAttribute<LevcnMethodAttribute>(w) })
                    .Where(w => w.attr != null && w.attr.Name == methodName)
                    .Select(w => w.method)
                    .FirstOrDefault();
                if (method == null) method = methods.FirstOrDefault(w => w.Name == methodName);
                if (method == null) throw new NullReferenceException(string.Format("在类{0}中,找不到名为{1}的方法", type.FullName, methodName));
                return method;
            });
            ParameterInfo[] parameterInfos = RCache<MethodInfo, ParameterInfo[]>.GetValue(method, method.GetParameters);
            List<object> objs;
            if (isXml) 
                objs = GetObjectByXml(parameterInfos, parames);
            else
                objs = GetObjectByJson(parameterInfos.Select(w => w.ParameterType).ToList(), parames);
            
            ActionMethodDispatcher d = new ActionMethodDispatcher(method);
            if (parameterInfos.Count() != parames.Count) throw new Exception("要执行方法的参数和传递的参数数量不相同");
            return d.Execute(GetObject(method.DeclaringType) as IController, objs.ToArray());
        }
        /// <summary>
        /// 执行指定的方法,并返回结果
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="parames"></param>
        /// <param name="paramTypes"></param>
        /// <returns></returns>
        public static object ExecuteMethod(List<string> assemblyName, string className, string methodName, List<object> parames)
        {
            var key = string.Concat(className, methodName);
            MethodInfo method;
            method = RCache<string, MethodInfo>.GetValue(key, () =>
            {
                Type type = GetTypeByFullName(assemblyName, className);
                var infos = type.GetMethods();
                var methods = RCache<Type, MethodInfo[]>.GetValue(type, () => infos);
                //查找定义方法
                method = methods.Select(w => new { method = w, attr = GetAttribute<LevcnMethodAttribute>(w) })
                    .Where(w => w.attr != null && w.attr.Name == methodName)
                    .Select(w => w.method)
                    .FirstOrDefault();
                if (method == null) method = methods.FirstOrDefault(w => w.Name == methodName);
                if (method == null) throw new NullReferenceException(string.Format("在类{0}中,找不到名为{1}的方法", className, methodName));
                return method;
            });
            ActionMethodDispatcher d = new ActionMethodDispatcher(method);
            var parameterInfos = RCache<MethodInfo, ParameterInfo[]>.GetValue(method, method.GetParameters); ;
            if (parameterInfos.Count() != parames.Count) throw new Exception("要执行方法的参数和传递的参数数量不相同");
            return d.Execute(GetObject(method.DeclaringType) as IController, parames.ToArray());
        }

        /// <summary>
        /// 按类型名,把JSON对象转成指定的类型
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <param name="parames"></param>
        /// <returns></returns>
        private static List<object> GetObjectByXml(IEnumerable<ParameterInfo> parameterInfo, List<string> parames)
        {
            return GetObjectByXml(parameterInfo.Select(w => w.ParameterType).ToList(), parames);
        }

        /// <summary>
        /// 按类型名,把JSON对象转成指定的类型
        /// </summary>
        /// <param name="types"></param>
        /// <param name="parames"></param>
        /// <returns></returns>
        private static List<object> GetObjectByXml(IList<Type> types, IList<string> parames)
        {
            if (parames == null) parames = new List<string>();
            List<object> re = new List<object>();
            for (int i = 0; i < parames.Count; i++)
            {
                re.Add(XmlHelper.GetXmlDeserialize(parames[i], types[i]));
            }
            return re;
        }

        /// <summary>
        /// 按类型名,把JSON对象转成指定的类型
        /// </summary>
        /// <param name="types"></param>
        /// <param name="parames"></param>
        /// <returns></returns>
        private static List<object> GetObjectByJson(IList<Type> types, IList<string> parames)
        {
            if (parames == null) parames = new List<string>();
            var re = new List<object>();
            for (int i = 0; i < parames.Count; i++)
            {
                re.Add(JsonHelper.FastJsonDeserialize(parames[i], types[i]));
            }
            return re;
        }

        public static Type GetType<T>()
        {
            return typeof(T);
        }

        /// <summary>
        /// 返回一个类的基本类型属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyType">返回的属性类型 </param>
        public static List<PropertyInfo> GetBaseTypeProperty<T>(PropertyType propertyType)
        {
            var type = GetType<T>();
            return GetBaseTypeProperty(propertyType, type);
        }

        private static List<PropertyInfo> GetBaseTypeProperty(PropertyType propertyType, Type type, bool includeIgnoreField = true)
        {
            PropertyInfo[] infos;
            infos = RCache<Type, PropertyInfo[]>.GetValue(type, "GetBaseTypePropertyincludeIgnoreField" + propertyType + includeIgnoreField, () =>
            {
                var pro = RCache<Type, PropertyInfo[]>.GetValue(type, type.GetProperties).ToList();

                return pro.Where(w =>
                {
                    if (propertyType == PropertyType.BaseType)
                    {
                        var column = (GetAttribute<LevcnColumnAttribute>(w));
                        var isBaseType =
                                (w.PropertyType.FullName.StartsWith("System.Nullable`1[[")
                                 || w.PropertyType == typeof(BigFieldValue)
                                 || (column != null && column.IsBigField)
                                 || (w.PropertyType.FullName.StartsWith("System.") && !w.PropertyType.IsGenericType));
                        if (column != null && column.Ignore && includeIgnoreField) return false;
                        return propertyType == PropertyType.BaseType ? isBaseType : !isBaseType;
                    }
                    else
                    {
                        return (GetAttribute<LevcnAssociationAttribute>(w)) != null;
                    }
                }).ToArray();
            });

            return infos.ToList();
        }

        public static List<PropertyInfo> GetBaseTypeProperty(Type type, PropertyType propertyType, bool includeIgnoreField = true)
        {
            return GetBaseTypeProperty(propertyType, type, includeIgnoreField);
        }
        /// <summary>
        /// 返回一个类别的表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTableName<T>()
        {
            var t = typeof(T);
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
            return RCache<PropertyInfo, bool>.GetValue(pi, "IsRelationTable", () =>
            {
                var type = GetTypeByProperty(pi);
                return RCache<Type, PropertyInfo[]>.GetValue(type, type.GetProperties)
                    .FirstOrDefault(w => GetAttribute<LevcnAssociationAttribute>(w) != null) != null;
            });
            
        }
        /// <summary>
        /// 返回一个属性所对类的类型是否是关联表的数据结构,并且是实时保存关系的
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static bool IsRelationTableAndSaveRelation(PropertyInfo pi)
        {
            var o = GetAttribute<LevcnAssociationAttribute>(pi);
            return o != null && o.SaveRelation;
        }
        /// <summary>
        /// 返回表名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTableName(Type type)
        {
            return RCache<Type, string>.GetValue(type, "GetTableName", () =>
            {
                if (type.IsDefined(typeof(LevcnTableAttribute), false))
                {
                    object[] attributes = type.GetCustomAttributes(typeof(LevcnTableAttribute), false);
                    var studentAttr = (LevcnTableAttribute)attributes[0];
                    if (!string.IsNullOrEmpty(studentAttr.Name)) return studentAttr.Name;
                }
                return type.Name;
            });
            
        }
        /// <summary>
        /// 返回一个类型的主键字段名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetPrimaryPropertyField(Type type)
        {
            var primaryKey = GetPrimaryKey(type);
            if (primaryKey==null)throw new Exception(string.Format("类型 {0} 没有主键",type.ToString()));
            return GetPropertyField(primaryKey);
        }

        /// <summary>
        /// 返回属性所对应的字段名
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetPropertyField(PropertyInfo propertyInfo)
        {
            return RCache<PropertyInfo, string>.GetValue(propertyInfo, "GetPropertyField", () =>
            {
                if (propertyInfo.IsDefined(typeof(LevcnColumnAttribute), false))
                {
                    var attributes = propertyInfo.GetCustomAttributes(typeof(LevcnColumnAttribute), false);
                    var studentAttr = (LevcnColumnAttribute)attributes[0];
                    if (!string.IsNullOrEmpty(studentAttr.Name)) return studentAttr.Name;
                }
                return propertyInfo.Name;
            });
        }
        public static T GetAttribute<T>(MethodInfo method) where T : class
        {
            var objs = RCache<MethodInfo, object[]>.GetValue(method, () => method.GetCustomAttributes(false));
            return objs.OfType<T>().FirstOrDefault();
        }
        public static T GetAttribute<T>(PropertyInfo propertyInfo) where T : class
        {
            var objs = RCache<PropertyInfo, object[]>.GetValue(propertyInfo, () => propertyInfo.GetCustomAttributes(false));
            return objs.OfType<T>().FirstOrDefault();
        }
        /// <summary>
        /// 返回属性的值
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        internal static object GetPropertyValue(PropertyInfo propertyInfo, object t)
        {

            var re = propertyInfo.GetValue(t, null);
            if (re == null)
            {
                re = DBNull.Value;
            }
            if (propertyInfo.PropertyType == typeof(DateTime))//|| propertyInfo.PropertyType == typeof(DateTime?))
            {
                var time = Convert.ToDateTime(re);
                if (time.Year < 1800)
                {
                    time = new DateTime(1900, 1, 1);
                    re = time;
                }
            }
            else if (propertyInfo.PropertyType == typeof(DateTime?))
            {
                if (re != DBNull.Value)
                {
                    var time = Convert.ToDateTime(re);
                    if (time.Year < 1800)
                    {
                        re = DBNull.Value;
                    }
                }
            }
            return re;
        }
        public static PropertyInfo GetPrimaryKey<T>()
        {
            return GetPrimaryKey(typeof(T));
        }
        /// <summary>
        /// 返回一个类型的主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static PropertyInfo GetPrimaryKey(Type type)
        {
            return RCache<Type, PropertyInfo>.GetValue(
                    type,
                    "PrimaryKey",
                    () =>{
                        var prs = GetBaseTypeProperty(PropertyType.BaseType, type);
                        return prs.FirstOrDefault(w =>
                        {
                            var attribute = GetAttribute<LevcnColumnAttribute>(w);
                            return attribute != null && attribute.IsPrimaryKey;
                        });
                    });
        }
        /// <summary>
        /// 返回指定属性中,不是主键的属性列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileds"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetNonPrimaryFields(Type type, List<PropertyInfo> fileds)
        {
            var pi = GetPrimaryKey(type);
            var re = fileds.Where(w => w != pi).ToList();
            return re;
        }
        public static IEnumerable<PropertyInfo> GetNonPrimaryFields<T>(List<PropertyInfo> fileds)
        {
            return GetNonPrimaryFields(typeof(T), fileds);
        }
        /// <summary>
        /// 返回指定属性中,不是主键的属性列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileds"></param>
        /// <returns></returns>
        public static IEnumerable<Property> GetNonPrimaryFields(EDataObject type, List<Property> fileds)
        {
            var pi = type.GetPrimary();
            var re = fileds.Where(w => w != pi).ToList();
            return re;
        }
        /// <summary>
        /// 返回多表的关联和字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static List<TableAndColumns> GetTableAndColumnsList<T>(ref List<TableRelation> tableNames)
        {
            var re = GetTableAndColumnsList(typeof(T), ref tableNames);
            return re;
        }
        /// <summary>
        /// 从input中删除filter列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetBlack(List<PropertyInfo> input, string[] filter)
        {
            if (filter == null || filter.Length == 0) return input;

            foreach (string s in filter)
            {
                var p = input.FirstOrDefault(w => w.Name.Equals(s, StringComparison.OrdinalIgnoreCase));
                if (p != null) input.Remove(p);
            }
            return input;
        }
        /// <summary>
        /// 从input中删除filter列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetWhite(List<PropertyInfo> input, string[] filter)
        {
            if (filter == null || filter.Length == 0) return input;
            List<PropertyInfo> re = new List<PropertyInfo>();
            foreach (string s in filter)
            {
                var p = input.FirstOrDefault(w => w.Name.Equals(s, StringComparison.OrdinalIgnoreCase));
                if (p != null) re.Add(p);
            }
            return re;
        }
        /// <summary>
        /// 从input中删除filter列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<Property> GetBlack(List<Property> input, string[] filter)
        {
            if (filter == null || filter.Length == 0) return input;

            foreach (string s in filter)
            {
                var p = input.FirstOrDefault(w => w.Code.Equals(s, StringComparison.OrdinalIgnoreCase));
                if (p != null) input.Remove(p);
            }
            return input;
        }
        /// <summary>
        /// 从input中删除filter列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<Property> GetWhite(List<Property> input, string[] filter)
        {
            if (filter == null || filter.Length == 0) return input;
            List<Property> re = new List<Property>();
            foreach (string s in filter)
            {
                var p = input.FirstOrDefault(w => w.Code.Equals(s, StringComparison.OrdinalIgnoreCase));
                if (p != null) re.Add(p);
            }
            return re;
        }
        /// <summary>
        /// 从input中删除filter列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<Relation> GetBlack(List<Relation> input, string[] filter)
        {
            if (filter == null || filter.Length == 0) return input;

            foreach (string s in filter)
            {
                var p = input.FirstOrDefault(w => w.Code.Equals(s, StringComparison.OrdinalIgnoreCase));
                if (p != null) input.Remove(p);
            }
            return input;
        }
        /// <summary>
        /// 从input中删除filter列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<Relation> GetWhite(List<Relation> input, string[] filter)
        {
            if (filter == null || filter.Length == 0) return input;
            List<Relation> re = new List<Relation>();
            foreach (string s in filter)
            {
                var p = input.FirstOrDefault(w => w.Code.Equals(s, StringComparison.OrdinalIgnoreCase));
                if (p != null) re.Add(p);
            }
            return re;
        }
        /// <summary>
        /// 返回一个类型的表名及字段名(集合的第一个元素是当前表,其它元素是表中的字典表)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tableNames"></param>
        /// <param name="pi">返回特定类型的表名和字段名</param>
        /// <param name="onlyPi">是否只返回特定的表名和字段名</param>
        /// <param name="association">上级属性的关联类型</param>
        /// <param name="parentType">上级类型</param>
        /// <returns></returns>
        public static List<TableAndColumns> GetTableAndColumnsList(Type type, ref List<TableRelation> tableNames, PropertyInfo pi = null, bool onlyPi = false, int maxDeep = 2, LevcnAssociationAttribute association = null, Type parentType = null,string[] white = null, string[] black = null)
        {
            List<TableAndColumns> re = new List<TableAndColumns>();
            var baseType = GetBaseTypeProperty(PropertyType.BaseType, type);
            if (parentType == null)
            {
                baseType = GetBlack(baseType, black);
                baseType = GetWhite(baseType, white);
                var p = GetPrimaryKey(type);
                if(!baseType.Contains(p))baseType.Add(p);
            }
            
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
            var customTypeProperty = GetBaseTypeProperty(PropertyType.CustomType, type);
            if (parentType == null)
            {
                customTypeProperty = GetBlack(customTypeProperty, black);
                customTypeProperty = GetWhite(customTypeProperty, white);
            }
            var associationType = customTypeProperty
                .Where(w => GetAttribute<LevcnAssociationAttribute>(w) != null && GetTypeByProperty(w) != parentType);
            foreach (var propertyInfo in associationType)
            {
                var currentAssociation = GetAttribute<LevcnAssociationAttribute>(propertyInfo);
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
                        tt = propertyInfo.PropertyType;
                    }
                    var deep = maxDeep - 1;
                    if (deep >= 0)
                    {
                        if (!isList || currentAssociation.Relation != RelationType.Multi) deep = 0;
                        var pRe = GetTableAndColumnsList(tt, ref tableNames, maxDeep: deep, association: currentAssociation, parentType: parentType);
                        re = re.Concat(pRe).ToList();
                        TableRelation tr = new TableRelation();
                        tr.TableName1 = GetTableName(type);
                        tr.TableName2 = GetTableName(tt);
                        var assoc = GetAttribute<LevcnAssociationAttribute>(propertyInfo);
                        tr.Column1 = assoc.ThisKey;
                        tr.Column2 = assoc.OtherKey;
                        tr.LeftJoin = assoc.LeftJoin;
                        tableNames.Add(tr);
                    }
                }
            }
            return re;
        }
        public static List<TableAndColumnsFC> GetTableAndColumnsList(EDataObject type,FwConfig fc, ref List<TableRelation> tableNames,Relation pi = null, bool onlyPi = false, int maxDeep = 2, LevcnAssociationAttribute association = null, Type parentType = null, string[] white = null, string[] black = null)
        {
            var objs = fc.DataObjects.ToList();
            objs.AddRange(fc.DictObject);
            List<TableAndColumnsFC> re = new List<TableAndColumnsFC>();
            var baseType = type.Property.Where(w => w.ColumnType != Property.关联).ToList();
            if (parentType == null)
            {
                baseType = GetBlack(baseType, black);
                baseType = GetWhite(baseType, white);
                var p = type.GetPrimary();
                if (!baseType.Contains(p)) baseType.Add(p);
            }

            if (maxDeep == -1) return re;
            if (!onlyPi)
            {
                re.Add(new TableAndColumnsFC
                {
                    Type = type,
                    TableName = type.KeyTableName,
                    PropertyInfos = new ObservableCollection<Property>(baseType),
                });
            }
            var customTypeProperty = type.Relation.ToList();//.Where(w=>w.RelationType == Relation.字典).ToList();
            if (parentType == null)
            {
                customTypeProperty = GetBlack(customTypeProperty, black);
                customTypeProperty = GetWhite(customTypeProperty, white);
            }
            var associationType = customTypeProperty;
//                .Where(w => GetAttribute<LevcnAssociationAttribute>(w) != null && GetTypeByProperty(w) != parentType);
            foreach (var propertyInfo in associationType)
            {
//                var currentAssociation = GetAttribute<LevcnAssociationAttribute>(propertyInfo);
                if (pi == null || pi.ObjectPorertity == propertyInfo.ObjectPorertity)
                {
                    string tt = null;
//                    if (propertyInfo.PropertyType.IsGenericType)
                    {
                        var relConfig = propertyInfo.RelConfig;
                        if (propertyInfo.RelationType == Relation.字典)
                        {
                            TableRelation tr = new TableRelation();
                            tr.TableName1 = type.KeyTableName;
                            tr.TableName2 = relConfig.DictTableName;
                            tr.Column1 = propertyInfo.ObjectPorertity;
                            tr.Column2 = relConfig.DictKey;
//                            tr.LeftJoin = assoc.LeftJoin;
                            tableNames.Add(tr);
                            var o = objs.FirstOrDefault(z => z.ObjectCode == relConfig.DictTableName);
                            re.Add(new TableAndColumnsFC {
                                TableName = relConfig.DictTableName,
                                Type = o,
                                PropertyInfos = new ObservableCollection<Property>(o.GetBaseProperties())
                            });
                        }
                        else
                        {
                            tt = relConfig.RelDictKey;

                            //关联表,
                            TableRelation tr = new TableRelation();
                            tr.TableName1 = type.KeyTableName;
                            tr.TableName2 = relConfig.RelTableName;
                            tr.Column1 = type.GetID().Code;
                            tr.Column2 = relConfig.RelMasertKey;
                            tableNames.Add(tr);

                            //字典表
                            tr = new TableRelation();
                            tr.TableName1 = relConfig.RelTableName;
                            tr.TableName2 = relConfig.DictTableName;
                            tr.Column1 = relConfig.RelDictKey;
                            tr.Column2 = relConfig.DictKey;
                            tableNames.Add(tr);
                            if (propertyInfo.RelationType == Relation.简单关联)
                            {
                                //                                var o = objs.FirstOrDefault(z => z.ObjectCode == propertyInfo.RelConfig.DictTableName);
                                var dataObject = propertyInfo.GetSimpleRelDataObject();
                                re.Add(new TableAndColumnsFC {
                                    TableName = relConfig.RelTableName,
                                    Type = dataObject,
                                    PropertyInfos = new ObservableCollection<Property>(dataObject.GetBaseProperties())
                                });
                            }
                            else
                            {
                                var dataObject = fc.FindObject(z => z.ObjectCode == relConfig.RelTableName);
//                                var dataObject = objs.FirstOrDefault(z => z.ObjectCode == relConfig.RelTableName);
                                re.Add(new TableAndColumnsFC
                                {
                                    TableName = relConfig.RelTableName,
                                    Type = dataObject,
                                    PropertyInfos = new ObservableCollection<Property>(dataObject.GetBaseProperties())
                                });
                            }
                            //字典表
                            var dataObject1 = objs.FirstOrDefault(z => z.ObjectCode == relConfig.DictTableName);
                                re.Add(new TableAndColumnsFC
                                {
                                    TableName = relConfig.DictTableName,
                                    Type = dataObject1,
                                    PropertyInfos = new ObservableCollection<Property>(dataObject1.GetBaseProperties())
                                });

                        }
                        
//                        tt = propertyInfo.PropertyType.GetGenericArguments()[0];
                    }
                    //                    else
//                    {
//                        tt = propertyInfo.PropertyType;
//                    }
//                    var deep = maxDeep - 1;
//                    if (deep >= 0)
//                    {
//                        if (!isList || currentAssociation.Relation != RelationType.Multi) deep = 0;
//                        var pRe = GetTableAndColumnsList(tt, ref tableNames, maxDeep: deep, association: currentAssociation, parentType: parentType);
//                        re = re.Concat(pRe).ToList();
//                        
//                    }
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
                tt = RCache<PropertyInfo, Type>.GetValue(propertyInfo, "GetTypeByProperty", () =>
                {
                    return propertyInfo.PropertyType.GetGenericArguments()[0];
                });
                
            }
            else
            {
                tt = propertyInfo.PropertyType;
            }
            return tt;
        }
        /// <summary>
        /// 返回列表的元素个数
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        [Obsolete("转成IList")]
        public static int GetListCount(object com)
        {
            Type t = com.GetType();
            var methodInfos = RCache<Type, MethodInfo[]>.GetValue(t, t.GetMethods);
            var countMethod = methodInfos.FirstOrDefault(w => w.Name == "get_Count");
            if (countMethod != null)
            {
//                return (int)GetFastExecMethod(countMethod, com, null);
                return (int)countMethod.Invoke(com, null);
            }
            return 0;
        }

        /// <summary>
        /// 返回一个列表中的第i个元素
        /// </summary>
        /// <param name="com"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        [Obsolete("转成IList")]
        internal static object GetListItem(object com, int i)
        {
            Type t = com.GetType();
//            var getItemMethod = t.GetMethod("get_Item");
            var methodInfos = RCache<Type, MethodInfo[]>.GetValue(t, t.GetMethods);
            var getItemMethod = methodInfos.FirstOrDefault(w => w.Name == "get_Item");
            if (getItemMethod != null)
            {
//                return GetFastExecMethod(getItemMethod, com, new object[] { i });
                return getItemMethod.Invoke(com, new object[] { i });
            }
            return null;
        }

        /// <summary>
        /// 返回一个类型的主键的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="item"></param>
        internal static object GetPrimaryValue(object item)
        {
            var p = GetPrimaryKey(item.GetType());
            return GetPropertyValue(p, item);
        }
        /// <summary>
        /// 返回类型item中,指定属性的值
        /// </summary>
        /// <param name="dicPrimaryKey"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        internal static object GetPropertyValue(string dicPrimaryKey, object item)
        {
            var type = item.GetType();

            var p = RCache<Type, PropertyInfo[]>.GetValue(type, type.GetProperties).ToList().FirstOrDefault(w => w.Name == dicPrimaryKey);
            if (p != null)
            {
                return GetPropertyValue(p, item);
            }
            return null;
        }

        /// <summary>
        /// 返回指定的类型
        /// </summary>
        /// <param name="actionObjectName"></param>
        /// <returns></returns>
        public static Type GetTypeByFullName(string actionObjectName)
        {
            return RCache<string, Type>.GetValue(actionObjectName, () => GetTypeByFullName(DefaultAssemblyNames, actionObjectName));
        }

        public static Type GetTypeByFullName(List<string> assemblyName, string actionObjectName)
        {
            foreach (string c in assemblyName)
            {
                var re = GetTypeByFullName(c, actionObjectName);
                if (re != null) return re;
            }
            return null;
        }
        public static Type GetTypeByFullName(string assemblyName, string actionObjectName)
        {
            Assembly asse = Assembly.Load(assemblyName);
            var re = asse.GetTypes().FirstOrDefault(w => w.FullName.Equals(actionObjectName,StringComparison.InvariantCultureIgnoreCase));
            if (re == null)
            {
                re = Type.GetType(actionObjectName);
            }
            return re;
        }

        /// <summary>
        /// 返回指定程序集里的类,
        /// </summary>
        /// <typeparam name="T">返回类必须继承T</typeparam>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="className">仅类名</param>
        /// <returns></returns>
        public static Type GetTypeByTypeAndName<T>(List<string> assemblyName, string className)
        {
            foreach (string s in assemblyName)
            {
                Assembly asse = Assembly.Load(s);
                var baseType = typeof(T);
                var re = asse.GetTypes().FirstOrDefault(w => w.IsSubclassOf(baseType) && w.Name.Equals(className, StringComparison.InvariantCultureIgnoreCase));
                if (re != null)
                {
                    return re;
                }
            }
            
//            if (re == null)
//            {
//                re = Type.GetType(actionObjectName);
//            }
            return null;
        }
        /// <summary>
        /// 按属性名称把source的值给target
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static void SetAttributeValues<T, T1>(T target, T1 source)
        {
            var type = typeof (T);
            var type1 = typeof(T1);

            var targetp = RCache<Type, PropertyInfo[]>.GetValue(type, type.GetProperties).ToList();
            var sourcep = RCache<Type, PropertyInfo[]>.GetValue(type1, type1.GetProperties);

            targetp.ForEach(
                w =>
                {
                    var sp1 = sourcep.FirstOrDefault(q => q.Name == w.Name);
                    if (sp1 != null && sp1.CanWrite && sp1.PropertyType == w.PropertyType)
                    {
                        var value = sp1.GetValue(source, null);
                        w.SetValue(target, value, null);
                    }
                });
        }

        /// <summary>
        /// 返回包括指定类型定义的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertyByAttributeType<T>(Type type) where T : Attribute
        {
            return RCache<Type, PropertyInfo[]>.GetValue(type, type.GetProperties).Where(w => GetAttribute<T>(w) != null).ToList();
        }
        /// <summary>
        /// 返回包括指定类型定义的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="whereFunc"> </param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertyByAttributeType<T>(Type type, Func<T, bool> whereFunc = null) where T : Attribute
        {
            return type.GetProperties().Where(w =>
            {
                if (whereFunc != null)
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
        /// 返回列表对象指定列的值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="getPropValue"></param>
        /// <returns></returns>
        public static List<object> GetListItemValues(object data, Func<object, object> getPropValue)
        {
            return GetListItems(data).Select(getPropValue).ToList();
        }
        /// <summary>
        /// 把一个List对象,转成List对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<object> GetListItems(object data)
        {
            return (data as IList).OfType<object>().ToList();
        }
        public static object GetObject(Type type)
        {
            if (type == typeof(string))
            {
                return "";
            }
            if (type == typeof(int))
            {
                return 0;
            }

            var constructor = type.GetConstructor(new Type[] { });
            if (constructor == null)
                throw new Exception(string.Format("类型 {0} 没有默认构造函数", type.FullName));
            return constructor.Invoke(null);
        }
        public delegate object FastInvokeHandler(object target, object[] paramters);

        /// <summary>
        /// 快速执行一个方法
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="target"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public static object GetFastExecMethod(MethodInfo methodInfo, object target, object[] paramters)
        {
            var fi = RCache<MethodInfo, FastInvokeHandler>.GetValue(methodInfo, () => GetMethodInvoker(methodInfo));
            return fi(target, paramters);
        }

        /// <summary>
        /// 返回一个执行方法的代理
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new [] { typeof(object), typeof(object[]) }, methodInfo.DeclaringType.Module);
            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();
            Type[] paramTypes = new Type[ps.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = ps[i].ParameterType;
            }
            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];

            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitFastInt(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(il, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }
            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    il.Emit(OpCodes.Ldloca_S, locals[i]);
                else
                    il.Emit(OpCodes.Ldloc, locals[i]);
            }
            if (methodInfo.IsStatic)
                il.EmitCall(OpCodes.Call, methodInfo, null);
            else
                il.EmitCall(OpCodes.Callvirt, methodInfo, null);
            if (methodInfo.ReturnType == typeof(void))
                il.Emit(OpCodes.Ldnull);
            else
                EmitBoxIfNeeded(il, methodInfo.ReturnType);

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(il, i);
                    il.Emit(OpCodes.Ldloc, locals[i]);
                    if (locals[i].LocalType.IsValueType)
                        il.Emit(OpCodes.Box, locals[i].LocalType);
                    il.Emit(OpCodes.Stelem_Ref);
                }
            }

            il.Emit(OpCodes.Ret);
            FastInvokeHandler invoder = (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
            return invoder;
        }
        private static void EmitCastToReference(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }
        private static void EmitBoxIfNeeded(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        private static void EmitFastInt(ILGenerator il, int value)
        {

            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }

        public static void ClearEvent(object obj,string eventName)
        {

            if (string.IsNullOrEmpty(eventName)) return;

            var controlType = obj.GetType();

            BindingFlags mPropertyFlags = BindingFlags.Instance | BindingFlags.Public

                | BindingFlags.Static | BindingFlags.NonPublic;//筛选

            BindingFlags mFieldFlags = BindingFlags.Static | BindingFlags.NonPublic;

//            Type controlType = typeof(System.Windows.Forms.Control);

            PropertyInfo propertyInfo = controlType.GetProperty("Events", mPropertyFlags);

            EventHandlerList eventHandlerList = (EventHandlerList)propertyInfo.GetValue(obj, null);//事件列表

            FieldInfo fieldInfo = (controlType).GetField("Event" + eventName, mFieldFlags);

            Delegate d = eventHandlerList[fieldInfo.GetValue(obj)];
            if (d == null) return;
            EventInfo eventInfo = controlType.GetEvent(eventName);
            foreach (Delegate dx in d.GetInvocationList())

                eventInfo.RemoveEventHandler(obj, dx);//移除已订阅的pEventName类型事件 
        }

    }
}
