using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STComponse.ObjectConfig
{

    /// <summary>
    /// 数据对象
    /// </summary>
    public class SObject
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string Code{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Comment { get; set; }

        public List<SProperty> Properties { get; set; }
    }
}
