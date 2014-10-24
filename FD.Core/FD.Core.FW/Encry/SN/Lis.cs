using System;
using System.Collections.Generic;


namespace Fw.Encry.SN
{
    public class Lis
    {
        /// <summary>
        /// ����ʱ��,���Ϊ��,���ǲ����ڵ���ʽ�汾
        /// </summary>
        public DateTime? OverDate { get; set; }

        /// <summary>
        /// ���һ��ʹ��Щϵͳ��ʱ��(���)
        /// </summary>
        public DateTime? LastUseDate { get; set; }

        /// <summary>
        /// ������ʱ���Ƿ����(���ʹ��ʱ��С�����ʹ��ʱ��,��Ϊtrue)
        /// </summary>
        public bool DateTimeError { get; set; }

        /// <summary>
        /// CPUID
        /// </summary>
        public string CPUID { get; set; }

        public List<string> Keys { get; set; }
    }
}