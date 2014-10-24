//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web;
//using Fw;
//using Fw.DataAccess;
//using Fw.Entity;
//using Fw.Extends;
//using Fw.Reflection;
//using Fw.Serializer;
//
//
//namespace ServerFw.Web
//{
//    public class BaseActionHandle: IHttpHandler
//    {
//        public void ProcessRequest(HttpContext context)
//        {
//            if (AppSetting.SiteRoot == string.Empty) 
//                AppSetting.SiteRoot = HttpContext.Current.Server.MapPath("~/");
//            RunAction(context);
//        }
//        /// <summary>
//        /// 执行动作
//        /// </summary>
//        /// <param name="context"></param>
//        private void RunAction(HttpContext context)
//        {
//            var text = context.Request.Form["ActionCmd"];
//            var cmd = (ActionCmd)JsonHelper.JsonDeserialize(text, typeof(ActionCmd));
//            try
//            {
////                System.Environment.use
//                ResultData re;
//                if (cmd.IsCustomCmd)//执行指定的方法
//                {
//                    re = ExecuteCustomMethod(cmd.CustomCmd);
//                }
//                else//执行数据查询
//                {
//                    re = DataAccessFactory.ExucetActionCmd(cmd);
//                }
//                var jsonSerializer = JsonHelper.JsonSerializer(re);
//                var ere= JsonHelper.JsonDeserialize(jsonSerializer, typeof (ResultData));
//                context.Response.Write(jsonSerializer);
//            }
//            catch (Exception ee)
//            {
//                var detailErrorMsg = ee.GetBaseException().ToString();
//                var rd = new ResultData { HaveError = true, ErrorMsg = ee.Message, DetailErrorMsg = detailErrorMsg };
//                context.Response.Write(JsonHelper.JsonSerializer(rd));
//            }
//
//            context.Response.End();
//        }
//        /// <summary>
//        /// 执行自定义命令
//        /// </summary>
//        /// <param name="customCmd"></param>
//        /// <returns></returns>
//        private ResultData ExecuteCustomMethod(CustomCmd customCmd)
//        {
//            var rd = new ResultData();
//            var obj =ReflectionHelper.ExecuteMethod(customCmd.ClassName, customCmd.MethodName, customCmd.Params);
//            if (obj != null) rd.ObjectEntryStr = JsonHelper.JsonSerializer(obj);
//            return rd;
//            //            return JsonHelper.JsonSerializer(rd);
//        }
//        public bool IsReusable
//        {
//            get
//            {
//                return false;
//            }
//        }
//    }
//}
