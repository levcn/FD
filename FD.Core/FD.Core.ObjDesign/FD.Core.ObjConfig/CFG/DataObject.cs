using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using STComponse.GCode;


namespace STComponse.CFG
{
    public interface IPositioning
    {
        int Left { get; set; }
        int Top { get; set; }
    }


    public class EDataObject : INotifyPropertyChanged, IPositioning
    {
        private string _keyTableName;
        private string _objectCode;
        private string _objectName;
        private ObservableCollection<Relation> _relation;
        private List<SPParameter> _spParameters;
        private ObservableCollection<BigFieldValue> _bigField;

        public EDataObject()
        {
            ObjectName = "";
            ObjectCode = "";
            KeyTableName = "";
            Remark = "";
            Property = new ObservableCollection<Property>();
            Relation = new ObservableCollection<Relation>();
            BigFields = new ObservableCollection<BigFieldValue>();

        }

        public bool IsVirtual { get; set; }

        /// <summary>
        ///     存储过程名称
        /// </summary>
        public string StoredProcedureName { get; set; }

        /// <summary>
        ///     存储过程参数
        /// </summary>
        public List<SPParameter> SPParameters
        {
            get { return _spParameters; }
            set
            {
                _spParameters = value;
                OnPropertyChanged("SPParameters");
            }
        }

        public Guid ID { get; set; }

        /// <summary>
        ///     对象名(人员表)
        /// </summary>
        public string ObjectName
        {
            get { return _objectName; }
            set
            {
                _objectName = value;
                OnPropertyChanged("ObjectName");
            }
        }

        /// <summary>
        ///     C#的类名(SYS_User)
        /// </summary>
        public string ObjectCode
        {
            get { return _objectCode; }
            set
            {
                _objectCode = value;
                OnPropertyChanged("ObjectCode");
            }
        }

        /// <summary>
        ///     表名(SYS_User)
        /// </summary>
        public string KeyTableName
        {
            get { return _keyTableName; }
            set
            {
                _keyTableName = value;
                OnPropertyChanged("KeyTableName");
            }
        }

        public ObservableCollection<BigFieldValue> BigFields
        {
            get { return _bigField; }
            set
            {
                _bigField = value;
                OnPropertyChanged("BigField");
            }
        }

        public string Remark { get; set; }
        public ObservableCollection<Property> Property { get; set; }

        public ObservableCollection<Relation> Relation
        {
            get { return _relation; }
            set
            {
                _relation = value;
                OnPropertyChanged("Relation");
            }
        }


        public int Left { get; set; }
        public int Top { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
//
//    public class BigField
//    {
//        public BigField()
//        {
//            ConfigItems = new ObservableCollection<BigFieldConfigItem>();
//        }
//        public Guid ID { get; set; }
//        public ObservableCollection<BigFieldConfigItem> ConfigItems { get; set; }
//    }
//
//    public static class BigFieldEx
//    {
//        public 
//    }
//    /// <summary>
//    /// 大字段中每个属性的设置
//    /// </summary>
//    public class BigFieldConfigItem : INotifyPropertyChanged
//    {
//        private Guid _id;
//        private string _displayName;
//        private string _code;
//        private string _itemType;
//        private string _remark;
//        private string _defaultValue;
//        private string _columnSize;
//
//        public Guid ID
//        {
//            get
//            {
//                return _id;
//            }
//            set
//            {
//                _id = value;
//                OnPropertyChanged("ID");
//            }
//        }
//
//        public string DisplayName
//        {
//            get
//            {
//                return _displayName;
//            }
//            set
//            {
//                _displayName = value;
//                OnPropertyChanged("Name");
//            }
//        }
//
//        public string Code
//        {
//            get
//            {
//                return _code;
//            }
//            set
//            {
//                _code = value;
//                OnPropertyChanged("Code");
//
//            }
//        }
//
//        public string ItemType
//        {
//            get
//            {
//                return _itemType;
//            }
//            set
//            {
//                _itemType = value;
//                OnPropertyChanged("ItemType");
//
//            }
//        }
//
//        public string Remark
//        {
//            get
//            {
//                return _remark;
//            }
//            set
//            {
//                _remark = value;
//                OnPropertyChanged("Remark");
//            }
//        }
//
//        public string DefaultValue
//        {
//            get
//            {
//                return _defaultValue;
//            }
//            set
//            {
//                _defaultValue = value;
//                OnPropertyChanged("DefaultValue");
//            }
//        }
//
//        public string ColumnSize
//        {
//            get
//            {
//                return _columnSize;
//            }
//            set
//            {
//                _columnSize = value;
//                OnPropertyChanged(()=>ColumnSize);
//
//            }
//        }
//
//        public event PropertyChangedEventHandler PropertyChanged;
//
////        protected virtual void OnPropertyChanged(string propertyName = null)
////        {
////            PropertyChangedEventHandler handler = PropertyChanged;
////            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
////        }
//
//        /// <summary>
//        /// 属性改变时触发事件
//        /// </summary>
//        /// <param name="propertyName">Property that changed.</param>
//        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyName)
//        {
//            if (PropertyChanged != null)
//            {
//                var expression = propertyName.Body as MemberExpression;
//                if (expression != null)
//                {
//                    PropertyChanged(this, new PropertyChangedEventArgs(expression.Member.Name));
//                }
//            }
//        }
//    }

    /// <summary>
    /// 大字段中条目属性的类型
    /// </summary>
    public enum BigFieldItemType
    {
        Int,
        Double,
        String,
        Date,
        DateTime,
        Boolean,
    }
}