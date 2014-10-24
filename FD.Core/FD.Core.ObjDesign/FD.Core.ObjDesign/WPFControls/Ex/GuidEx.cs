using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFControls.Ex
{
    public static class GuidEx
    {

        /// <summary>
        /// Guid+1
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static Guid Add(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                var b = bytes[i];
                if (b >= 255)
                {
                    bytes[i] = 0;
                }
                else
                {
                    bytes[i]++;
                    break;
                }
            }
            return new Guid(bytes);
        }

        /// <summary>
        /// Guid-1
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static Guid Sub(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                var b = bytes[i];
                if (b > 0)
                {
                    bytes[i]--;
                    break;
                }
                else
                {
                    bytes[i] = 255;
                }
            }
            return new Guid(bytes);
        }

        public static Guid ToGuid(this string guidStr)
        {
            Guid re;
            Guid.TryParse(guidStr, out re);
            return re;
        }
    }
}
