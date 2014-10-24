using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SLControls.Editors;
using SLControls.Extends;
using StaffTrain.FwClass.DataClientTools;
using StaffTrain.FwClass.DataClientTools.Configs;
using StaffTrain.FwClass.NavigatorTools;
using StaffTrain.FwClass.Reflectors;
using StaffTrain.FwClass.Serializer;


namespace SLControls.DataClientTools
{
    /// <summary>
    /// 对架构通用方法的二次封装
    /// </summary>
    public static class ActionHelper
    {
        /// <summary>
        /// 返回一个转换数据类型的匿名方法
        /// </summary>
        /// <typeparam name="targetT"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Action<string> GetAction1<targetT>(Action<targetT> action) where targetT : class
        {
            return s => action(XmlHelper.GetXmlDeserialize<targetT>(s));
        }

//        public static async Task<ResultData> ActionRequest(ActionCommand ac)
//        {
//            return await ActionRequest(ac);
//        }
//        public static async Task<ResultData> ActionRequest(ActionCmd ac,  IPopupable ui = null, WattingConfig showPopConfig = null)
//        {
//            var showPop = true;
//            if (showPopConfig != null) showPop = showPopConfig.Show;
//            if (ui == null) ui = ActionSetting.PopupPage;
//            Action<WattingConfig> watting = null;
//            if (ui != null) watting = ui.OpenWattingPopup;
////            Action<PostResult<T>> re = w =>
////                                           {
////                                               postBackMethod(w);
////                                               if (ui != null && showPop) ui.ClosePopup();
////                                           };
////            if (!showPop) watting = null;
//            var re = await ActionRequest(ac,  watting,wc:showPopConfig);
//            if (ui != null && showPop) ui.ClosePopup();
//            return re;
//        }
//        public static async Task<ResultData> ActionRequest(ActionCommand ac)
//        {
////            if (ui == null) ui = ActionSetting.PopupPage;
////            Action<WattingConfig> watting = null;
////            if (ui != null) watting = ui.OpenWattingPopup;
//            var re1 = await ActionRequest(ac,);
////            if (ui != null) ui.ClosePopup();
//            return re1;
//        }
//        public static async Task<ResultData> ActionRequest(ActionCommand ac, Action<ResultData> exception )
//        {
//            return await DataAccess.ActionRequest(ActionSetting.DataAccessUri, ac, exception);
//        }
        public static async Task<ResultData> ActionRequest(ActionCommand ac, ActionConfig acb = null)
        {
            return await DataAccess.ActionRequest(ActionSetting.DataAccessUri, ac, acb);
        }

        /// <summary>
        /// 执行服务器的一个方法并返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userActionClassName"></param>
        /// <param name="userActionMethodName"></param>
        /// <param name="paramTypes"></param>
        /// <param name="param"></param>
        /// <param name="executeMehod"></param>
        /// <param name="showPop"></param>
        /// <param name="isRelativePath"></param>
        /// <param name="wc"></param>
        public static async Task<T> CustomRequest<T>(string userActionClassName, string userActionMethodName, object[] param, bool showPop = true, bool isRelativePath = false)
        {
            List<string> strParam = new List<string>();
            if (param != null)
            {
                param.ForEach(w => strParam.Add(XmlHelper.GetXmlSerialize(w)));
            }

            var actionCmd = new ActionCommand();
            actionCmd.Operator.CustomCmd = new CustomCmd {
                ClassName = userActionClassName,
                IsRelativePath = isRelativePath,
                MethodName = userActionMethodName,
                //                    ParamTypes = paramTypes,
                Params = strParam
            };
            actionCmd.Operator.IsCustomCmd = true;
//                IsCustomCmd = true,
//                ActionObjectName = ReflectionHelper.GetTypeName(typeof(T))//返回列表的实体类
//            };

            var re = await ActionRequest(actionCmd);
            return re.GetEntity<T>();
        }
       
//        /// <summary>
//        /// 执行服务器的一个方法并返回结果
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="userActionClassName"></param>
//        /// <param name="userActionMethodName"></param>
//        /// <param name="paramTypes"></param>
//        /// <param name="param"></param>
//        /// <param name="executeMehod"></param>
        
//        public static T CustomRequest<T>(string userActionClassName, string userActionMethodName, List<string> paramTypes, List<object> param, Action<T, Action> executeMehod, WattingConfig wc)
//        {
//            List<string> strParam = new List<string>();
//            if (param != null)
//            {
//                param.ForEach(w => strParam.Add(XmlHelper.GetXmlSerialize(w)));
//            }
//            var actionCmd = new ActionCmd
//            {
//                CustomCmd = new CustomCmd
//                {
//                    ClassName = userActionClassName
//                    ,
//                    MethodName = userActionMethodName,
//                    ParamTypes = paramTypes,
//                    Params = strParam
//                },
//                IsCustomCmd = true,
////                ActionObjectName = ReflectionHelper.GetTypeName(typeof(T))//返回列表的实体类
//            };
//            ActionRequest(actionCmd, null, null,);
//        }

        /// <summary>
        /// 通用的删除命令(delete)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"> </param>
        public static void Delete<T>(Guid id)
        {
            Delete<T>(id.ToString());
        }
        /// <summary>
        /// 通用的删除命令(delete)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"> </param>
        public static void Delete<T>(string id)
        {
            DeleteList<T>(new List<string> { id });
        }

        public static async Task<object> Delete(Type type,List<string> ids)
        {
//            id = id.Include("'");
            var method = typeof(ActionHelper).GetMethod("DeleteList");
            var method1 = method.MakeGenericMethod(type);
            var re123 = method1.Invoke(null, new object[] { ids}) as Task;
            var re = await ReflectionHelper.AwaitExecute(type, re123);
            return re;
        }
        /// <summary>
        /// 通用的删除命令(delete)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"> </param>
        /// <param name="showPop"></param>
        public static async Task<object> DeleteList<T>(List<string> ids)
        {
            var cmd = new ActionCommand();
            cmd.Entity.ActionObjectEntryStr = ids.Serialize(",", "'");
            cmd.Operator.ActionType = ActionTypeConst.delete;
            cmd.Entity.ActionObjectName = ReflectionHelper.GetTypeName(typeof (T));
//            var actionCmd = new ActionCommand
//            {
//                Entity = = ReflectionHelper.GetTypeName(typeof(T))//返回列表的实体类
//                ,
//                ActionType = ActionTypeConst.delete//使用update 查询
//                ,
//                ActionObjectEntryStr = ids.Serialize(",", "'")
//            };
            return await ActionRequest(cmd);
        }

        /// <summary>
        /// 通用的更新命令(Insert)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public static void Insert<T>(T entity)
        {
            Insert(entity, true);
        }

        public static async Task<object> Insert(Type type,object entities, bool showpop)
        {
            var method = typeof(ActionHelper).GetMethod("Insert1");
//            var method1 = method.MakeGenericMethod(type);
            var re123 = method.Invoke(null, new object[] { type, entities, showpop }) as Task;
            var re = await ReflectionHelper.AwaitExecute(type, re123);
            return re;
        }

        public static async Task<int> Insert1(Type type,object entities, bool showpop)
        {
//            var type = typeof(T);
            var uploadType = type;
            if (type.IsGenericType)
            {
                uploadType = ReflectionHelper.GetGenericType(type);
            }
            var actionCmd = new ActionCommand();
            actionCmd.Entity.ActionObjectName = ReflectionHelper.GetTypeName(uploadType);
            actionCmd.Operator.ActionType = ActionTypeConst.insert;
            actionCmd.Entity.ActionObjectEntryStr = JsonHelper.JsonSerializer(entities);
//            {
//                ActionObjectName = ReflectionHelper.GetTypeName(uploadType)//返回列表的实体类
//                ,
//                ActionType = ActionTypeConst.insert//使用update 查询
//                ,
//                ActionObjectEntryStr = JsonHelper.JsonSerializer(entities)
//            };
            var re = await ActionRequest(actionCmd);
            return re.Record;
        }
        public static async Task<int> Insert<T>(T entities, bool showpop)
        {
            return await Insert1(typeof (T), entities, showpop);
        }
        /// <summary>
        /// 通用的更新命令(update)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public  static async Task<bool> Update<T>(T entity)
        {
         return await Update(entity,  true);
        }

        /// <summary>
        /// 通用的更新命令(update)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="showPop"></param>
        public static async Task<bool> Update<T>(T entity, bool showPop)
        {
            var type = typeof(T);
            var uploadType = type;
            if (type.IsGenericType)
            {
                uploadType = ReflectionHelper.GetGenericType(type);
            }
            var actionCmd = new ActionCommand();

            actionCmd.Entity.ActionObjectName = ReflectionHelper.GetTypeName(uploadType);
            actionCmd.Operator.ActionType = ActionTypeConst.insert;
            actionCmd.Entity.ActionObjectEntryStr = JsonHelper.JsonSerializer(entity);
//            var actionCmd = new ActionCmd
//            {
//                ActionObjectName = ReflectionHelper.GetTypeName(uploadType)//返回列表的实体类
//                ,
//                ActionType = ActionTypeConst.update//使用update 查询
//                ,
//                ActionObjectEntryStr = JsonHelper.JsonSerializer(entity)
//            };
            var re = await ActionRequest(actionCmd);
            return !re.HaveError;
        }

        #region 执行存储过程

        /// <summary>
        /// 使用存储过程返回所有的数据,在本地分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName">存储过程名称</param>
        /// <param name="executeMehod"></param>
        /// <param name="searchEntries">查询条件,可以为null</param>
        /// <param name="parametes"> </param>
        public static async Task<PagerListResult<T>> ExecSTSelectAll<T>(string storedProcedureName, Action<T> executeMehod, List<SearchEntry> searchEntries = null, List<STParamete> parametes = null)
        {
            Action<T, PageInfo> action = (list, info) => executeMehod(list);
            string where = SearchHelper.GetWhereStr(searchEntries);
            List<string> paramsName = new List<string>();
            List<string> paramsValue = new List<string>();
            if (@where != null)
            {
                paramsName.Add("@Where");
                paramsValue.Add(@where);
            }
            return await ExecSTSelect(storedProcedureName,
                            action, parametes);
        }
        /// <summary>
        /// 使用存储过程分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName"></param>
        /// <param name="searchEntries"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="executeMehod"></param>
        public static async Task<PagerListResult<T>> ExecSTSelectPaging<T>(string storedProcedureName, List<SearchEntry> searchEntries, string orderBy, int pageIndex, int pageSize, Action<T, PageInfo> executeMehod)
        {
            string where = SearchHelper.GetWhereStr(searchEntries);
            return await ExecSTSelect(storedProcedureName,
                    executeMehod, new List<STParamete> {
                        new STParamete("@PageSize", pageSize.ToString()),
                        new STParamete("@PageIndex", pageIndex.ToString()),
                        new STParamete("@Where", @where),
                        new STParamete("@OrderBy", orderBy),
                    });
        }

        public static async Task<PagerListResult<T>> ExecSTSelect<T>(string storedProcedureName, Action<T, PageInfo> executeMehod, List<STParamete> parametes = null, bool showPop = true)
        {
//            if (paramsName == null) paramsName = new List<string>();
//            if (paramsValue == null) paramsValue = new List<string>();
//            if (parametes != null)
//            {
//                parametes.ForEach(w =>
//                                      {
//                                          paramsName.Add(w.Name);
//                                          paramsValue.Add(w.Value);
//                                      });
//            }
            var actionCmd = new ActionCommand();

            actionCmd.Entity.ActionObjectName = ReflectionHelper.GetTypeName(typeof(T));
            actionCmd.Operator.ActionType = ActionTypeConst.storedProcedure;
//            actionCmd.Entity.ActionObjectEntryStr = JsonHelper.JsonSerializer(entity);
            actionCmd.Parameter.StoredProcedureParams = new StoredProcedureParams {
                StoredProcedureName = storedProcedureName,
                Params = parametes,
//                ParamsName = paramsName,
//                ParamsValue = paramsValue
            };
//            var actionCmd = new ActionCmd
//            {
//                ActionObjectName = ReflectionHelper.GetTypeName(typeof(T))//返回列表的实体类
//                ,
//                ActionType = ActionTypeConst.storedProcedure//使用select 查询
//                ,
//                StoredProcedureParams =
//                    new StoredProcedureParams
//                                            {
//                                                StoredProcedureName = storedProcedureName,
//                                                ParamsName = paramsName,//new List<string>{"@PageSize","@PageIndex","@Where","@OrderBy"},
//                                                ParamsValue = paramsValue//new List<string>{pageSize.ToString(),pageIndex.ToString(),where,orderBy}
//                                            }
//            ,
//            };
            var re = await ActionRequest(actionCmd);
            return re.ToPagerList<T>();
        }
        /// <summary>
        /// 查询存储过程,返回数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName"></param>
        /// <param name="entity"></param>
        /// <param name="showPop"></param>
        /// <returns></returns>
        public static async Task<ResultDataItem<T>> ExecSTCheck<T>(string storedProcedureName, T entity, List<STParamete> parametes, List<OutputValue> outputValues, bool showPop = true)
        {
            var actionCmd = new ActionCommand();

            actionCmd.Entity.ActionObjectName = ReflectionHelper.GetTypeName(typeof(T));
            actionCmd.Operator.ActionType = ActionTypeConst.storedProcedureCheck;
            actionCmd.Entity.ActionObjectEntryStr = JsonHelper.JsonSerializer(entity);
            actionCmd.Parameter.StoredProcedureParams = new StoredProcedureParams
            {
                StoredProcedureName = storedProcedureName,
                OutputValues = outputValues,
                Params = parametes,
            };
            ResultDataItem<T> re1 = new ResultDataItem<T>();
            var re = await ActionRequest(actionCmd);
            var list =  re.GetEntity<List<T>>();
            if (list != null && list.Count > 0)
            {
                re1.Entity = list[0];
            }
            else
            {
                re1.Entity = default(T);
            }
            ReflectionHelper.SetAttributeValues(re1,re);
            
            re1.OutputValues = re.OutputValues;
            return re1;
        }
        #endregion
//        public static async Task<PagerListResult<T>> Select<T>(List<SearchEntry> searchEntries, bool showPop = true, string[] black = null, string[] white = null)
//        {
//            return await Select<T>(searchEntries, showPop, black: black, white: white);
//        }
//        /// <summary>
//        /// 返回一个列表
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="searchEntries"></param>
//        /// <param name="executeMehod"></param>
//        /// <param name="showPop"></param>
//        public static async Task<PagerListResult<T>> Select<T>(List<SearchEntry> searchEntries, bool showPop = true, string[] black = null, string[] white = null)
//        {
//            return await Select(searchEntries, showPop, black: black, white: white);
//        }
        /// <summary>
        /// 返回一个列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchEntries"></param>
        /// <param name="showPop"></param>
        public static async Task<PagerListResult<T>> Select<T>(List<SearchEntry> searchEntries = null, bool showPop = true, string[] black = null, string[] white = null)
        {
            var re =  await Select<T>(searchEntries, null, 0, 100000,null,  black, white);
            return re;
        }
//        /// <summary>
//        /// 返回一个列表或一个实体
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="executeMehod"></param>
//        /// <param name="searchEntries"></param>
//        /// <param name="showPop"></param>
//        public static void Select<T>(List<SearchEntry> searchEntries = null, bool showPop = true, string[] black = null, string[] white = null)
//        {
//            if (!typeof(T).IsGenericType)
//            {
//                Select<List<T>>((list, page) =>
//                                                                 {
//                                                                     var re = default(T);
//                                                                     if (list.Count > 0)
//                                                                     {
//                                                                         re = list[0];
//                                                                     }
//                                                                     executeMehod(re);
//                                                                 }, searchEntries, showPop);
//            }
//            Select<T>(searchEntries, showPop, black: black, white: white);
//        }
        static List<SearchEntry> initSearch(List<SearchEntry> search)
        {
            //if (search != null)
            //{
            //    search.ForEach(
            //            w =>
            //                {
            //                    if (!w.value.IsNumber())
            //                    {
            //                        if (!(w.value.StartsWith("'") && w.value.EndsWith("'")))
            //                        {
            //                            w.value.Include("'");
            //                        }
            //                    }
            //                });
            //}
            return search;
        }

//        public static async Task<PagerListResult<T>> Select<T>(List<SearchEntry> searchEntries, string orderBy, int pageIndex, int pageSize,  WattingConfig showPop, string whereStr = null)
//        {
//            return await Select(searchEntries, orderBy, pageIndex, pageSize,  showPop);
//        }

        public static async Task<object> Select(Type type,List<SearchEntry> searchEntries, string orderBy, int pageIndex, int pageSize, string whereStr, string[] black, string[] white)
        {
            var method = typeof(ActionHelper).GetMethod("Select", new[] { typeof(List<SearchEntry>), typeof(string), typeof(int), typeof(int),  typeof(string), typeof(string[]), typeof(string[]) });
            var method1 = method.MakeGenericMethod(type);
            var re123 = method1.Invoke(null, new object[] { searchEntries, orderBy, pageIndex, pageSize,  whereStr, black, white }) as Task;
            var re = await ReflectionHelper.AwaitExecute(type,re123);
            return re;
        }
        public static async Task<PagerListResult<T>> Select<T>(List<SearchEntry> searchEntries, string orderBy, int pageIndex, int pageSize,  string whereStr, string[] black, string[] white)
        {
            var actionCmd = new ActionCommand();

            actionCmd.Entity.ActionObjectName = ReflectionHelper.GetTypeName(typeof(T));
            actionCmd.Operator.ActionType = ActionTypeConst.select;
            //            actionCmd.Entity.ActionObjectEntryStr = JsonHelper.JsonSerializer(entity);
            actionCmd.Parameter.SelectAcionParams = new SelectAcionParams {
                WhereStr = whereStr
                ,
                Search = initSearch(searchEntries) //搜索条件
                ,
                OrderBy = orderBy //排序
                ,
                PageInfo = new PageInfo //分页信息
                {
                    IsPaging = 1,
                    PageSize = pageSize,
                    PageIndex = pageIndex
                }
            };
            actionCmd.Parameter.FieldFilter = new FieldFilter {
                BlackNames = black,
                WhiteNames = white,
            };
//            var actionCmd = new ActionCmd
//            {
//                ActionObjectName = ReflectionHelper.GetTypeName(typeof(T))//返回列表的实体类
//                ,ActionType = "select"//使用select 查询
//                ,SelectAcionParams = new SelectAcionParams
//                {
//                    WhereStr = whereStr
//                    ,
//                    Search = initSearch(searchEntries)//搜索条件
//                    ,
//                    OrderBy = orderBy//排序
//                        ,
//                    PageInfo = new PageInfo//分页信息
//                    {
//                        IsPaging = 1,
//                        PageSize = pageSize,
//                        PageIndex = pageIndex
//                    }
//                },
//                FieldFilter = new FieldFilter
//                {
//                    BlackNames = black,
//                    WhiteNames = white,
//                }
//            };

            if (pageIndex == 0)
            {
                actionCmd.Parameter.SelectAcionParams.PageInfo = null;
            }
            var re = await ActionRequest(actionCmd);
            return re.ToPagerList<T>();
        }

//        /// <summary>
//        /// 通用查询(select)命令
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="searchEntries">搜索条件</param>
//        /// <param name="orderBy">排序</param>
//        /// <param name="pageIndex"></param>
//        /// <param name="pageSize"></param>
//        /// <param name="executeMehod"></param>
//        /// <param name="whereStr"></param>
//        [Obsolete]
//        public static async Task<PagerListResult<T>> Select<T>(List<SearchEntry> searchEntries, string orderBy, int pageIndex, int pageSize, bool showPop, string whereStr, string[] black, string[] white)
//        {
//            return await Select(searchEntries, orderBy, pageIndex, pageSize, new WattingConfig {Show = showPop}, whereStr, black, white);
//        }

    }
//    public interface IPopupable
//    {
//        void OpenFullWindowsShow(UIElement element, bool allowClose = true);
//
//        void OpenFullWindowsShow(UIElement element, List<Button> rightTopButtons, bool allowClose = true);
//        void CloseFullWindowsShow();
//        void PendantVisibility(Visibility pVisibility);//是否显示右侧挂件
//        void OpenWattingPopup();
//        void OpenWattingPopup(WattingConfig wc);
//
//        void OpenMessagePopup(MessageType messageType, string title, string message, Action<Result> action = null, ButtonContentType btnType = ButtonContentType.OkCancel);
//
//        void ClosePopup();
//    }
    /// <summary>
    /// 信息类型
    /// </summary>
    public enum MessageType
    {
        Infomation,
        Warning,
        Error,
    }
    /// <summary>
    /// 按钮类型
    /// </summary>
    public enum ButtonContentType
    {
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel,
    }
    public enum Result
    {
        Yes, No, None
    }
}
