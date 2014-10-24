using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCreater.Settings
{
    /// <summary>
    /// 软件启动页面
    /// </summary>
    public class MainPageConfig : BasePageHabitConfig
    {
        private const string Tag = "1";
        public MainPageConfig(UserHabit mainHabit) : base(mainHabit)
        {
            
        }

        /// <summary>
        /// 页签显示
        /// </summary>
        public int TabIndex
        {
            get
            {
                return GetInt(Tag + "TabIndex");
            }
            set
            {
                SetInt(Tag + "TabIndex",value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int WWidth
        {
            get
            {
                return GetInt(Tag + "WWidth",1000);
            }
            set
            {
                SetInt(Tag + "WWidth", value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int WHeight
        {
            get
            {
                return GetInt(Tag + "WHeight",800);
            }
            set
            {
                SetInt(Tag + "WHeight", value);
            }
        } 
    }
}
