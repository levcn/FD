using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Fw.Net.IPExtends
{
    /// <summary>
    /// IP过虑设置
    /// </summary>
    public class IPFilterConfig
    {
        public IPFilterConfig()
        {
            Black = new List<IPSection>();
            White = new List<IPSection>();
        }
        /// <summary>
        /// 黑名单
        /// </summary>
        public List<IPSection> Black { get; set; }

        /// <summary>
        /// 白名单
        /// </summary>
        public List<IPSection> White { get; set; }
    }
}
