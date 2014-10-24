using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STComponse.GCode
{
    public class GProperty
    {
        public string TypeName { get; set; }
        public string PropertyName { get; set; }
        public string DefaultValue { get; set; }
    }

    public static class GPropertyEx
    {
        private const string Template = @"
        $TYPE$ $PRIVATENAME$;
        public $TYPE$ $PUBLICNAME$
        {
            get
            {
                return $PRIVATENAME$;
            }
            set
            {
                if (value != $PRIVATENAME$)
                {
                    $PRIVATENAME$ = value;
                    OnPropertySelectedChanged(() => $PUBLICNAME$);
                }
            }
        }";

        /// <summary>
        /// 返回当前属性的默认值表达式
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static string GetDefaultExpresstion(this GProperty g)
        {
            var defaultValue = g.DefaultValue;
            if (!string.IsNullOrEmpty(defaultValue))
            {
                if (g.TypeName == "string")
                {
                    defaultValue = string.Format("\"{0}\"",defaultValue.Replace("\\","\\\\").Replace("\r\n","\\r\\n").Replace("\"","\\\""));
                }
                return string.Format("{0} = {1};", g.GetPrivateName(), defaultValue);
            }
            return "";
        }
        public static string GetPrivateName(this GProperty g)
        {
            var privateName = "_" + g.PropertyName[0].ToString().ToLower() + g.PropertyName.Substring(1);
            return privateName;
        }

        /// <summary>
        /// 返回属性的代码
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static string ToCodeStr(this GProperty g)
        {
            try
            {
                return Template.Replace("$PUBLICNAME$", g.PropertyName)
                        .Replace("$TYPE$", g.TypeName)
                        .Replace("$PRIVATENAME$", g.GetPrivateName());
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
