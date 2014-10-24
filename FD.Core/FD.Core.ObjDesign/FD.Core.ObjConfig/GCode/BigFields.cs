using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Xml.Serialization;
using STComponse.CFG;


namespace STComponse.GCode
{
    public class BigFieldValue
    {
        public Guid ID { get; set; }
        public BigFieldValue()
        {
            ID = Guid.NewGuid();
            ConfigItems = new ObservableCollection<BigFieldItem>();
        }
        public ObservableCollection<BigFieldItem> ConfigItems { get; set; }
    }
    /// <summary>
    /// 大字段条目
    /// </summary>
    public class BigFieldItem : INotifyPropertyChanged
    {
        public BigFieldItem()
        {
            ID = Guid.NewGuid();
        }

        private Guid _id;
        private string _code;
        private int _groupIndex;
        private string _value;
        private string _displayName;
        private string _groupCode;
        private string _columnSize;
        private string _defaultValue;
        private string _remark;
        private string _itemType;
        private string _controlType;
        private string _controlConfig;
        private DropDownConfig _ddConfig;
        public string ControlConfig
        {
            get
            {
                return _controlConfig;
            }
            set
            {
                _controlConfig = value;
                OnPropertyChanged(() => ControlType);
            }
        }

        [XmlIgnore]
        public DropDownConfig DDConfig
        {
            get
            {
                if (_ddConfig == null)
                {
                    _ddConfig = ControlConfig.ToObject<DropDownConfig>();
                    if(_ddConfig==null)_ddConfig = new DropDownConfig();
                }
                return _ddConfig;
            }
            set
            {
                _ddConfig = value;
                ControlConfig = _ddConfig.ToJson();
            }
        }
        /// <summary>
        /// 下拉框设置
        /// </summary>
        public class DropDownConfig
        {
            public DropDownConfig()
            {
                Items = new List<DropDownItem>();
            }
            public List<DropDownItem> Items { get; set; }
        }
        /// <summary>
        /// 下拉框中每条内容的设置
        /// </summary>
        public class DropDownItem
        {
            public string DisplayName { get; set; }
            public string Value { get; set; }
        }
        public string ControlType
        {
            get
            {
                return _controlType;
            }
            set
            {
                _controlType = value;
                OnPropertyChanged(() => ControlType);
            }
        }

        public Guid ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged(()=>ID);
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

        /// <summary>
        /// 当前条目在分组中的第几条;
        /// </summary>
        public int GroupIndex
        {
            get
            {
                return _groupIndex;
            }
            set
            {
                _groupIndex = value;
                OnPropertyChanged(() => GroupIndex);
            }
        }

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
                _displayName = value;
                OnPropertyChanged(() => DisplayName);
            }
        }

        /// <summary>
        /// 组不为空就可以复制
        /// </summary>
        public string GroupCode
        {
            get
            {
                return _groupCode;
            }
            set
            {
                _groupCode = value;
                OnPropertyChanged(() => GroupCode);

            }
        }

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

        public string UID 
        {
            get
            {
                return string.Format("{0}.{1}.{2}", GroupCode, GroupIndex,Code);
            }
        }

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

        public string ItemType
        {
            get
            {
                return _itemType;
            }
            set
            {
                _itemType = value;
                OnPropertyChanged(() => ItemType);

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性改变时触发事件
        /// </summary>
        /// <param name="propertyName">Property that changed.</param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyName)
        {
            if (PropertyChanged != null)
            {
                var expression = propertyName.Body as MemberExpression;
                if (expression != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(expression.Member.Name));
                }
            }
        }
    }

}
