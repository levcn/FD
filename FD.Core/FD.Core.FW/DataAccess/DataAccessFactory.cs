using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using Fw.DataAccess.Plugins;
using Fw.Entity;
using Fw.Extends;
using Fw.Reflection;
using Fw.Serializer;
using Fw.Threads;
using Fw.Web;
using ServerFw.DataAccess;
using ServerFw.Reflection;
using STComponse.CFG;


namespace Fw.DataAccess
{
    public static class DataAccessFactory
    {
        /// <summary>
        /// 实体类的设置
        /// </summary>
        public static FwConfig FwConfig { get; set; }

        /// <summary>
        /// 实体类所在的应用程序集.
        /// </summary>
        public static Assembly EntityAssembly { get; set; }

        static DataAccessFactory()
        {
            Plugins.Add(new SelectAfterCustomField());
        }
        /// <summary>
        /// 插件列表
        /// </summary>
        static List<IDataAccessPlugin> Plugins = new List<IDataAccessPlugin>();

        /// <summary>
        /// 返回指定类型的插件是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool ExistPlugin<T>() where T : class, IDataAccessPlugin
        {
            return (GetPlugin<T>() != null);
        }
        /// <summary>
        /// 添加一个插件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p"></param>
        public static void AddPlugin<T>(T p) where T : class, IDataAccessPlugin
        {
            if (!ExistPlugin<T>()) Plugins.Add(p);
        }

        /// <summary>
        /// 移除指定的类型的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RemovePlugin<T>() where T : class, IDataAccessPlugin
        {
            var exists = GetPlugin<T>();
            if (exists != null && exists.Count > 0)
            {
                exists.ForEach(w => Plugins.Remove(w));
            }
        }
        /// <summary>
        /// 返回指定类型的插件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<T> GetPlugin<T>() where T : class, IDataAccessPlugin
        {
            var type = typeof(T);
            if (type.IsInterface)
            {
                var list = Plugins.Where(w => w.GetType().GetInterface(type.Name) != null).ToList();
                var dww = list.Select(w => w as T).ToList();
                return dww;
            }
            else
            {
                return Plugins.Where(w => w.GetType().FullName == type.FullName).ToList().Select(w => w as T).ToList();
            }
        }

        /// <summary>
        /// 把一上object对象转成List object对象
        /// </summary>
        /// <param name="serObj"></param>
        /// <returns></returns>
        private static List<object> GetListByObject(object serObj)
        {
            var type = serObj.GetType();
//            var itemType = type.GetGenericArguments()[0];
            List<object> re = new List<object>();
//            RCache<Type, PropertyInfo[]>.GetValue(type, type.GetProperties).FirstOrDefault(w => w.Name == "Count");
            var propertyInfos = RCache<Type, PropertyInfo[]>.GetValue(type, type.GetProperties);
            var prop = propertyInfos.FirstOrDefault(w => w.Name == "Count");
//            var prop = type.GetProperty("Count");
//            var dww = propertyInfos.Select(w => w.Name).ToList();
            var itemProp = propertyInfos.FirstOrDefault(w => w.Name == "Item");
//            var itemProp = type.GetProperty("Item");

            var count = (int)prop.GetValue(serObj, null);
            for (int i = 0; i < count; i++)
            {
                var item1 = itemProp.GetValue(serObj, new object[] { i });
                re.Add(item1);
            }
            return re;
        }

        public static List<object> GetListByID(List<object> obj, List<Guid> ids)
        {
            if (obj != null && obj.Count > 0)
            {
                var prop = ReflectionHelper.GetPrimaryKey(obj[0].GetType());
                var re = obj.Where(w =>
                {
                    object o = prop.GetValue(w, null);
                    if (o != null)
                    {
                        string id = o.ToString().ToLower();
                        if (ids.Any(q => q.ToString().ToLower() == id)) return true;
                    }
                    return false;
                }).ToList();
                return re;
            }
            else
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// 新版的执行SQL接口
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static string ExecuteActionCmdStr(ActionCommand cmd)
        {
            ResultData rd;
            if (cmd.Operator.Version == 2)
            {
                rd = ExucetActionCmdV2(cmd);
            }
            else
            {
                rd = ExucetActionCmd(cmd);
            }
            
            return JsonHelper.JsonSerializer(rd);
        }
        /// <summary>
        /// 新版的执行SQL接口
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static ResultData ExucetActionCmd(ActionCommand cmd)
        {
            switch (cmd.Operator.ActionType)
            {
                case ActionType.select:
                    return SelectAction(cmd);
                case ActionType.insert:
                    return new ResultData { Record = InsertAction(cmd) };
                case ActionType.delete:
                    return new ResultData { Record = DeleteAction(cmd) };
                case ActionType.update:
                    return new ResultData { Record = UpdateAction(cmd) };
                case ActionType.storedProcedure:
                    return ExecuteStoredProcedureAction(cmd);
                default:
                    throw new Exception("未知的操作类型" + cmd.Operator.ActionType);
            }
        }
        /// <summary>
        /// 新版的执行SQL接口
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static ResultData ExucetActionCmdV2(ActionCommand cmd)
        {
            switch (cmd.Operator.ActionType)
            {
                case ActionType.select:
                    return SelectActionV2(cmd);
                case ActionType.insert:
                    return new ResultData { Record = InsertActionV2(cmd) };
                case ActionType.delete:
                    return new ResultData { Record = DeleteActionV2(cmd) };
                case ActionType.update:
                    return new ResultData { Record = UpdateActionV2(cmd) };
                case ActionType.storedProcedure:
                    return ExecuteStoredProcedureActionV2(cmd);
                case ActionType.storedProcedureCheck:
                    return ExecuteStoredProcedureActionV2(cmd);
                default:
                    throw new Exception("未知的操作类型" + cmd.Operator.ActionType);
            }
        }

        #region 执行存储过程

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ActionObjectName">返回对象的全名</param>
        /// <param name="StoredProcedureName">存储过程名</param>
        /// <param name="ParamsName">存储过程参数</param>
        /// <param name="ParamsValue">存储过程参数的值</param>
        /// <returns></returns>
        public static ResultData ExecuteStoredProcedureAction(string ActionObjectName, string StoredProcedureName, List<STParamete> parametes)
        {
            var actionCmd = new ActionCommand();
            actionCmd.Entity.ActionObjectName = ActionObjectName;//返回列表的实体类
            actionCmd.Operator.ActionType = "storedProcedure";//使用
            actionCmd.Parameter.StoredProcedureParams = new StoredProcedureParams
            {
                StoredProcedureName = StoredProcedureName,
                Params = parametes,
            };
            return ExecuteStoredProcedureAction(actionCmd);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static ResultData ExecuteStoredProcedureActionV2(ActionCommand cmd)
        {
//            Type type = ReflectionHelper.GetTypeByFullName(cmd.Entity.ActionObjectName);
            var type = FwConfig.FindObject(w => w.ObjectCode == cmd.Entity.ActionObjectName);
//            if (type == null)
//            {
//                if (cmd.Entity.ActionObjectName.EndsWith(".String"))
//                {
//                    type = typeof(String);
//                }
//            }
            if (type == null) throw new Exception("未找到类型:" + cmd.Entity.ActionObjectName);
            
            var spName = cmd.Parameter.StoredProcedureParams.StoredProcedureName;

            var sp = FwConfig.StoredProcedures.FirstOrDefault(w => w.ObjectCode == spName);
            if (sp == null) throw new Exception("未找到存储过程:" + spName);
            var access = GetMsSqlDataAccess();
            var _params = sp.Parameters.Select(w => w.Code).ToList();
            var type1 = EntityAssembly.GetTypes().FirstOrDefault(w => w.Name == type.ObjectCode);
            if (type1 == null) throw new Exception("未找到类型:" + type.ObjectCode);
            var entity = JsonHelper.JsonDeserialize(cmd.Entity.ActionObjectEntryStr, type1);

            var props = type1.GetProperties().ToList();
            List<SqlParameter> parameters = new List<SqlParameter>();
            var ps = cmd.Parameter.StoredProcedureParams.Params;
            var values = _params.Select(w =>
            {
                var f = ps.FirstOrDefault(z=>z.Name == w);
                if (f != null && !string.IsNullOrEmpty(f.Value))//优先使用设置好的值
                {
                    return f.Value;
                }
                else
                {
                    var p = props.FirstOrDefault(z => z.Name == w);//从对象上取值
                    if (p != null)
                    {
                        var v = p.GetValue(entity, null);
                        if (v != null)
                        {
                            return v;
                        }
                    }
                }
                return "";
            }).ToList();
            for (int i = 0; i < _params.Count; i++)
            {
                SqlParameter parameter = new SqlParameter(_params[i],values[i]);
                if (sp.Parameters[i].IsOutput)
                {
                    parameter.Direction = ParameterDirection.InputOutput;
                    parameter.Size = 1000;
                }
                parameters.Add(parameter);
            }
            var obj = access.Select(EntityAssembly,type,FwConfig, spName, parameters, out parameters);
            
            var outputValues = cmd.Parameter.StoredProcedureParams.OutputValues;
            if (obj.Count > 0)
            {
                var o = obj[0];
                for (int i = 0; i < _params.Count; i++)
                {
                    if (parameters[i].Direction == ParameterDirection.InputOutput)
                    {
                        var p = props.FirstOrDefault(w => w.Name == _params[i]);
                        if (p != null)
                        {
                            p.SetValue(o, parameters[i].Value, null);
                        }
                        var item = outputValues.FirstOrDefault(z => z.Name == _params[i]);
                        if (item == null)
                        {
                            outputValues.Add(
                                    new OutputValue {
                                        Name = _params[i],
                                        Value = parameters[i].Value.ToString()
                                    });
                        }
                        else
                        {
                            item.Value = parameters[i].Value.ToString();
                        }
                    }
                }
            }
            else
            {
                var list = ReflectionHelper.GetObject(typeof (List<>).MakeGenericType(type1)) as IList;
                list.Add(entity);
                obj = list;
            }
            ResultData rd = new ResultData
            {
                ObjectEntryStr = JsonHelper.JsonSerializer(obj),
                OutputValues = outputValues,
            };
            return rd;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static ResultData ExecuteStoredProcedureAction(ActionCommand cmd)
        {
            Type type = ReflectionHelper.GetTypeByFullName(cmd.Entity.ActionObjectName);
            if (type == null)
            {
                if (cmd.Entity.ActionObjectName.EndsWith(".String"))
                {
                    type = typeof(String);
                }
            }
            if (type == null) throw new Exception("未找到类型:" + cmd.Entity.ActionObjectName);
            var entity = JsonHelper.JsonDeserialize(cmd.Entity.ActionObjectEntryStr, type);
            var spName = cmd.Parameter.StoredProcedureParams.StoredProcedureName;

            var sp = FwConfig.StoredProcedures.FirstOrDefault(w => w.ObjectCode == spName);
            if (sp == null) throw new Exception("未找到存储过程:" + spName);
            
            var _params = sp.Parameters.Select(w => w.Code).ToList();
            var props = type.GetProperties().ToList();
            List<SqlParameter> parameters = new List<SqlParameter>();
            var values = _params.Select(w =>
            {
                var p = props.FirstOrDefault(z => z.Name == w);
                if (p != null)
                {
                    var v = p.GetValue(entity, null);
                    if (v != null)
                    {
                        return v;
                    }

                }
                return "";
            }).ToList();
            for (int i = 0; i < _params.Count; i++)
            {
                SqlParameter parameter = new SqlParameter(_params[i], values[i]);
                if (sp.Parameters[i].IsOutput) parameter.Direction = ParameterDirection.InputOutput;
                parameters.Add(parameter);
            }
            var access = GetMsSqlDataAccess();
            var obj = access.Select(type, spName, parameters, out parameters);
            if (obj.Count > 0)
            {
                var o = obj[0];
                for (int i = 0; i < _params.Count; i++)
                {
                    if (parameters[i].Direction == ParameterDirection.InputOutput)
                    {
                        var p = props.FirstOrDefault(w => w.Name == _params[i]);
                        if (p != null)
                        {
                            p.SetValue(o, parameters[i].Value, null);
                        }
                    }
                }
            }
            ResultData rd = new ResultData
            {
                ObjectEntryStr = JsonHelper.JsonSerializer(obj)
            };
            return rd;
        }
        public static int ExecuteStoredProcNonQuery(string storedProcedureName, List<string> paramsName = null, List<string> paramsValue = null)
        {
            var access = GetMsSqlDataAccess();
            return access.ExecuteStoredProcNonQuery(storedProcedureName, paramsName, paramsValue);
        }

        #endregion
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static int UpdateAction(ActionCommand cmd)
        {
            Type type = ReflectionHelper.GetTypeByFullName(cmd.Entity.ActionObjectName);
            var access = GetMsSqlDataAccess();
            object item = JsonHelper.JsonDeserialize(cmd.Entity.ActionObjectEntryStr, type);
            FieldFilter ff = cmd.Parameter.FieldFilter ?? new FieldFilter();
            if (cmd.Entity.ActionObjectEntryStr.StartsWith("["))
            {
                var listType = typeof(List<>).MakeGenericType(type);
                var items = JsonHelper.JsonDeserialize(cmd.Entity.ActionObjectEntryStr, listType);
                var itemObject = ReflectionHelper.GetListItems(items);
                var re = 0;
                for (int i = 0; i < itemObject.Count; i++)
                {
                    re += access.Update(type, itemObject[i],black:ff.BlackNames,white:ff.WhiteNames);
                }
                return re;
            }
            else
            {
                return access.Update(type, item, black: ff.BlackNames, white: ff.WhiteNames);
            }
        }
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static int UpdateActionV2(ActionCommand cmd)
        {
//            Type type = ReflectionHelper.GetTypeByFullName(cmd.ActionObjectName);
//            var obj = FwConfig.FindObject(w => w.ObjectCode == cmd.ActionObjectName);
            var type = EntityAssembly.GetType(cmd.Entity.ActionObjectName);
            var access = GetMsSqlDataAccess();
            object item = JsonHelper.JsonDeserialize(cmd.Entity.ActionObjectEntryStr, type);
            FieldFilter ff = cmd.Parameter.FieldFilter ?? new FieldFilter();
            if (cmd.Entity.ActionObjectEntryStr.StartsWith("["))
            {
                var listType = typeof(List<>).MakeGenericType(type);
                var items = JsonHelper.JsonDeserialize(cmd.Entity.ActionObjectEntryStr, listType);
                var itemObject = ReflectionHelper.GetListItems(items);
                var re = 0;
                for (int i = 0; i < itemObject.Count; i++)
                {
                    re += access.Update(FwConfig, itemObject[i], black: ff.BlackNames, white: ff.WhiteNames);
                }
                return re;
            }
            else
            {
                return access.Update(FwConfig, item, black: ff.BlackNames, white: ff.WhiteNames);
            }
        }
        public static void ThreadUpdateAction<T>(T t, Action<int> result = null)
        {
            UpdateDBThreadPool<T>.Add(t);
            ThreadHelper.StartThread(() =>
            {
                var re = UpdateAction(t);
                UpdateDBThreadPool<T>.Remove(t);
                if (result != null)
                {
                    result(re);
                }
            });
        }
        public static int UpdateAction<T>(T t, string[] white = null, string[] black = null)
        {
            var access = GetMsSqlDataAccess();
            return access.Update(typeof(T), t,white,black);
        }
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="ActionObjectName">类型的完整名称</param>
        /// <param name="jsonEntity">Json格式的数据</param>
        /// <returns></returns>
        public static int UpdateAction(string ActionObjectName, string jsonEntity)
        {
            var actionCmd = new ActionCommand();
            actionCmd.Entity.ActionObjectName = ActionObjectName;//返回列表的实体类
            actionCmd.Operator.ActionType = ActionType.update;//使用update 查询
            actionCmd.Entity.ActionObjectEntryStr = jsonEntity;
            return UpdateAction(actionCmd);
        }
        /// <summary>
        /// 按ID删除一条或多条记录
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static int DeleteAction(ActionCommand cmd)
        {
            Type type = ReflectionHelper.GetTypeByFullName(cmd.Entity.ActionObjectName);
            var access = GetMsSqlDataAccess();
            return access.Delete(type, cmd.Entity.ActionObjectEntryStr.Split(',').ToList());
        }
        /// <summary>
        /// 按ID删除一条或多条记录
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static int DeleteActionV2(ActionCommand cmd)
        {
//            Type type = ReflectionHelper.GetTypeByFullName(cmd.ActionObjectName);
            var obj = FwConfig.FindObject(w => w.ObjectCode == cmd.Entity.ActionObjectName);
            if (obj == null) throw new Exception(string.Format("未找到类型为{0}的配置", cmd.Entity.ActionObjectName));
//            var type = EntityAssembly.GetType(cmd.ActionObjectName);
            var access = GetMsSqlDataAccess();
            return access.Delete(obj, cmd.Entity.ActionObjectEntryStr.Split(',').ToList());
        }

        /// <summary>
        /// 返回数据库是否可以打开
        /// </summary>
        /// <returns></returns>
        public static bool CheckDBConnection()
        {
            var access = GetMsSqlDataAccess();
            return access.CheckDBConnection();
        }
        public static int DeleteAction<T>(List<T> ids, Func<T, string> getID)
        {
            return DeleteAction<T>(ids.Select(getID).ToList());
        }
        public static int DeleteAction<T>(List<string> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                return DeleteAction(typeof(T).FullName, ids);
            }
            return 0;
        }
        /// <summary>
        /// ID删除一条或多条记录
        /// </summary>
        /// <param name="ActionObjectName">类型的完整名称</param>
        /// <param name="ids">Id</param>
        /// <returns></returns>
        public static int DeleteAction(string ActionObjectName, IEnumerable<string> ids)
        {
            var actionCmd = new ActionCommand();
                actionCmd.Entity.ActionObjectName = ActionObjectName;//返回列表的实体类
                actionCmd.Operator.ActionType = ActionType.delete;//使用update 查询
            actionCmd.Entity.ActionObjectEntryStr = ids.Serialize(",", "'");
            return DeleteAction(actionCmd);
        }
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static ResultData InsertAction<T>(T t)
        {
            var access = GetMsSqlDataAccess();
            //方法1
            ResultData re = new ResultData { Record = access.Insert(typeof(T), t) };
            return re;
        }

        public static void ThreadInsertAction<T>(T t, Action<int> result = null)
        {
            InsertThreadPool<T>.Add(t);
            ThreadHelper.StartThread(() =>
            {
                var re = InsertAction(t);
                InsertThreadPool<T>.Remove(t);
                if (result != null)
                {
                    result(re.Record);
                }
            });
        }
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="ActionObjectName">类型的完整名称</param>
        /// <param name="ActionObjectEntryStr">Json格式的实体</param>
        /// <returns></returns>
        public static int InsertAction(string ActionObjectName, string ActionObjectEntryStr)
        {
            var actionCmd = new ActionCommand();
              actionCmd.Entity.ActionObjectName = ActionObjectName;//返回列表的实体类
            actionCmd.Operator.ActionType = ActionType.insert;//使用select 查询
            actionCmd.Entity.ActionObjectEntryStr = ActionObjectEntryStr;
            return InsertAction(actionCmd);
        }

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static int InsertAction(ActionCommand cmd)
        {
            Type type = ReflectionHelper.GetTypeByFullName(cmd.Entity.ActionObjectName);
            var access = GetMsSqlDataAccess();
            if (cmd.Entity.ActionObjectEntryStr.StartsWith("["))
            {
                var listType = typeof(List<>).MakeGenericType(type);
                var items = JsonHelper.JsonDeserialize(cmd.Entity.ActionObjectEntryStr, listType);
                var itemObject = ReflectionHelper.GetListItems(items);
                var re = 0;
                for (int i = 0; i < itemObject.Count; i++)
                {
                    re += access.Insert(type, itemObject[i]);
                }
                return re;
            }
            else
            {
                object item = JsonHelper.JsonDeserialize(cmd.Entity.ActionObjectEntryStr, type);
                //方法1
                return access.Insert(type, item);
            }

        }
/// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static int InsertActionV2(ActionCommand cmd)
        {
//            var obj = FwConfig.FindObject(w => w.ObjectName == cmd.ActionObjectName);
//    var list = EntityAssembly.GetTypes().Select(w => w.FullName).ToList();
    var type = EntityAssembly.GetTypes().FirstOrDefault(w => w.Name == cmd.Entity.ActionObjectName);
//            var type = EntityAssembly.GetType(cmd.Entity.ActionObjectName);

            if (type == null) throw new Exception("未找到类型" + cmd.Entity.ActionObjectName);
//            if (obj == null)
//            Type type = ReflectionHelper.GetTypeByFullName(cmd.ActionObjectName);
            var access = GetMsSqlDataAccess();
            if (cmd.Entity.ActionObjectEntryStr.StartsWith("["))
            {
                var listType = typeof(List<>).MakeGenericType(type);
                var items = JsonHelper.JsonDeserialize(cmd.Entity.ActionObjectEntryStr, listType);
                var itemObject = ReflectionHelper.GetListItems(items);
                var re = 0;
                for (int i = 0; i < itemObject.Count; i++)
                {
                    re += access.Insert(FwConfig,itemObject[i]);
                }
                return re;
            }
            else
            {
                object item = JsonHelper.JsonDeserialize(cmd.Entity.ActionObjectEntryStr, type);
                //方法1
                return access.Insert(FwConfig, item);
            }

        }
        public static ResultData SelectAction(string ActionObjectName, List<SearchEntry> Search, PageInfo PageInfo, string OrderBy)
        {
            var actionCmd = new ActionCommand();
            actionCmd.Entity.ActionObjectName = ActionObjectName;//返回列表的实体类
            actionCmd.Operator.ActionType = ActionType.select;//使用select 查询
            actionCmd.Parameter.SelectAcionParams = new SelectAcionParams {Search = Search, PageInfo = PageInfo, OrderBy = OrderBy};
            return SelectAction(actionCmd);
        }
        /// <summary>
        /// 搜索记录,可以支持分页
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static ResultData SelectAction(ActionCommand cmd)
        {
            Type type = ReflectionHelper.GetTypeByFullName(cmd.Entity.ActionObjectName);
            if (type == null)
            {
                throw new Exception("未找到类型 " + cmd.Entity.ActionObjectName);
            }
            if (cmd.Parameter.SelectAcionParams == null) cmd.Parameter.SelectAcionParams = new SelectAcionParams();
            var access = GetMsSqlDataAccess();
            PageInfo pageInfo = null;
            if (cmd.Parameter.SelectAcionParams != null && cmd.Parameter.SelectAcionParams.PageInfo != null && cmd.Parameter.SelectAcionParams.PageInfo.IsPaging == 1)
            {
                pageInfo = cmd.Parameter.SelectAcionParams.PageInfo;
            }
            FieldFilter ff = cmd.Parameter.FieldFilter ?? new FieldFilter();
            //方法1
            var dww = access.Select(type, GetWhereStr(cmd.Parameter.SelectAcionParams.Search, whereStr: cmd.Parameter.SelectAcionParams.WhereStr), cmd.Parameter.SelectAcionParams.OrderBy, ref pageInfo, "RowID", white: ff.WhiteNames,black:ff.BlackNames);
            var selectAfterPlugins = GetPlugin<ISelectAfter>();
            //执行插件
            selectAfterPlugins.ForEach(w => w.Execute(dww, type, cmd, access));
            ResultData rd = new ResultData
            {
                ObjectEntryStr = JsonHelper.FastJsonSerializer(dww)
                    ,
                PageInfo = pageInfo
            };
            return rd;
        }
        /// <summary>
        /// 搜索记录,可以支持分页
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static ResultData SelectActionV2(ActionCommand cmd)
        {
            var obj = FwConfig.FindObject(w => w.ObjectCode == cmd.Entity.ActionObjectName);
            if (obj == null)
            {
                throw new Exception("未找到类型 " + cmd.Entity.ActionObjectName);
            }
            if (cmd.Parameter.SelectAcionParams == null) cmd.Parameter.SelectAcionParams = new SelectAcionParams();
            var access = GetMsSqlDataAccess();
            PageInfo pageInfo = null;
            if (cmd.Parameter.SelectAcionParams != null && cmd.Parameter.SelectAcionParams.PageInfo != null && cmd.Parameter.SelectAcionParams.PageInfo.IsPaging == 1)
            {
                pageInfo = cmd.Parameter.SelectAcionParams.PageInfo;
            }
            FieldFilter ff = cmd.Parameter.FieldFilter ?? new FieldFilter();
            //方法1
            var dww = access.Select(EntityAssembly, obj, FwConfig, GetWhereStr(cmd.Parameter.SelectAcionParams.Search, whereStr: cmd.Parameter.SelectAcionParams.WhereStr), cmd.Parameter.SelectAcionParams.OrderBy, ref pageInfo, "RowID", white: ff.WhiteNames, black: ff.BlackNames);
            var selectAfterPlugins = GetPlugin<ISelectAfter>();
            //执行插件
            selectAfterPlugins.ForEach(w => w.Execute(dww, cmd, access));
            ResultData rd = new ResultData
            {
                ObjectEntryStr = JsonHelper.FastJsonSerializer(dww)
                    ,
                PageInfo = pageInfo
            };
            return rd;
        }
        public static List<T> Select<T>(List<SearchEntry> search = null, string[] white = null, string[] black = null)
        {
            PageInfo pageinfo = null;
            return Select<T>(search, null, ref pageinfo, white ,black);
        }
        public static List<T> Select<T>(string storedProcedureNae, List<STParamete> parametes)
        {
            var access = GetMsSqlDataAccess();
            return access.Select(typeof(T), storedProcedureNae, parametes) as List<T>;
        }

        public static List<T> Select<T>(string storedProcedureNae, List<STParamete> parametes, ref PageInfo pi)
        {
            var access = GetMsSqlDataAccess();
            return access.Select(typeof(T), storedProcedureNae, parametes, ref pi) as List<T>;
        }

        public static int TransactionExecuteNonQuery(List<string> sqlList)
        {
            var sf = new SqlFactory();
            var comm = sf.GetCommand(sqlList);
            var access = GetMsSqlDataAccess();
            return access.ExecuteNonQuery(comm);
        }

        public static int TransactionExecuteNonQuery(Dictionary<object, SqlOperType> operList)
        {
            var sf = new SqlFactory();
            var sqlList = operList.Select(item => sf.GetSql(item.Key, item.Value)).ToList();
            //var comm = sf.GetCommand(sqlList);
            //var access = GetMsSqlDataAccess();
            //return access.ExecuteNonQuery(comm);
            return 1;
        }

        public static int TransactionExecuteNonQuery(List<TranParam> operList)
        {
            var sf = new SqlFactory();
            var sqlList = operList.Select(item => sf.GetSql(item.EntityObject, item.OperType, item.Where)).ToList();
            var comm = sf.GetCommand(sqlList);
            var access = GetMsSqlDataAccess();
            return access.ExecuteNonQuery(comm);
            //return 1;
        }

        public static int TransactionExecuteNonQuery(string strSql)
        {
            var sqlList = new List<string> { strSql };
            return TransactionExecuteNonQuery(sqlList);
        }

        public static List<T> Select<T>(List<SearchEntry> search, string order, ref PageInfo pageinfo, string[] white = null,string[] black = null)
        {
            var access = GetMsSqlDataAccess();
            var tableName = ReflectionHelper.GetTableName<T>();
            var pKey = ReflectionHelper.GetPrimaryPropertyField(typeof(T));
            var dww = access.Select(typeof(T), GetWhereStr(search, tableName, pKey), order, ref pageinfo, "RowID",white:white,black:black) as List<T>;

            return dww;
        }

        /// <summary>
        /// 给定一些条件,返回条件的SQL查询
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tableName">主表的表名 </param>
        /// <param name="pKey">主表的主键 </param>
        /// <returns></returns>
        private static string GetWhereStr(List<SearchEntry> list, string tableName = null, string pKey = null, string whereStr = null)
        {
            if (list == null || list.Count == 0) return "1=1";
            list = list.Select(w =>
                                   {
                                       if (string.Equals(w.ColumnName, pKey, StringComparison.OrdinalIgnoreCase))
                                       {
                                           w.ColumnName = string.Format("{0}.{1}", tableName, w.ColumnName);
                                       }
                                       return w;
                                   }).ToList();
            var group1 = list.Where(w => string.IsNullOrEmpty(w.GroupName)).ToList();
            var otherList = list.Where(w => group1.All(q => q != w)).ToList();
            var otherGroups = otherList.GroupBy(w => w.GroupName).ToList();
            var str = "";
            if (group1.Count != 0)
            {
                str = GetOneWhereStr(group1, "and");
                if (otherGroups.Count > 0) str += " and ";
            }
            if (otherGroups.Count > 0)
            {
                str += otherGroups.Select(w => GetOneWhereStr(w.ToList(), " or ")).ToList().Serialize(" and ");
            }
            if (str == "")
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    var w = whereStr.Trim().ToLower();
                    if (w.StartsWith("and ")) str = w.Substring(4, w.Length - 4);
                    else if (w.StartsWith("or ")) str = w.Substring(3, w.Length - 3);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    str += " " + whereStr;
                }
            }
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group1"></param>
        /// <param name="andOr"> </param>
        /// <returns></returns>
        private static string GetOneWhereStr(IEnumerable<SearchEntry> group1, string andOr)
        {
            var str = group1.Select(w =>
                {
                    if (w.Flag.Trim().IndexOf("in") >= 0)
                    {
                        return string.Format("{0} {1} ({2})", w.ColumnName, w.Flag, w.GetSearchValue());
                    }
                    else
                    {
                        return string.Format("{0} {1} {2}", w.ColumnName, w.Flag, w.GetSearchValue());
                    }

                }).Serialize(" " + andOr + " ");
            return string.Format("( {0} )",str);
        }

        public static MsSqlDataAccess GetMsSqlDataAccess(string connectionStr = null)
        {
            MsSqlDataAccess current = null;
            var connStr = connectionStr??WebConfigs.GetConfig("SqlServer");
            if (current == null)
            {
                try
                {
                    var logpath = AppSetting.SiteRoot + @"sql.log";
                    logpath.TryDeleteFile();
                    current =
                            new MsSqlDataAccess
                                {

                                    LogStream =
                                            new FileStream(logpath, FileMode.OpenOrCreate, FileAccess.Write,
                                                           FileShare.ReadWrite),
                                    ConnectionString = connStr
                                };
                }
                catch
                {
                    current = new MsSqlDataAccess
                               {
                                   ConnectionString = connStr
                               };
                }
            }
            return current;
        }
    }
}
