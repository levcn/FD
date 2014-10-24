using System.IO;
using System.Linq;
using System.Web;
using Fw.Extends;
using Fw.Net.IPExtends;
using Fw.Serializer;


namespace Fw.Encry.SN
{
    public static class MainClass
    {
        public static void CheckIP(HttpContext context)
        {
            var ip = context.Request.ServerVariables["REMOTE_ADDR"];
            if (ip == "::1") ip = "127.0.0.1";
            if (CurrentIPFilterConfig == null || !CurrentIPFilterConfig.CheckIPEnabled(ip))
            {
                context.Response.Write(ip + "访问被拒绝(IP不在指定范围之内).");
                if (CurrentIPFilterConfig != null) context.Response.Write("<br>可用IP:" + CurrentIPFilterConfig.White.Select(w=>w.StringStart + " " + w.StringEnd));
                context.Response.End();
            }
        }

        private static IPFilterConfig currentIPFilterConfig;
        public static IPFilterConfig CurrentIPFilterConfig
        {
            get
            {
                if (currentIPFilterConfig == null)
                {
                    if (File.Exists(IPFilterPath))
                    {
                        try
                        {
                            var text = File.ReadAllText(IPFilterPath);
                            currentIPFilterConfig =
                                    XmlHelper.GetXmlDeserialize<IPFilterConfig>(EncryptAction.GetDencryStr(text));
                        }
                        catch
                        {

                        }
                    }
                }
                return currentIPFilterConfig;
            }
        }
        /// <summary>
        /// 过虑文件的文件地址
        /// </summary>
        public static string IPFilterPath
        {
            get
            {
                return Fw.AppSetting.IPFilterPath;
                //return Path.Combine(Fw.AppSetting.SiteRoot, AppSetting.IPFilterFileName);
            }
        }
    }
}