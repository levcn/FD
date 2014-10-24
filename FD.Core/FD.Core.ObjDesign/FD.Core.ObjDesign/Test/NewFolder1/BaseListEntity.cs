using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StaffTrain.Entity
{
    public class BaseListEntity
    {
        /// <summary>
        /// 属性改变时触发事件
        /// </summary>
        /// <param name="propertyName">Property that changed.</param>
        protected void OnPropertySelectedChanged<T>(Expression<Func<T>> propertyName)
        {
//            if (PropertyChanged != null)
//            {
//                var expression = propertyName.Body as MemberExpression;
//                if (expression != null)
//                {
//                    PropertyChanged(this, new PropertyChangedEventArgs(expression.Member.Name));
//                }
//            }
        }
    }
}
