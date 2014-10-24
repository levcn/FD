using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using Fw.DataAccess;
using Fw.Entity;
using Fw.Extends;
using Fw.Reflection;
using Fw.Serializer;
using StaffTrain.SVFw.Helpers;


namespace SComm.ActionBase
{
    /// <summary>
    /// Action/ActionHandle.ashx
    /// </summary>
    public class BaseActionHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (string.IsNullOrEmpty(Setting.SiteRoot))
                Setting.SiteRoot = HttpContext.Current.Server.MapPath("~/");
            {
                RunAction(context);
            }

        }

        static object ooo = new object();
        /// <summary>
        /// 执行动作
        /// </summary> 
        /// <param name="context"></param>
        private void RunAction(HttpContext context)
        {
            var text = context.Request["ActionCmd"];
            List<ActionCommand> r = new List<ActionCommand>();
            if (!text.IsNullOrEmpty())
            {
                if (text[0] == '[')
                {
                    r = text.FromJson<List<ActionCommand>>();
                }
                else if (text[0] == '{')
                {
                    var cmd = (ActionCommand)JsonHelper.FastJsonDeserialize(text, typeof(ActionCommand));
                    r.Add(cmd);
                }
                List<ResultData> result = new List<ResultData>();
                foreach (ActionCommand cmd in r)
                {
                    ResultData re;
                    try
                    {
                        //                using (new AutoStopwatch("dddd"))
                        {

                            if (cmd.Operator.IsCustomCmd) //执行指定的方法
                            {
                                re = ExecuteCustomMethod(cmd.Operator.CustomCmd);
                            }
                            else //执行数据查询
                            {
                                if (cmd.Operator.Version == 2)
                                {
                                    re = DataAccessFactory.ExucetActionCmdV2(cmd);
                                }
                                else
                                {
                                    re = DataAccessFactory.ExucetActionCmd(cmd);
                                    
                                }
                            }
                            //                    JsonHelper.FastJsonSerializer
                            //                            context.Response.Write(re);
                        }
                    }
                    catch (Exception ee)
                    {
                        var detailErrorMsg = ee.GetBaseException().ToString();
                        re = new ResultData { HaveError = true, ErrorMsg = ee.Message, DetailErrorMsg = detailErrorMsg };
                        ExceptionHelper.ExceptionOper(ee, re);
                    }
                    result.Add(re);
                }
                //如果只有一个,兼容老版本,只发送一个
                context.Response.Write(result.Count == 1 ? result[0].ToJson() : result.ToJson());
            }
            context.Response.End();
        }
        /// <summary>
        /// 执行自定义命令
        /// </summary>
        /// <param name="customCmd"></param>
        /// <returns></returns>
        private ResultData ExecuteCustomMethod(CustomCmd customCmd)
        {
            
            var rd = new ResultData();
            var obj = ReflectionHelper.ExecuteClientMethod(customCmd.ClassName, customCmd.MethodName, customCmd.Params);
            if (obj != null)
            {
                //                if (obj is string)
                //                {
                //                    rd.ObjectEntryStr = obj.ToString();
                //                }
                //                else
                {
                    rd.ObjectEntryStr = JsonHelper.FastJsonSerializer(obj);
                }
            }
            return (rd);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
