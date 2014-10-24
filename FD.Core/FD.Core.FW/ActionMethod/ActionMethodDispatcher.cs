using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Fw.ActionMethod
{
    public class ActionMethodDispatcher
    {
        private ActionExecutor _executor;

        public MethodInfo MethodInfo { get; private set; }

        public ActionMethodDispatcher(MethodInfo methodInfo)
        {
            this._executor = GetExecutor(methodInfo);
            this.MethodInfo = methodInfo;
        }

        public object Execute(IController controller, object[] parameters)
        {
            return this._executor(controller, parameters);
        }

        private static ActionExecutor GetExecutor(MethodInfo methodInfo)
        {
            ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object[]), "parameters");
            List<Expression> list = new List<Expression>();
            ParameterExpression parameterExpression1 = Expression.Parameter(typeof(IController), "controller");
            var parameters = methodInfo.GetParameters();
            for (int index = 0; index < parameters.Length; ++index)
            {
                ParameterInfo parameterInfo = parameters[index];
                UnaryExpression unaryExpression = Expression.Convert(Expression.ArrayIndex(parameterExpression2, Expression.Constant(index)), parameterInfo.ParameterType);
                list.Add(unaryExpression);
            }
            Expression ds = null;
            if (!methodInfo.IsStatic)
            {
                ds = Expression.Convert(parameterExpression1, methodInfo.ReflectedType);
            }
            MethodCallExpression methodCallExpression = Expression.Call(ds, methodInfo, list);
            if (methodCallExpression.Type == typeof (void))
                return
                        WrapVoidAction(
                                Expression.Lambda<VoidActionExecutor>(
                                         methodCallExpression, new []
                                                                               {
                                                                                       parameterExpression1,
                                                                                       parameterExpression2
                                                                               }).Compile());
            else
            {
                return
                        Expression.Lambda<ActionExecutor>(Expression.Convert(methodCallExpression, typeof (object)),
                                                          new []
                                                              {
                                                                      parameterExpression1,
                                                                      parameterExpression2
                                                              }).Compile();
            }
        }

        private static ActionExecutor WrapVoidAction(VoidActionExecutor executor)
        {
            return (controller, parameters) =>
            {
                executor(controller, parameters);
                return (object)null;
            };
        }
    }
}
