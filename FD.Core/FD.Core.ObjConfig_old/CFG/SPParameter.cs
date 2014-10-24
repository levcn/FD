namespace STComponse.CFG
{
    /// <summary>
    /// 存储过程参数
    /// </summary>
    public class SPParameter : EntityBase
    {
        public const string 字符串 = "字符串";
        public const string 整数 = "整数";
        public const string 小数 = "小数";
        public const string 日期 = "日期";

        private bool _isOutput;

        /// <summary>
        /// 是否为输出
        /// </summary>
        public bool IsOutput
        {
            get
            {
                return  _isOutput;
            }
            set
            {
                _isOutput = value;
                OnPropertyChanged(() => IsOutput);
            }
        }

        private string _defaultValue;
        private string _value;

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get
            {
                return _defaultValue;
            }
            set
            {
                _defaultValue = value;
                OnPropertyChanged(() => DefaultValue);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged(() => Value);
            }
        }

        private string _parameterSize;
        
        /// <summary>
        /// 列大小
        /// 整数:10,3
        /// 字符串:200
        /// 逻辑:0=未设置,1=男,2=女
        /// </summary>
        public string ParameterSize
        {
            get
            {
                return _parameterSize;
            }
            set
            {
                _parameterSize = value;
                OnPropertyChanged(() => ParameterSize);
            }
        }
        /// <summary>
        /// 列大小
        /// 整数:10,3
        /// 字符串:200
        /// 逻辑:0=未设置,1=男,2=女
        /// </summary>
        public string ParameterType
        {
            get
            {
                return _parameterType;
            }
            set
            {
                _parameterType = value;
                OnPropertyChanged(() => ParameterType);
            }
        }
        /// <summary>
        /// 存储过程的内容
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyChanged(()=>Content);
            }
        }

        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                OnPropertyChanged(() => Code);

            }
        }

        private string _name;
        private string _code;
        private string _content;
        private string _parameterType;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }
    }
}