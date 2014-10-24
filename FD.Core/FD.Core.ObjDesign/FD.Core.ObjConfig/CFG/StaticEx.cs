using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fastJSON;


namespace STComponse.CFG
{
    public static class StaticEx
    {
        public static string ToJson(this object str)
        {
            return JSON.Instance.ToJSON(str);
        }
        public static T ToObject<T>(this string str)
        {
            try
            {
                return JSON.Instance.ToObject<T>(str);
            }
            catch(Exception eee)
            {
                return default(T);
            }
        }
       
        public static T JsonClone<T>(this T t)
        {
            try
            {
                return t.ToJson().ToObject<T>();
            }
            catch (Exception eee)
            {
                return default(T);
            }
        }

        public static double? ToDouble(this string str,double? defaultValue = null)
        {
            double re;
            if (double.TryParse(str, out re))
            {
                return re;
            }
            return defaultValue;
        }

        public static DateTime? ToDate(this string str, DateTime? _default = null)
        {
            DateTime re;
            if (DateTime.TryParse(str, out re))
            {
                return re;
            }
            return _default;
        }
        public static int ToInt(this string str,int _default = 0)
        {
            int re;
            if (int.TryParse(str, out re))
            {
                return re;
            }
            return _default;
        }
        /// <summary>
        /// 序列化字符串列表
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="separator">分隔符</param>
        /// <param name="singleInclude">使用该字符把每个元素包含起来,可以为null或empty</param>
        /// <returns>序列化结果</returns>
        public static string Serialize1(this IEnumerable<string> source, string separator, string singleInclude = null)
        {
            if (singleInclude == "") singleInclude = null;
            if (singleInclude != null)
            {
                source = source.Select(w => singleInclude + w + singleInclude).ToList();
            }
            if (source != null && source.Count() >= 2)
            {
                return source.Aggregate((i, j) => i
                    + separator + j);
            }
            else if (source != null && source.Count() == 1)
            {
                return source.FirstOrDefault();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
