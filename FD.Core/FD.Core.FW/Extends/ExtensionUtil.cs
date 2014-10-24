using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Fw.Serializer;


namespace Fw.Extends
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionUtil
    {
        /// <summary>
        /// 返回字符串的显示长度(双字节字符为2,单字节字符为1)
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static int RealLength(this string owner)
        {
            // str 字符串
            // return 字符串的字节长度
            int lenTotal = 0;
            int n = owner.Length;
            string strWord = "";
            int asc;
            for (int i = 0; i < n; i++)
            {
                strWord = owner.Substring(i, 1);
                asc = Convert.ToChar(strWord);
                if (asc < 0 || asc > 127)
                    lenTotal = lenTotal + 2;
                else
                    lenTotal = lenTotal + 1;
            }
            return lenTotal;
        }
        /// <summary>
        /// Xml序列化 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ToXml(this object content)
        {
            return XmlHelper.GetXmlSerialize(content);
        }
        /// <summary>
        /// json序列化 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ToJson(this object content)
        {
            if (content == null) return null;
            return JsonHelper.FastJsonSerializer(content);
        }
        /// <summary>
        /// json序列化 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string content) where T : class
        {
            if (content == null) return default(T);
            return JsonHelper.FastJsonDeserialize<T>(content);
        }

        /// <summary>
        /// 转换int
        /// </summary>
        /// <param name="content"></param>
        /// <param name="min">最小值 </param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int ToInt(this string content, int? defaultValue = 0, int? min = null)
        {
            min = min ?? 0;
            var re = 0;
            try
            {
                re = Convert.ToInt32(content);
            }
            catch
            {
                re = defaultValue ?? 0;
            }
            if (re < min.Value) re = min.Value;
            return re;
        }
        public static long ToLong(this string content, long? defaultValue = 0, long? min = null)
        {
            min = min ?? 0;
            long re;
            try
            {
                re = Convert.ToInt64(content);
            }
            catch
            {
                re = defaultValue ?? 0;
            }
            if (re < min.Value) re = min.Value;
            return re;
        }
        public static ulong ToULong(this string content, ulong? defaultValue = 0, ulong? min = null)
        {
            min = min ?? 0;
            ulong re;
            try
            {
                re = Convert.ToUInt64(content);
            }
            catch
            {
                re = defaultValue ?? 0;
            }
            if (re < min.Value) re = min.Value;
            return re;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T FromXml<T>(this string content) where T : class
        {
            return XmlHelper.GetXmlDeserialize<T>(content);
        }
        #region 字符串相关扩展方法


        //        public static int ToInt(this string str,int defaultValue = 0)
        //        {
        //            try
        //            {
        //                return Convert.ToInt32(str);
        //            }
        //            catch
        //            {
        //                return defaultValue;
        //            }
        //            
        //        }
        /// <summary>
        /// 指示指定的字符串是 null 还是 System.String.Empty 字符串。
        /// </summary>
        /// <param name="source">要测试的字符串。</param>
        /// <param name="value"> </param>
        /// <returns>如果 value 参数为 null 或空字符串 ("")，则为 true；否则为 false。</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }
        public static string FilterSql(this string value)
        {
            return value.Replace("'", "''");
        }

        public static string Left(this string source, int length)
        {
            if (source == null) return "";
            if (source.Length < length) return source;
            return source.Substring(0, length);
        }
        /// <summary>
        /// 将指定对象的值转换为其等效的字符串表示形式。
        /// </summary>
        /// <param name="source">源：一个对象，用于提供要转换的值，或 null。</param>
        /// <returns>source 的字符串表示形式，如果 source 为 null，则为 System.String.Empty。</returns>
        public static string ToString2(this object source)
        {
            return Convert.ToString(source) ?? String.Empty;
        }
        /// <summary>
        /// #00FF00
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Color ToColor(this string source)
        {
            return ColorTranslator.FromHtml(source);
        }
        public static string Fmt(this string source, params object[] objs)
        {
            if (objs == null || objs.Length == 0) return source;
            return string.Format(source, objs);
        }
        public static Guid ToGuid(this string source)
        {
            Guid g;
            if (Guid.TryParse(source, out g))
            {
                return g;
            }
            return Guid.Empty;
        }
        public static byte[] ToByte(this string source)
        {
            return Encoding.UTF8.GetBytes(source);
        }
        public static string ToBase64(this string content, bool encrypt = true)
        {
            return Convert.ToBase64String(content.ToByte());
        }
        public static string FromBase64(this string content, bool encrypt = true)
        {
            return Convert.FromBase64String(content).ToStr();
        }
        public static string ToStr(this byte[] source)
        {
            return Encoding.UTF8.GetString(source);
        }
        public static void WriteToFile(this byte[] source, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                fs.Write(source, 0, source.Length);
                fs.SetLength(source.Length);
                fs.Flush();
            }
        }
        public static string IncludeAndFilter(this string source, string chars)
        {
            return source.FilterSql().Include(chars);
        }
        public static string Include(this string source, string chars)
        {
            return chars + source + chars;
        }
        /// <summary>
        /// 序列化字符串列表
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="separator">分隔符</param>
        /// <param name="singleInclude">使用该字符把每个元素包含起来,可以为null或empty</param>
        /// <returns>序列化结果</returns>
        public static string Serialize(this IEnumerable<string> source, string separator, string singleInclude = null)
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
        /// <summary>
        /// 序列化字符串列表
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="separator">分隔符</param>
        /// <returns>序列化结果</returns>
        public static string Serialize(this IEnumerable<Guid> source, string separator)
        {
            return source.Select(w => w.ToString()).Serialize(separator);
        }
        #endregion

        #region TimeSpan

        /// <summary>
        /// 返回汉字式时间间隔
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static string ToString2(this TimeSpan timeSpan)
        {
            string returnValue = "";
            if (timeSpan.Days > 0)
            {
                returnValue += String.Format("{0}天", timeSpan.Days);
            }
            if (timeSpan.Hours > 0)
            {
                returnValue += String.Format("{0}小时", timeSpan.Hours);
            }
            if (timeSpan.Minutes > 0)
            {
                returnValue += String.Format("{0}分", timeSpan.Minutes);
            }
            if (timeSpan.Seconds > 0)
            {
                returnValue += String.Format("{0}秒", timeSpan.Seconds);
            }
            if (timeSpan.Milliseconds > 0)
            {
                returnValue += String.Format("{0}毫秒", timeSpan.Milliseconds);
            }
            return returnValue;
        }

        /// <summary>
        /// 返回延迟的颜色,从绿到黄到红
        /// </summary>
        /// <param name="timeSpan">延迟(计算方式为秒)</param>
        /// <param name="min">绿的阀值</param>
        /// <param name="max">红的阀值</param>
        /// <returns></returns>
        public static Color GetDelayColor(this TimeSpan timeSpan, int min, int max)
        {
            Color color = Color.Fuchsia;
            var percent = -1M;
            var totalMilliseconds = timeSpan.TotalMilliseconds;
            if (totalMilliseconds >= max) percent = 100;
            if (totalMilliseconds <= min) percent = 0;
            if (percent == -1)
            {
                totalMilliseconds -= min;
                max -= min;
                min = 0;
                percent = (decimal)((totalMilliseconds / max) * 100);
            }
            if (percent > 0 && percent <= 50)
            {
                color = Color.FromArgb((int)percent * 5, 255, 0);
            }
            else if (percent > 50 && percent <= 100)
            {
                percent -= 50;
                color = Color.FromArgb(255, 255 - (int)percent * 5, 0);
            }
            color = color.Lower(30);
            return color;
        }
        /// <summary>
        /// percent = 1-100
        /// 增加颜色深度,简易算法
        /// </summary>
        /// <param name="color"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static Color Lower(this Color color, int percent)
        {
            decimal p = percent / 100M;
            byte r = color.R;
            byte b = color.B;
            byte g = color.G;
            r = (byte)(r * (1 - p));
            b = (byte)(b * (1 - p));
            g = (byte)(g * (1 - p));
            Func<byte, byte> col = a =>
            {
                if (a < 0) a = 0;
                if (a > 255) a = 255;
                return a;
            };
            r = col(r);
            b = col(b);
            g = col(g);
            return Color.FromArgb(r, g, b);
        }
        #endregion

        #region 颜色

        /// <summary>
        /// 颜色转成HTML格式
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ToHtml(this Color color)
        {
            return ColorTranslator.ToHtml(color);
        }

        #endregion
    }
    public static class StringBuilderEx
    {
        public static void AppendLineFormat(this StringBuilder sb, string str, params object[] objs)
        {
            try
            {
                if (objs != null && objs.Length > 0) sb.AppendFormat(str, objs);
                else
                {
                    sb.Append(str);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + str);
            }
            sb.AppendLine();
        }
    }

    public static class DataTableExtensions
    {

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult">类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<TResult> ToList<TResult>(this DataTable dt) where TResult : class,new()
        {
            //创建一个属性的列表
            var prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            Type t = typeof(TResult);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表 
            Array.ForEach(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //创建返回的集合
            var oblist = new List<TResult>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                var ob = new TResult();
                //找到对应的数据  并赋值
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }
    }
}
