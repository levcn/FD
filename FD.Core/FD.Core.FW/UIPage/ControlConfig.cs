using System.Collections.Generic;
using System.ComponentModel;
using Fw.Entity;


namespace StaffTrain.SVFw.UIPage
{
    /// <summary>
    /// 控件配置
    /// </summary>
    public class ControlConfig
    {
        public ControlConfig()
        {
            ItemConfig = new List<PropItem>();
            DataConfig = new ControlDataConfig();
            ValidateConfigs = new List<ColumnValidataConfig>();

        }
        /// <summary>
        /// 名称,控件的Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 配置的内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 需要绑定的对象代码
        /// </summary>
        public string ObjectCode { get; set; }
        public string EntityCode { get; set; }


        /// <summary>
        /// 控件的设置
        /// </summary>
        public List<PropItem> ItemConfig { get; set; }

        /// <summary>
        /// 数据来源设置
        /// </summary>
        public ControlDataConfig DataConfig { get; set; }

        /// <summary>
        /// 控件的验证
        /// </summary>
        public List<ColumnValidataConfig> ValidateConfigs { get; set; }
    }
    public class ColumnValidataConfig
    {
        public ColumnValidataConfig(string name)
        {
            Name = name;
        }

        public ColumnValidataConfig()
        {}

        /// <summary>
        /// 属性名
        /// </summary>
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Required { get; set; }
        /// <summary>
        /// 字符串类型 0:正则,1:数字,2:小数,3:日期,4:信箱,5:邮编,6:身份证
        /// </summary>
        public int DataType { get; set; }
        public bool RequiredError { get; set; }
        public string RegexPattern { get; set; }
        public string RegexPatternError { get; set; }
        public int? TextMinLenth { get; set; }
        public int? TextMaxLenth { get; set; }
        public double? NumberMin { get; set; }
        public double? NumberMax { get; set; }
    }
    /// <summary>
    /// 数据来源设置
    /// </summary>
    public class ControlDataConfig
    {
        public ControlDataConfig()
        {
            Parametes = new List<STParamete>();
        }

        /// <summary>
        /// 和页面的哪个对象绑定
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// 1数据,2存储过程
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 存储过程名称
        /// </summary>
        public string SPName { get; set; }

        /// <summary>
        /// 存储过程的参数设置
        /// </summary>
        public List<STParamete> Parametes { get; set; }

        /// <summary>
        /// 该控件的数据源是页面的哪个对象;Object1-Object10
        /// </summary>
        public string PageObject { get; set; }
    }
    /// <summary>
    /// 属性值的设置(保存和读取)
    /// </summary>
    public class PropItem : INotifyPropertyChanged
    {
        private bool _isSaveCache;
        private CacheType _cacheType;
        private string _from;
        private string _propName;

        public PropItem()
        {
            CacheType = CacheType.Navigator;
        }

        /// <summary>
        /// 属性名 (eg:User = #Page1.User)
        /// </summary>
        public string PropName
        {
            get
            {
                return _propName;
            }
            set
            {

                if (_propName != value)
                {
                    _propName = value;
                    OnPropertyChanged("PropName");
                }
            }
        }

        private string _displayName;
        /// <summary>
        /// 显示名 
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {

                if (_displayName != value)
                {
                    _displayName = value;
                    OnPropertyChanged("DisplayName");
                }
            }
        }
        /// <summary>
        /// 值从哪里来 (#Page1.User)
        /// </summary>
        public string From
        {
            get
            {
                return _from;
            }
            set
            {
                if (_from != value)
                {
                    _from = value;
                    OnPropertyChanged("From");
                }
            }
        }

        //        /// <summary>
        //        /// 值保存到哪里(User)
        //        /// </summary>
        //        public string To { get; set; }

        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType CacheType
        {
            get
            {
                return _cacheType;
            }
            set
            {
                if (_cacheType != value)
                {
                    _cacheType = value;
                    OnPropertyChanged("CacheType");
                }
            }
        }

        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool IsSaveCache
        {
            get
            {
                return _isSaveCache;
            }
            set
            {
                if (_isSaveCache != value)
                {
                    _isSaveCache = value;
                    OnPropertyChanged("IsSaveCache");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// 缓存类型
    /// </summary>
    public enum CacheType
    {
        App,
        Navigator,
        Page,
    }
}