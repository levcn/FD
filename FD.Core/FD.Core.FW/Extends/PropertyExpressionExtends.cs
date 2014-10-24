using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Fw.Extends.Table;


namespace Fw.Extends
{
    public static class PropertyExpressionExtends
    {


        public static PropertyInfo GetProperty(this Expression owner)
        {
            PropertyInfo re = null;
            PartialEvaluator evaluator = new PartialEvaluator();
            Expression evaluatedExpression = evaluator.Eval(owner);
            var type = evaluatedExpression.GetType();
            var property = type.GetProperty("Member");
            var value = property.GetValue(evaluatedExpression, null);
            re = value as PropertyInfo;
            return re;
        }
    }
}
