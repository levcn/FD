using System;
using System.Collections.Generic;


namespace Fw.Encry.SN
{
    public class Lis
    {
        /// <summary>
        /// 过期时间,如果为空,则是不过期的正式版本
        /// </summary>
        public DateTime? OverDate { get; set; }

        /// <summary>
        /// 最后一次使用些系统的时间(如果)
        /// </summary>
        public DateTime? LastUseDate { get; set; }

        /// <summary>
        /// 服务器时间是否错误(如果使用时间小于最后使用时间,则为true)
        /// </summary>
        public bool DateTimeError { get; set; }

        /// <summary>
        /// CPUID
        /// </summary>
        public string CPUID { get; set; }

        public List<string> Keys { get; set; }
    }
}