using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;
using SLComponse.Validate;
using STComponse.CFG;
using Telerik.Windows.Controls;


namespace FD.Core.SLControl.Data
{
    public class CustomValidArgs
    {
        public BaseListEntity Obj { get; set; }
        public string PropName { get; set; }
        public object PropValue { get; set; }
        /// <summary>
        /// 7表唯一验证
        /// </summary>
        public int ValidType { get; set; }
    }

    public interface IIsSelected
    {
        bool IsSelected { get; set; }
    }
    public class BaseListEntity : ViewModelBase, IDataErrorInfo, INotifyDataErrorInfo, IIsSelected, INode

    {
        //        public class BaseListEntity : INode
        [IgnoreDataMember]
        [XmlIgnore]
        private string _picUrl;

        [IgnoreDataMember]
        [XmlIgnore]
        public string PicUrl
        {
            get
            {
                return _picUrl;
            }
            set
            {
                _picUrl = value;
#if SL
                //                if (value != null) Picture = new BitmapImage(new Uri(value, UriKind.Relative));
#endif
            }
        }
        private bool _IsExpanded;
        [IgnoreDataMember]
        [XmlIgnore]
        public virtual bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                if (value != _IsExpanded)
                {
                    _IsExpanded = value;

                    OnPropertySelectedChanged("IsExpanded");
                }
            }
        }


        //        [IgnoreDataMember]
        //        [XmlIgnore]
        //        public BitmapImage Picture { get; set; }
        [IgnoreDataMember]
        [XmlIgnore]
        private IList<INode> children;
        //        private ObservableCollection<INode> children;


        //        public event PropertyChangedEventHandler PropertyChanged;

        [IgnoreDataMember]
        [XmlIgnore]
        public bool HasSubcomponents
        {
            get
            {
                return Children.Count != 0;
            }
        }
        [IgnoreDataMember]
        [XmlIgnore]
        //        public ObservableCollection<INode> Children
        public IList<INode> Children
        {
            get { return children ?? (children = new List<INode>()); }
            //            get { return children ?? (children = new ObservableCollection<INode>()); }
            set
            {
                if (children != value)
                {
                    children = value;
                    OnPropertySelectedChanged("Children");
                }
            }
        }

        private string _text;
        [IgnoreDataMember]
        [XmlIgnore]
        public virtual String Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (value != _text)
                {
                    _text = value;
                    OnPropertySelectedChanged("Text");
                }
            }
        }

        public BaseListEntity()
        {
            Children = new ObservableCollection<INode>();
#if SL
            _validationErrors = new Dictionary<string, ObservableCollection<string>>();
#endif
        }

        public void Add(INode node)
        {
            children.Add(node);
            OnPropertySelectedChanged("Children");
        }

        public void Delete(INode node)
        {
            children.Remove(node);
            OnPropertySelectedChanged("Children");
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        private bool _isSelected;

        [IgnoreDataMember]
        [XmlIgnore]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertySelectedChanged(() => IsSelected);
                }
            }
        }
#if SL
        [XmlIgnore]
        [IgnoreDataMember]
        public string Error
        {
            get
            {
                return ValidateAllError();
                //                var error = ValidateCheckInDate() ?? ValidateCheckOutDate() ?? ValidateText();
                //                return error;
            }
        }

        private string ValidateAllError(string propName = null)
        {
            var ps = GetType().GetProperties().ToList();
            if (propName != null)
            {
                ps = ps.Where(w => w.Name == propName).ToList();
            }
            string re = null;
            if (ValidataConfig != null)
            {
                ps.ForEach(w =>
                {
                    var validateError = GetValidateError(w);
                    if (validateError != null) re += validateError + "\r\n";
                });
            }
            if (re != null && re.Length >= 2) re = re.Substring(0, re.Length - 2);
            var enumerable = GetErrors(propName);
            if (re == null && enumerable != null)
            {
                re = enumerable.OfType<string>().ToList().FirstOrDefault();
            }
            return re;
        }

        private string GetValidateError(PropertyInfo p)
        {
            var config = ValidataConfig.ColumnValidata.FirstOrDefault(w => w.Name == p.Name);
            if (config != null)
            {
                var value = p.GetValue(this, null);
                var haveErr = false;
                var v = value == null ? "" : value.ToString();
                //不为空检测
                if (config.Required)
                {
                    if (value == null)//值为空
                    {
                        haveErr = true;
                    }
                    else
                    {
                        var str = value.ToString();
                        if (string.IsNullOrWhiteSpace(str))//值为空
                        {
                            haveErr = true;
                        }
                    }
                    if (haveErr)
                    {
                        return string.Format("{0}不能为空。", config.Name);
                    }
                }
                if (config.TextMinLenth != null && v.Length < config.TextMinLenth) return string.Format("至少输入{0}位。", config.TextMinLenth);
                if (config.TextMaxLenth != null && v.Length > config.TextMaxLenth) return string.Format("最多输入{0}位。", config.TextMaxLenth);

                string re = null;
                //
                switch (config.DataType)
                {
                    case 0:
                        re = MatchPattern(v, config.RegexPattern, config.RegexPatternError);
                        break;
                    case 1://数字
                        re = MatchPattern(v, @"^^[1-9]\d*$", "请输入数字。");
                        break;
                    case 2://小数
                        re = MatchPattern(v, @"^^[1-9]\d*$", "请输入数字。");
                        break;
                    case 3://日期
                        re = v.ToDate() == null ? "请输入正确的日期。" : null;
                        break;
                    case 4://信箱
                        re = MatchPattern(v, @"^^[1-9]\d*$", "请输入数字。");
                        break;
                    case 5://邮编
                        re = MatchPattern(v, @"^^[1-9]\d*$", "请输入数字。");
                        break;
                    case 6://身份证
                        re = MatchPattern(v, @"^^[1-9]\d*$", "请输入数字。");
                        break;
                    case 7://表唯一
                        ClearError(p.Name);
                        SetErrors(p.Name, "正在检测..");
                        OnErrorsChanged(new DataErrorsChangedEventArgs(p.Name));
                        OnOnValidate(new CustomValidArgs { Obj = this, PropName = p.Name, PropValue = value, ValidType = 7 });
                        return "正在检测..";
                        break;
                    default:
                        throw new Exception("未知的验证类型:" + config.DataType);
                }
                if (re == null)
                {
                    if (config.NumberMin != null && v.ToDouble(0) < config.NumberMin) return string.Format("应该大于{0}。", config.NumberMin);
                    if (config.NumberMax != null && v.ToDouble(0) > config.NumberMax) return string.Format("应该小于{0}。", config.NumberMax);
                }
                return re;
            }
            return null;
        }
        private string MatchPattern(string str, string regexPattern, string regexPatternError, string defaultErrorMessage = null)
        {
            defaultErrorMessage = defaultErrorMessage ?? "格式不正确";
            if (!Regex.IsMatch(str, regexPattern))
            {
                return string.IsNullOrWhiteSpace(regexPatternError) ? defaultErrorMessage : regexPatternError;
            }
            return null;
        }

        public string this[string columnName]
        {
            get
            {
                return ValidateAllError(columnName);
                //                switch (columnName)
                //                {
                //                    case "CheckInDate": return this.ValidateCheckInDate();
                //                    case "CheckOutDate": return this.ValidateCheckOutDate();
                //                    case "Name": return ValidateText();
                //                }
            }
        }
        public ValidataConfig ValidataConfig { get; set; }
#endif
        /// <summary>
        /// 属性改变时触发事件
        /// </summary>
        /// <param name="propertyName">Property that changed.</param>
        protected void OnPropertySelectedChanged<T>(Expression<Func<T>> propertyName)
        {
#if SL
            base.OnPropertyChanged(propertyName);
#endif
        }

        /// <summary>
        /// 属性改变时触发事件
        /// </summary>
        /// <param name="propertyName">Property that changed.</param>
        public void OnPropertySelectedChanged([CallerMemberName]string propertyName = null)
        {
#if SL
            base.OnPropertyChanged(propertyName);
#endif
        }
        #region INotifyDataErrorInfo
#if SL
        #region 异步验证
        private Dictionary<string, ObservableCollection<string>> _validationErrors;

        /// <summary>
        /// 清除指定属性的错误列表
        /// </summary>
        /// <param name="propName"></param>
        public void ClearError(string propName)
        {
            if (_validationErrors.ContainsKey(propName))
            {
                _validationErrors[propName].Clear();
            }
        }

        public void SetErrors(string propertyName, string errorMessage)
        {
            if (!_validationErrors.ContainsKey(propertyName))
                _validationErrors.Add(propertyName, new ObservableCollection<string>());

            _validationErrors[propertyName].Add(errorMessage);
        }
        /// <summary>
        /// 返回指定属性的错误
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (_validationErrors.ContainsKey(propertyName))
                    return _validationErrors[propertyName];
                else
                    return null;
            }
            else
                return null;
        }

        public bool HasErrors
        {
            get
            {
                foreach (string key in _validationErrors.Keys)
                {
                    if (_validationErrors[key].Count > 0)
                        return true;
                }
                return false;
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            var handler = ErrorsChanged;
            if (handler != null) handler(this, e);
        }
        /// <summary>
        /// 当有需要验证时(异步验证，自定义验证等)
        /// </summary>
        public event TEventHandler<object, CustomValidArgs> OnValidate;

        protected virtual void OnOnValidate(CustomValidArgs args)
        {
            var handler = OnValidate;
            if (handler != null) handler(this, args);
        }
        #endregion

#endif

        #endregion
    }
    public delegate void TEventHandler<in TSender, in TEventArgs1>(TSender sender, TEventArgs1 args);
    public interface INode : INotifyPropertyChanged,IIsSelected
    {
        [IgnoreDataMember]
        [XmlIgnore]
        IList<INode> Children { get; set; }

        /// <summary>
        ///     要显示的文本
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        string Text { get; set; }

        /// <summary>
        ///     是否展开
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        bool IsExpanded { get; set; }

#if SL
//        /// <summary>
//        ///     图片
//        /// </summary>
//        [IgnoreDataMember]
//        [XmlIgnore]
//        BitmapImage Picture { get; set; }
#endif
        /// <summary>
        ///     图片地址
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        string PicUrl { get; set; }

        void Add(INode node);

        void Delete(INode node);
    }
}
