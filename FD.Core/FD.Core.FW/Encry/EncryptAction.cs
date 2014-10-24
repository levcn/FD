using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fw.Encry
{
    public static class EncryptAction
    {
        public static string GetEncryStr(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str).Encry();
            return Convert.ToBase64String(bytes);
        }
        public static string GetDencryStr(string str)
        {
            var bytes = Convert.FromBase64String(str).Dencry();
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Encry(this Byte[] bytes)
        {
            return bytes.ToList().Select(w => (byte) ~w).ToArray();
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Dencry(this Byte[] bytes)
        {
            return bytes.ToList().Select(w => (byte)~w).ToArray();
        }
    }
}
