using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace STComponse.CFG
{
    public abstract class EntityBase : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly Dictionary<string, PropertyInfo> _propertyGetters = new Dictionary<string, PropertyInfo>();
        private readonly Dictionary<string, ValidationAttribute[]> _validators = new Dictionary<string, ValidationAttribute[]>();
        private readonly Type _type;
        protected EntityBase()
        {
            _type = GetType();
            LoadData();
        }
        #region 私有方法
        private void LoadData()
        {
            PropertyInfo[] properties = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in properties)
            {
                //拥μ有D的?验é证¤特?性?
                object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), true);
                if (customAttributes.Length > 0)
                {
                    _validators.Add(propertyInfo.Name, customAttributes as ValidationAttribute[]);
                    _propertyGetters.Add(propertyInfo.Name, propertyInfo);
                }
            }
        }
        /// 

//        /// 属?性?更ü改?通¨知a
//        /// 
//        /// 
//        protected void OnPropertyChanged(string propertyName)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
        #endregion

        #region IDataErrorInfo Members
        /// 

        /// 实μ现?IDataErrorInfo接ó口ú(获?取?校￡验é未′通¨过y的?错í误ó提á示?)
        /// 
//        [CustomizeValidation]
        public string Error
        {
            get
            {
#if SL
                return null;
#else
                IEnumerable<string> errors = from d in _validators
                                             from v in d.Value

//                                             where v.Validate(_propertyGetters[d.Key].GetValue(this, null),new ValidationContext(null))

                                             where !v.IsValid(_propertyGetters[d.Key].GetValue(this, null))

                                             select v.ErrorMessage;
                return string.Join(Environment.NewLine, errors.ToArray());
#endif
            }
        }
        /// 

        /// 实μ现?IDataErrorInfo接ó口ú()
        /// 
        /// 
        /// 
        public string this[string columnName]
        {
            get
            {
                if (_propertyGetters.ContainsKey(columnName))
                {
#if !SL
                    object value = _propertyGetters[columnName].GetValue(this, null);
                    string[] errors = _validators[columnName].Where(v => !v.IsValid(value))
                        .Select(v => v.ErrorMessage).ToArray();
                    OnPropertyChanged(() => Error);
                    return string.Join(Environment.NewLine, errors);
#endif
                }
                OnPropertyChanged(()=>Error);
                return string.Empty;
            }
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
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
