using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using FD.Core.Test.Entity;
using Fw;
using Fw.DataAccess;
using Fw.Reflection;
using Fw.Serializer;
using Fw.Web;
using Fw.WindowsSystem;
using STComponse.CFG;


namespace FD.Core.Test.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ObservableCollection<string> r = new ObservableCollection<string>();
            
            InitVersion();
            var web = Assembly.GetExecutingAssembly().FullName;
            var fw = typeof(ReflectionHelper).Assembly.FullName;
            DataAccessFactory.EntityAssembly = typeof(SYS_User).Assembly;
            var fw1 = typeof(SYS_User).Assembly.FullName;
            //            var ere = ReflectionHelper.GetTypeByTypeAndName<BaseController>(rere123, "PageAction");
            ReflectionHelper.AddAssemblyName(web);
            ReflectionHelper.AddAssemblyName(fw);
            ReflectionHelper.AddAssemblyName(fw1);
            //            ReflectionHelper.DefaultAssemblyName = GetType().Assembly.FullName;
            AppSetting.SiteRoot = Server.MapPath("~/");
            AppSetting.IPFilterPath = Path.Combine(Fw.AppSetting.SiteRoot, AppSetting.IPFilterFileName);
            AppSetting.LisFileFullPath = Path.Combine(Fw.AppSetting.SiteRoot, AppSetting.LisFileName);
            AppSetting.CPUID = WSystemInfo.GetCPUID();
            AppSetting.DBConnection = DataAccessFactory.CheckDBConnection();
            DataAccessFactory.FwConfig = JsonHelper.FastJsonDeserialize<FwConfig>(File.ReadAllText(Path.Combine(AppSetting.SiteRoot, "2.cfg")));
        }
        private void InitVersion()
        {
            AppSetting.ProductVersion = new ProductVersion();
            AppSetting.ProductVersion.Code = WebConfigs.GetConfig("VersionCode");
            AppSetting.ProductVersion.Name = WebConfigs.GetConfig("VersionName");
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}