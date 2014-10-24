using System;
using System.Linq;
using System.Net;
using Fw.Extends;


namespace Fw.Net.IPExtends
{
    public static class IPExtendsMethod
    {
        /// <summary>
        /// 把192.168.1.1字符串,转成IP的Int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static uint ToIPInt(this string str)
        {
            IPAddress ip;
            if (IPAddress.TryParse(str, out ip))
            {
                var list = ip.GetAddressBytes();
                Array.Reverse(list);
                return BitConverter.ToUInt32(list, 0);
            }
            return 0;
        }
        /// <summary>
        /// 把IP的Int转成192.168.1.1字符串,
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToIPString(this uint str)
        {
            var bytes = BitConverter.GetBytes(str);
            Array.Reverse(bytes);
            return bytes.ToList().Select(w => w.ToString()).Serialize(".");
        }
        /// <summary>
        /// 返回指定的IP是否在当前区间之内
        /// </summary>
        /// <param name="content"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static bool CheckIPEnabled(this IPSection content, uint IP)
        {
            return IP >= content.Start && IP <= content.End;
        }
        /// <summary>
        /// 返回指定的IP是否在IP过虑之内
        /// </summary>
        /// <param name="content"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static bool CheckIPEnabled(this IPFilterConfig content, string IP)
        {
            var ipint = IP.ToIPInt();
            if (ipint != 0)
            {
                if (content.White != null && content.White.Any(w => w.CheckIPEnabled(ipint)))
                {
                    return true;
                }
            }
            return false;
        }
    }
}