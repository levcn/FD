using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using StaffTrain.FwClass.Serializer;


namespace SLControls.Extends
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
        /// 
        /// </summary>
        /// <param name="strOriginal">strOriginal 原始字符串</param>
        /// <param name="maxTrueLength">maxTrueLength 需要返回的字符串的字节长度</param>
        /// <param name="chrPad">chrPad 字符串不够时的填充字符</param>
        /// <param name="blnCutTail">blnCutTail 字符串的字节长度超过maxTrueLength时是否截断多余字符</param>
        /// <returns>return 返回填充或截断后的字符串</returns>
        static public string LeftRealLength(this string strOriginal, int maxTrueLength, char? chrPad = null, bool blnCutTail = true)
        {
            string strNew = strOriginal;

            if (strOriginal == null || maxTrueLength <= 0)
            {
                strNew = "";
                return strNew;
            }
            int trueLen = strOriginal.RealLength();
            if (trueLen > maxTrueLength)//超过maxTrueLength
            {
                if (blnCutTail)//截断
                {
                    for (var i = strOriginal.Length - 1; i > 0; i--)
                    {
                        strNew = strNew.Substring(0, i);
                        if (strNew.RealLength() <= maxTrueLength)
                            break;
                    }
                }
            }
            else//填充
            {
                for (int i = 0; i < maxTrueLength - trueLen; i++)
                {
                    strNew += chrPad.ToString();
                }
            }

            return strNew;
        }
        /// <summary>
        /// 返回/TestModule;component/
        /// </summary>
        /// <param name="thisType"></param>
        /// <returns></returns>
        public static string GetModuleUriPrefix(this Type thisType)
        {
            return String.Format("/{0};component/", thisType.GetNamesapce());
        }
        public static string GetNamesapce(this Type thisType)
        {
            return thisType.Assembly.GetNamesapce();
        }
        /// <summary>
        /// 返回命名空间
        /// </summary>
        /// <param name="thisAssembly"></param>
        /// <returns></returns>
        public static string GetNamesapce(this Assembly thisAssembly)
        {
            return thisAssembly.ManifestModule.ToString().Replace(".dll", "");
        }
        #region 字符串相关扩展方法

        public static Color ToColor(this string colorName)
        {
            if (colorName.StartsWith("#"))
                colorName = colorName.Replace("#", string.Empty);
            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
            return new Color()
            {
                A = Convert.ToByte((v >> 24) & 255),
                R = Convert.ToByte((v >> 16) & 255),
                G = Convert.ToByte((v >> 8) & 255),
                B = Convert.ToByte((v >> 0) & 255)
            };
        }
        public static bool IsNumber(this string content)
        {
            try
            {
                Convert.ToDouble(content);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static double ToDouble(this string content)
        {
            try
            {
                return Convert.ToDouble(content);
            }
            catch
            {
                return 0;
            }
        }
        public static int ToInt(this string content)
        {
            try
            {
                return Convert.ToInt32(content);
            }
            catch
            {
                return 0;
            }
        }
        public static long ToLong(this string content)
        {
            try
            {
                return Convert.ToInt64(content);
            }
            catch
            {
                return 0;
            }
        }
        public static byte[] ToByte(this string source)
        {
            return Encoding.UTF8.GetBytes(source);
        }

        public static string ToStr(this byte[] source)
        {
            return Encoding.UTF8.GetString(source, 0, source.Length);
        }

        public static string ToBase64(this string content, bool encrypt = true)
        {
            return Convert.ToBase64String(content.ToByte());
        }

        public static T FromJson<T>(this string content)
        {
            return JsonHelper.JsonDeserialize<T>(content);
        }
        public static string FromBase64(this string content, bool encrypt = true)
        {
            if (string.IsNullOrEmpty(content)) return "";
            try
            {
                return Convert.FromBase64String(content).ToStr();
            }
            catch
            {
                return "";
            }
        }
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
        public static string SqlFilte(this string value)
        {
            return value.Replace("'", "''");
        }

        /// <summary>
        /// 返回是否相同,不分大小写
        /// </summary>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool EqualsAny(this string source, string other)
        {
            if (source == null)
            {
                return other == null;
            }
            return source.Equals(other, StringComparison.CurrentCultureIgnoreCase);
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
        public static string SubRight(this string owner, int len)
        {
            if (owner != null && owner.Length >= len)
            {
                return owner.Substring(owner.Length - len);
            }
            return "";
        }
        public static List<string> ToSplitList(this string owner)
        {
            return owner.Split(',', '|').Where(w => !string.IsNullOrEmpty(w)).ToList();
        }
        public static byte[] UTF8Bytes(this string source)
        {
            return Encoding.UTF8.GetBytes(source);
        }
        public static string UTF8String(this byte[] source)
        {
            try
            {
                return source == null ? "" : Encoding.UTF8.GetString(source, 0, source.Length);
            }
            catch
            {
                return "";
            }
        }
        public static string UTF8String(this byte[] source, out bool re)
        {
            try
            {
                re = true;
                return source == null ? "" : Encoding.UTF8.GetString(source, 0, source.Length);
            }
            catch
            {
                re = false;
                return "";
            }
        }
        /// <summary>
        /// 返回字符最后面的数据 EG:新文件(1) ,返回1
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int GetMaxNumber(this string content)
        {
            var match = Regex.Match(content, @"[\s\S]*\((?<number>[0-9]+)\)");
            if (match.Success)
            {
                return match.Groups["number"].Value.ToInt();
            }
            return 0;
        }
        //        public static string Left(this string source,int len)
        //        {
        //            return Convert.ToString(source) ?? String.Empty;
        //        }
        /// <summary>
        /// 将指定对象的值转换为其等效的字符串表示形式。
        /// </summary>
        /// <param name="source">源：一个对象，用于提供要转换的值，或 null。</param>
        /// <returns>source 的字符串表示形式，如果 source 为 null，则为 System.String.Empty。</returns>
        public static string ToStringAndInclude(this object source, string str = "")
        {
            return source.ToString().Include(str);
        }
        public static string Left(this string source, int count)
        {
            if (source.Length <= count) return source;
            return source.Substring(0, count);
        }
        public static string Include(this string source, string chars1, string chars2 = null)
        {
            if (chars2 == null) chars2 = chars1;
            return chars1 + source + chars2;
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

        #endregion

        #region 颜色


        #endregion
    }
}
