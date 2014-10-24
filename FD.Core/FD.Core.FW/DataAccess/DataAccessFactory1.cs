using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Fw.DataAccess.Plugins;
using Fw.Entity;
using Fw.Extends;
using Fw.Reflection;
using Fw.Serializer;
using Fw.Threads;
using Fw.Web;
using ServerFw.DataAccess;
using ServerFw.Reflection;


namespace Fw.DataAccess
{
    public class DataAccessFactory1
    {
//        static Dictionary<string,DataAccessFactory1> Factory = new Dictionary<string, DataAccessFactory1>();
        private static object o = new object();
        public static DataAccessFactory1 GetFactory(string name)
        {
            lock (o)
            {
//                if (Factory.ContainsKey(name)) return Factory[name];
//                else
//                {
                    var a = new DataAccessFactory1(name);
//                    Factory.Add(name,a);
                    return a;
//                }
            }
        }

        public DataAccessFactory1(string key)
        {
            SqlConnKey = key;
        }
        public string SqlConnKey = null;
        static DataAccessFactory1()
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
        public  string ExecuteActionCmdStr(ActionCmd cmd)
        {
            var rd = ExucetActionCmd(cmd);
            return JsonHelper.JsonSerializer(rd);
        }
        /// <summary>
        /// 新版的执行SQL接口
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public  ResultData ExucetActionCmd(ActionCmd cmd)
        {
            switch (cmd.ActionType)
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
                    throw new Exception("未知的操作类型" + cmd.ActionType);
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
        public  ResultData ExecuteStoredProcedureAction(string ActionObjectName, string StoredProcedureName, List<string> ParamsName, List<string> ParamsValue)
        {
            var actionCmd = new ActionCmd
            {
                ActionObjectName = ActionObjectName//返回列表的实体类
                ,
                ActionType = "storedProcedure"//使用
                ,
                StoredProcedureParams = new StoredProcedureParams
                {
                    StoredProcedureName = StoredProcedureName,
                    ParamsName = ParamsName,
                    ParamsValue = ParamsValue
                }
            };
            return ExecuteStoredProcedureAction(actionCmd);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private  ResultData ExecuteStoredProcedureAction(ActionCmd cmd)
        {
            Type type = ReflectionHelper.GetTypeByFullName(cmd.ActionObjectName);
            if (type == null)
            {
                if (cmd.ActionObjectName.EndsWith(".String"))
                {
                    type = typeof(String);
                }
            }
            var access = GetMsSqlDataAccess();
            var obj = access.Select(type, cmd.StoredProcedureParams.StoredProcedureName, cmd.StoredProcedureParams.ParamsName, cmd.StoredProcedureParams.ParamsValue);
            ResultData rd = new ResultData
            {
                ObjectEntryStr = JsonHelper.JsonSerializer(obj)
            };
            return rd;
        }
         
        public  int ExecuteStoredProcNonQuery(string storedProcedureName, List<string> paramsName = null, List<string> paramsValue = null)
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
        private  int UpdateAction(ActionCmd cmd)
        {
            Type type = ReflectionHelper.GetTypeByFullName(cmd.ActionObjectName);
            var access = GetMsSqlDataAccess();
            object item = JsonHelper.JsonDeserialize(cmd.ActionObjectEntryStr, type);
            FieldFilter ff = cmd.FieldFilter ?? new FieldFilter();
            if (cmd.ActionObjectEntryStr.StartsWith("["))
            {
                var listType = typeof(List<>).MakeGenericType(type);
                var items = JsonHelper.JsonDeserialize(cmd.ActionObjectEntryStr, listType);
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
        public  void ThreadUpdateAction<T>(T t, Action<int> result = null)
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
        public  int UpdateAction<T>(T t, string[] white = null, string[] black = null)
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
        public  int UpdateAction(string ActionObjectName, string jsonEntity)
        {
            var actionCmd = new ActionCmd
            {
                ActionObjectName = ActionObjectName//返回列表的实体类
                ,
                ActionType = ActionType.update//使用update 查询
                ,
                ActionObjectEntryStr = jsonEntity
            };
            return UpdateAction(actionCmd);
        }
        /// <summary>
        /// 按ID删除一条或多条记录
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private  int DeleteAction(ActionCmd cmd)
        {
            Type type = ReflectionHelper.GetTypeByFullName(cmd.ActionObjectName);
            var access = GetMsSqlDataAccess();
            return access.Delete(type, cmd.ActionObjectEntryStr.Split(',').ToList());
        }
        public  int DeleteAction<T>(List<T> ids, Func<T, string> getID)
        {
            return DeleteAction<T>(ids.Select(getID).ToList());
        }
        public  int DeleteAction<T>(List<string> ids)
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
        public  int DeleteAction(string ActionObjectName, IEnumerable<string> ids)
        {
            var actionCmd = new ActionCmd
            {
                ActionObjectName = ActionObjectName//返回列表的实体类
                ,
                ActionType = ActionType.delete//使用update 查询
                ,
                ActionObjectEntryStr = ids.Serialize(",", "'")
            };
            return DeleteAction(actionCmd);
        }
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public  ResultData InsertAction<T>(T t)
        {
            var access = GetMsSqlDataAccess();
            //方法1
            ResultData re = new ResultData { Record = access.Insert(typeof(T), t) };
            return re;
        }

        public  void ThreadInsertAction<T>(T t, Action<int> result = null)
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
        public  int InsertAction(string ActionObjectName, string ActionObjectEntryStr)
        {
            var actionCmd = new ActionCmd
            {
                ActionObjectName = ActionObjectName//返回列表的实体类
                ,
                ActionType = ActionType.insert//使用select 查询
                ,
                ActionObjectEntryStr = ActionObjectEntryStr,
            };
            return InsertAction(actionCmd);
        }
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private  int InsertAction(ActionCmd cmd)
        {
            Type type = ReflectionHelper.GetTypeByFullName(cmd.ActionObjectName);
            var access = GetMsSqlDataAccess();
            if (cmd.ActionObjectEntryStr.StartsWith("["))
            {
                var listType = typeof(List<>).MakeGenericType(type);
                var items = JsonHelper.JsonDeserialize(cmd.ActionObjectEntryStr, listType);
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
                object item = JsonHelper.JsonDeserialize(cmd.ActionObjectEntryStr, type);
                //方法1
                return access.Insert(type, item);
            }

        }

        public  ResultData SelectAction(string ActionObjectName, List<SearchEntry> Search, PageInfo PageInfo, string OrderBy)
        {
            var actionCmd = new ActionCmd
            {
                ActionObjectName = ActionObjectName//返回列表的实体类
                ,
                ActionType = ActionType.select//使用select 查询
                ,
                SelectAcionParams = new SelectAcionParams { Search = Search, PageInfo = PageInfo, OrderBy = OrderBy }
            };
            return SelectAction(actionCmd);
        }
        /// <summary>
        /// 搜索记录,可以支持分页
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public  ResultData SelectAction(ActionCmd cmd)
        {
            Type type = ReflectionHelper.GetTypeByFullName(cmd.ActionObjectName);
            if (type == null)
            {
                throw new Exception("未找到类型 " + cmd.ActionObjectName);
            }
            if (cmd.SelectAcionParams == null) cmd.SelectAcionParams = new SelectAcionParams();
            var access = GetMsSqlDataAccess();
            PageInfo pageInfo = null;
            if (cmd.SelectAcionParams != null && cmd.SelectAcionParams.PageInfo != null && cmd.SelectAcionParams.PageInfo.IsPaging == 1)
            {
                pageInfo = cmd.SelectAcionParams.PageInfo;
            }
            FieldFilter ff = cmd.FieldFilter ?? new FieldFilter();
            //方法1
            var dww = access.Select(type, GetWhereStr(cmd.SelectAcionParams.Search, whereStr: cmd.SelectAcionParams.WhereStr), cmd.SelectAcionParams.OrderBy, ref pageInfo, "RowID", white: ff.WhiteNames,black:ff.BlackNames);
            var selectAfterPlugins = GetPlugin<ISelectAfter>();
            //执行插件
            selectAfterPlugins.ForEach(w => w.Execute(dww, type, cmd, access));
            ResultData rd = new ResultData
            {
                ObjectEntryStr = JsonHelper.JsonSerializer(dww)
                    ,
                PageInfo = pageInfo
            };
            return rd;
        }
        public  List<T> Select<T>(List<SearchEntry> search = null, string[] white = null, string[] black = null)
        {
            PageInfo pageinfo = null;
            return Select<T>(search, null, ref pageinfo, white ,black);
        }
        public  List<T> Select<T>(string storedProcedureNae, List<string> parames, List<string> paramesValue)
        {
            var access = GetMsSqlDataAccess();
            return access.Select(typeof(T), storedProcedureNae, parames, paramesValue) as List<T>;
        }

        public  List<T> Select<T>(string storedProcedureNae, List<string> parames, List<string> paramesValue, ref PageInfo pi)
        {
            var access = GetMsSqlDataAccess();
            return access.Select(typeof(T), storedProcedureNae, parames, paramesValue, ref pi) as List<T>;
        }

        public  int TransactionExecuteNonQuery(List<string> sqlList)
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

        public  int TransactionExecuteNonQuery(List<TranParam> operList)
        {
            var sf = new SqlFactory();
            var sqlList = operList.Select(item => sf.GetSql(item.EntityObject, item.OperType, item.Where)).ToList();
            var comm = sf.GetCommand(sqlList);
            var access = GetMsSqlDataAccess();
            return access.ExecuteNonQuery(comm);
            //return 1;
        }

        public  int TransactionExecuteNonQuery(string strSql)
        {
            var sqlList = new List<string> { strSql };
            return TransactionExecuteNonQuery(sqlList);
        }

        public  List<T> Select<T>(List<SearchEntry> search, string order, ref PageInfo pageinfo, string[] white = null,string[] black = null)
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
            return group1.Select(w =>
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
        }

        //private static MsSqlDataAccess current;
        public MsSqlDataAccess GetMsSqlDataAccess(string key = null)
        {
            if (key == null) key = SqlConnKey;
            MsSqlDataAccess current = null;
            var connStr = WebConfigs.GetConfig(key);
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
