using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Controls;
//using System.Windows.Media;
using STComponse.CFG;


namespace STComponse.GCode
{
    public static class GClassEx
    {
        /// <summary>
        /// 返回类的代码
        /// </summary>
        /// <param name="gClass"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string ToCodeStr(this GClass gClass, string template)
        {
            template = template.Replace("$CLASSNAME$", gClass.ClassName);
            template = template.Replace("$CONSTRUCT$", GetConstruct(gClass));
            var pStr = gClass.Properties.Select(w => w.ToCodeStr()).Serialize1("\r\n");
            template = template.Replace("$RPOPERTIES$", pStr);
            return template;
        }

        private static string GetConstruct(GClass gClass)
        {
            var re = @"public {0}(){{
{1}
}}";
            var defaultValue = GetDefaultValue(gClass);
            return string.Format(re, gClass.ClassName, defaultValue);
        }

        /// <summary>
        /// 返回类的默认值
        /// </summary>
        /// <param name="gClass"></param>
        /// <returns></returns>
        private static string GetDefaultValue(GClass gClass)
        {
            return gClass.Properties.Select(w=>w.GetDefaultExpresstion()).Where(w=>!string.IsNullOrEmpty(w)).Serialize1("\r\n");
        }
    }
    public class GClass
    {
        public string ClassName { get; set; }
        public List<GProperty> Properties { get; set; }
    }
}
