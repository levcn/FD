using System;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Fw.Extends.Table;
using StaffTrain.FwClass.Reflectors;
using StaffTrain.FwClass.UserAttributes;


namespace StaffTrain.FwClass.Extends.Linq
{
    public static class PropertyExpressionExtends
    {
        public static string GetColumnName(this System.Linq.Expressions.Expression owner, bool includeTableName = true)
        {
            var property = owner.GetProperty();
            if (property != null)
            {
//                ReflectionHelper.GetTableName()
                var re = ReflectionHelper.GetPropertyField(property);
                return re;
            }
            return "";
        }

        public static PropertyInfo GetProperty(this System.Linq.Expressions.Expression owner)
        {
            PropertyInfo re = null;
            PartialEvaluator evaluator = new PartialEvaluator();
            System.Linq.Expressions.Expression evaluatedExpression = evaluator.Eval(owner);
            var type = evaluatedExpression.GetType();
            var property = type.GetProperty("Member");
            var value = property.GetValue(evaluatedExpression, null);
            re = value as PropertyInfo;
            return re;
        }
    }
}
