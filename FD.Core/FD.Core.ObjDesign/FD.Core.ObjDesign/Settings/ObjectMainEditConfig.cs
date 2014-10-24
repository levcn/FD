namespace ProjectCreater.Settings
{
    /// <summary>
    /// 对象编辑页面主页
    /// </summary>
    public class ObjectMainEditConfig : BasePageHabitConfig
    {
        private const string Tag = "2";
        public ObjectMainEditConfig(UserHabit mainHabit)
                : base(mainHabit)
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
                SetInt(Tag + "TabIndex", value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int WWidth
        {
            get
            {
                return GetInt(Tag + "WWidth", 700);
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
                return GetInt(Tag + "WHeight", 500);
            }
            set
            {
                SetInt(Tag + "WHeight", value);
            }
        }

        #region 属性表格设置

         
        /// <summary>
        /// 页签显示
        /// </summary>
        public int GNameWidth
        {
            get
            {
                return GetInt(Tag + "GNameWidth", 100);
            }
            set
            {
                SetInt(Tag + "GNameWidth", value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int GCodeWidth
        {
            get
            {
                return GetInt(Tag + "GCodeWidth", 100);
            }
            set
            {
                SetInt(Tag + "GCodeWidth", value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int GTypeWidth
        {
            get
            {
                return GetInt(Tag + "GTypeWidth", 70);
            }
            set
            {
                SetInt(Tag + "GTypeWidth", value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int GLenghtWidth
        {
            get
            {
                return GetInt(Tag + "GLenghtWidth", 200);
            }
            set
            {
                SetInt(Tag + "GLenghtWidth", value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int GDefaultValueWidth
        {
            get
            {
                return GetInt(Tag + "GDefaultValueWidth", 200);
            }
            set
            {
                SetInt(Tag + "GDefaultValueWidth", value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int GIsPKeyWidth
        {
            get
            {
                return GetInt(Tag + "GIsPKeyWidth", 80);
            }
            set
            {
                SetInt(Tag + "GIsPKeyWidth", value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int GIsRelWidth
        {
            get
            {
                return GetInt(Tag + "GIsRelWidth", 80);
            }
            set
            {
                SetInt(Tag + "GIsRelWidth", value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int GIsShowWidth
        {
            get
            {
                return GetInt(Tag + "GIsShowWidth", 80);
            }
            set
            {
                SetInt(Tag + "GIsShowWidth", value);
            }
        }

        #endregion

        #region 关系表格设置

        /// <summary>
        /// 页签显示
        /// </summary>
        public int GAttr
        {
            get
            {
                return GetInt(Tag + "GAttr", 100);
            }
            set
            {
                SetInt(Tag + "GAttr", value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int GRelType
        {
            get
            {
                return GetInt(Tag + "GRelType", 80);
            }
            set
            {
                SetInt(Tag + "GRelType", value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int GRelExplan
        {
            get
            {
                return GetInt(Tag + "GRelExplan", 500);
            }
            set
            {
                SetInt(Tag + "GRelExplan", value);
            }
        }
        /// <summary>
        /// 页签显示
        /// </summary>
        public int GRelMemo
        {
            get
            {
                return GetInt(Tag + "GRelMemo", 150);
            }
            set
            {
                SetInt(Tag + "GRelMemo", value);
            }
        }
        #endregion
    }
}