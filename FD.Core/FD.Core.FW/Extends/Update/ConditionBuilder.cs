using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionVisitor = Fw.Extends.Table.ExpressionVisitor;


namespace Fw.Extends.Table
{
	public class ConditionBuilder : ExpressionVisitor
	{

		private List<object> m_arguments;
		private Stack<string> m_conditionParts;
	    private List<ConstantExpression> constantExpressions;

		public string Condition { get; private set; }

		public object[] Arguments { get; private set; }

	    public List<ConstantExpression> ConstantExpressions
	    {
	        get
	        {
	            return constantExpressions;
	        }
	        set
	        {
	            constantExpressions = value;
	        }
	    }

	    public void Build(Expression expression)
		{
			PartialEvaluator evaluator = new PartialEvaluator();
			Expression evaluatedExpression = evaluator.Eval(expression);
	        
	        
			this.m_arguments = new List<object>();
			this.m_conditionParts = new Stack<string>();

			this.Visit(evaluatedExpression);

			this.Arguments = this.m_arguments.ToArray();
			this.Condition = this.m_conditionParts.Count > 0 ? this.m_conditionParts.Pop() : null;
		}

		protected override Expression VisitBinary(BinaryExpression b)
		{
			if (b == null) return b;

			string opr;
			switch (b.NodeType)
			{
				case ExpressionType.Equal:
					opr = "=";
					break;
				case ExpressionType.NotEqual:
					opr = "<>";
					break;
				case ExpressionType.GreaterThan:
					opr = ">";
					break;
				case ExpressionType.GreaterThanOrEqual:
					opr = ">=";
					break;
				case ExpressionType.LessThan:
					opr = "<";
					break;
				case ExpressionType.LessThanOrEqual:
					opr = "<=";
					break;
				case ExpressionType.AndAlso:
					opr = "AND";
					break;
				case ExpressionType.OrElse:
					opr = "OR";
					break;
				case ExpressionType.Add:
					opr = "+";
					break;
				case ExpressionType.Subtract:
					opr = "-";
					break;
				case ExpressionType.Multiply:
					opr = "*";
					break;
				case ExpressionType.Divide:
					opr = "/";
					break;
				default:
					throw new NotSupportedException(b.NodeType + " is not supported.");
			}

			this.Visit(b.Left);
			this.Visit(b.Right);

			string right = this.m_conditionParts.Pop();
			string left = this.m_conditionParts.Pop();

			string condition = String.Format("({0} {1} {2})", left, opr, right);
			this.m_conditionParts.Push(condition);

			return b;
		}
        public virtual List<T> GetParameter<T>()
        {
            List<T> re = new List<T>();
            Type type = typeof (T);
            var constructor = type.GetConstructor(new Type[]{typeof(string),typeof(object)});
            int index = 0;
            constantExpressions.ForEach(w=>
                                            {
                                                re.Add(
                                                        (T)
                                                        constructor.Invoke(new object[]
                                                                               {
                                                                                       "@a" + index++,
                                                                                       Convert.ChangeType(w.Value,
                                                                                                          w.Type)
                                                                               }));
                                            });
            return re;
        }
		protected override Expression VisitConstant(ConstantExpression c)
		{
			if (c == null) return c;

			this.m_arguments.Add(c.Value);
            //this.m_conditionParts.Push(String.Format("{{{0}}}", this.m_arguments.Count - 1));
            this.m_conditionParts.Push(String.Format("@a{0}", this.m_arguments.Count - 1));
            if (constantExpressions==null)constantExpressions = new List<ConstantExpression>();
            constantExpressions.Add(c);
			return c;
		}

		protected override Expression VisitMemberAccess(MemberExpression m)
		{
			if (m == null) return m;

			PropertyInfo propertyInfo = m.Member as PropertyInfo;
			if (propertyInfo == null) return m;

			this.m_conditionParts.Push(String.Format("[{0}]", propertyInfo.Name));

			return m;
		}

		protected override Expression VisitMethodCall(MethodCallExpression m)
		{
			if (m == null) return m;

			string format;
			switch (m.Method.Name)
			{
				case "StartsWith":
					format = "({0} LIKE {1}+'%')";
					break;

				case "Contains":
					format = "({0} LIKE '%'+{1}+'%')";
					break;

				case "EndsWith":
					format = "({0} LIKE '%'+{1})";
					break;

				default:
					throw new NotSupportedException(m.NodeType + " is not supported!");
			}

			this.Visit(m.Object);
			this.Visit(m.Arguments[0]);
			string right = this.m_conditionParts.Pop();
			string left = this.m_conditionParts.Pop();
			this.m_conditionParts.Push(String.Format(format, left, right));

			return m;
		}
	}
}
