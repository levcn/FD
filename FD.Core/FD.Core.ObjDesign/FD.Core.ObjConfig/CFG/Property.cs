using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace STComponse.CFG
{
#if SL
    public class Property :EntityBase
#else
    public class Property : EntityBase
#endif
    
    {
        private string _code;
        private string _name;
        private string _columnType;
        private string _columnSize;
        private string _initValue;
        private bool _isPrimaryKey;
        private bool _isOutKey;
        private bool _isShow;
        private string _remark;

        public Property()
        {
            ID = Guid.NewGuid();
        }

        public const string 字符串 = "字符串";
        public const string GUID = "GUID";
        public const string 整数 = "整数";
        public const string 逻辑 = "逻辑";
        public const string 小数 = "小数";
        public const string 日期 = "日期";
        public const string 备注 = "备注";
        public const string 关联 = "关联";
        public const string 大字段 = "大字段";
        public static readonly string[] PropertyTypes = { 字符串, GUID, 整数, 逻辑, 小数, 日期, 备注, 关联, 大字段 };

//        [XmlIgnore]
//        public EDataObject EDataObject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ID{ get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(5, ErrorMessage = "长度(1-5)", MinimumLength = 1)]
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

        /// <summary>
        /// 字段名
        /// </summary>
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                OnPropertyChanged(() =>Code);
            }
        }

        /// <summary>
        /// 列类型(汉字式)
        /// 字符串
        /// GUID
        /// 整数
        /// 逻辑
        /// 小数
        /// 日期
        /// 备注
        /// 关联
        /// </summary>
        public string ColumnType
        {
            get
            {
                return _columnType;
            }
            set
            {
                _columnType = value;
                OnPropertyChanged(() => ColumnType);
            }
        }

        /// <summary>
        /// 列大小
        /// 整数:10,3
        /// 字符串:200
        /// 逻辑:0=未设置,1=男,2=女
        /// </summary>
        public string ColumnSize
        {
            get
            {
                return _columnSize;
            }
            set
            {
                _columnSize = value;
                OnPropertyChanged(() => ColumnSize);
            }
        }

        [XmlIgnore]
        public LogicConfig LogicConfig
        {
            get
            {
                if (ColumnType != "逻辑") return null;
                return ColumnSize;
            }
            set
            {
                if (ColumnType != "逻辑") return;
                ColumnSize = value;
            }
        }

        [XmlIgnore]
        public VarcharConfig VarcharConfig
        {
            get
            {
                if (ColumnType != "字符串") return null;
                return ColumnSize;
            }
            set
            {
                if (ColumnType != "字符串") return;
                ColumnSize = value;
            }
        }
        [XmlIgnore]
        public DecimalConfig DecimalConfig
        {
            get
            {
                if (ColumnType != "小数" && ColumnType != Property.整数) return null;
                return ColumnSize;
            }
            set
            {
                if (ColumnType != "小数" && ColumnType != Property.整数) return;
                ColumnSize = value;
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public string InitValue
        {
            get
            {
                return _initValue;
            }
            set
            {
                _initValue = value;
                OnPropertyChanged(() => InitValue);
            }
        }

        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return _isPrimaryKey;
            }
            set
            {
                _isPrimaryKey = value;
                OnPropertyChanged(() => IsPrimaryKey);
            }
        }

        /// <summary>
        /// 是否外部关键字
        /// </summary>
        public bool IsOutKey
        {
            get
            {
                return _isOutKey;
            }
            set
            {
                _isOutKey = value;
                OnPropertyChanged(() => IsOutKey);
            }
        }

        /// <summary>
        /// 是否为显示名
        /// </summary>
        public bool IsShow
        {
            get
            {
                return _isShow;
            }
            set
            {
                _isShow = value;
                OnPropertyChanged(() => IsShow);
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                _remark = value;
                OnPropertyChanged(() => Remark);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}({1})",Name,Code);
        }
    }

    /// <summary>
    /// 自定义类型
    /// </summary>
    public class CusProperty : Property
    {
        public EDataObject Type { get; set; }
    }
}
