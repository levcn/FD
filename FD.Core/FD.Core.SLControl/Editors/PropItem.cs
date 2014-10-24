using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLControls.Editors
{

    /// <summary>
    /// 属性值的设置(保存和读取)
    /// </summary>
    public class PropItem:INotifyPropertyChanged
    {
        private bool _isSaveCache;
        private CacheType _cacheType;
        private string _from;
        private string _propName;

        public PropItem()
        {
            CacheType = CacheType.Navigator;
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
