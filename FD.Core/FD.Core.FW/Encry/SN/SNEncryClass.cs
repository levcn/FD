using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fw.Extends;
using Fw.Net.IPExtends;
using Fw.Serializer;
using Fw.WindowsSystem;


namespace Fw.Encry.SN
{
    /// <summary>
    /// 加密认证类
    /// </summary>
    public class SNEncryClass
    {
        private static string Strs = "0123456789abcdefghijklmnopqrstuvwxyz";
        static int[] AddNumber = new int[] { 2, 4, 8, 2, 7, 2, 6, 1, 4, 5, 4, 3, 4 };
        static Random Ran = new Random();
        public static string GetEncrySN(TimeSpan ts)
        {
            return GetEncrySN(DateTime.Now.Add(ts));
        }
        /// <summary>
        /// 返回当前时间之后指定时间的加密字符串
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string GetEncrySN(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                date = new DateTime(2000,1,1);
            }
            var add = Ran.Next(1, 10);
            var str = date.ToString("yyMMddHHmmss");
            var adds = AddNumber.ToList().Select(w => w + add).ToList();
            int index = 1;
            var re = new string(str.Select(w => GetChar1(Strs, adds, w, index++)).ToArray()) + add;
            return re.ToUpper();
        }
        /// <summary>
        /// 把加密字符串解密转成日期
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static DateTime GetDencrySNDate(string p)
        {
            var str = GetDencrySN(p);
            var re = Enumerable.Range(1, 7)
                    .Select(w => new string(str.Take(w * 2).ToArray()))
                    .Where(w => w.Length >= 2)
                    .Select(w => Convert.ToInt32(w.Substring(w.Length - 2, 2)))
                    .ToList();
            var re1 = Enumerable.Range(0, 6).Select(w => w < re.Count ? re[w] : 0).ToList();
            var date = new DateTime(re1[0] + 2000, re[1], re[2], re[3], re[4], re[5]);
            if (date == new DateTime(2000, 1, 1))
            {
                date = DateTime.MinValue;
            }
            return date;
        }
        /// <summary>
        /// 从strs中找到w的位置,并加上index,返回该位置的字符
        /// </summary>
        /// <param name="Strs"></param>
        /// <param name="adds"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        private static char GetChar1(string Strs, List<int> adds, char w, int index)
        {
            var a = adds[index] + Strs.IndexOf(w);
            return Strs[a % Strs.Length];
        }
        private static char GetChar2(string Strs, List<int> adds, char w, int index)
        {
            //var a = Strs.IndexOf(w);
            //var index1 = a - adds[index];
            //return Strs[index1];
            return Strs[Enumerable.Range(0, 5)
                         .Select(q => q * Strs.Length)
                         .TakeWhile(q => q < Strs.Length)
                         .Select(q => Strs.IndexOf(w) + q - adds[index])
                         .TakeWhile(q => q < Strs.Length && q >= 0).First()];
        }

        internal static string GetDencrySN(string p)
        {
            var v = p.ToLower().Take(12).ToList();
            var add = Convert.ToInt32(p[12].ToString());
            var adds = AddNumber.ToList().Select(w => w + add).ToList();
            int index = 1;
            var re = new string(v.Select(w => GetChar2(Strs, adds, w, index++)).ToArray());
            return re;
        }
        

        #region 字符串加密

        public static byte[] EncryStr(byte[] str)
        {
            return str.ToList().Select(w => (byte)~w).ToArray();
        }

        public static byte[] DecryStr(byte[] str)
        {
            return str.ToList().Select(w => (byte)~w).ToArray();
        }

        #endregion

        #region 文件管理

        /// <summary>
        /// 更新key(1更新为正式版本,2更新试用为试用版本,3注册码已经使用过,)
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="key"></param>
        /// <returns>(0错误,1更新为正式版本,2更新试用为试用版本,3注册码已经使用过,)</returns>
        public static string UpdateLis(string fullPath, string key)
        {
            var re = "";
            Lis lis = GetLis(fullPath);
            if(lis ==null)lis = new Lis();
            if(lis.Keys == null)lis.Keys = new List<string>();
            if (lis.Keys.Contains(key)) return "3";
            lis.Keys.Add(key);
            bool haveError = false;
            try
            {
                var date = GetDencrySNDate(key);
            if (date == DateTime.MinValue)
            {
                re = "1";
                lis.OverDate = null;
                }
                else
                {
                    re = "2";
                    lis.OverDate = date;
                }
            }
            catch (Exception)
            {
                haveError = true;
                re = "0";
            }
            if (!haveError) SaveLisFile(fullPath, lis);
            return re;
        }
        /// <summary>
        /// 把认证信息加密并保存到文件
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="lis"></param>
        public static void SaveLisFile(string fullPath,Lis lis)
        {
            lis.CPUID = WSystemInfo.GetCPUID();
            EncryStr(XmlHelper.GetXmlSerialize(lis).ToByte()).WriteToFile(fullPath);
        }
        /// <summary>
        /// 把认证信息加密并保存到文件
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="lis"></param>
        public static void SaveIPFile(string fullPath, IPFilterConfig lis)
        {
            EncryptAction.GetEncryStr(XmlHelper.GetXmlSerialize(lis)).ToByte().WriteToFile(fullPath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="lis"></param>
        public static IPFilterConfig GetIPFile(string fullPath)
        {
            return XmlHelper.GetXmlDeserialize<IPFilterConfig>(EncryptAction.GetDencryStr(File.ReadAllText(fullPath)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static double GetFreeUseDay(string fullPath)
        {
            var lis = GetLis(fullPath);
            if(lis!=null)
            {
                if(lis.OverDate == null)//正式版本
                {
                    return -1;
                }
                else
                {
                    if(DateTime.Now < lis.OverDate.Value)//如果在试用期内
                    {
                        var ts = DateTime.Now - lis.OverDate.Value;
                        return ts.TotalDays;
                    }
                    else//如果试用期结束
                    {
                        return 0;
                    }
                }
            }
            return 0;//如果没有找到授权信息,则退出
        }
        /// <summary>
        /// 从文件中读取认证信息,并解密
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static Lis GetLis(string fullPath)
        {
            try
            {
                var bytes = File.ReadAllBytes(fullPath);
                return XmlHelper.GetXmlDeserialize<Lis>(DecryStr(bytes).ToStr());
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}