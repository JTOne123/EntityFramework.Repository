using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository.Internals.Utils
{
	internal static class DbHelpers
	{
		internal static Expression RemoveConvert(this Expression expression)
		{
			DebugCheck.NotNull(expression);

			while (expression.NodeType == ExpressionType.Convert
				   || expression.NodeType == ExpressionType.ConvertChecked)
			{
				expression = ((UnaryExpression)expression).Operand;
			}

			return expression;
		}

		// <summary>
		// Called recursively to parse an expression tree representing a property path such
		// as can be passed to Include or the Reference/Collection/Property methods of <see cref="InternalEntityEntry" />.
		// This involves parsing simple property accesses like o =&gt; o.Products as well as calls to Select like
		// o =&gt; o.Products.Select(p =&gt; p.OrderLines).
		// </summary>
		// <param name="expression"> The expression to parse. </param>
		// <param name="path"> The expression parsed into an include path, or null if the expression did not match. </param>
		// <returns> True if matching succeeded; false if the expression could not be parsed. </returns>
		public static bool TryParsePath(Expression expression, out string path)
		{
			DebugCheck.NotNull(expression);
			path = null;
			var withoutConvert = expression.RemoveConvert(); // Removes boxing
			var memberExpression = withoutConvert as MemberExpression;
			var callExpression = withoutConvert as MethodCallExpression;

			if (memberExpression != null)
			{
				var thisPart = memberExpression.Member.Name;
				string parentPart;
				if (!TryParsePath(memberExpression.Expression, out parentPart))
				{
					return false;
				}
				path = parentPart == null ? thisPart : (parentPart + "." + thisPart);
			}
			else if (callExpression != null)
			{
				if (callExpression.Method.Name == "Select"
					&& callExpression.Arguments.Count == 2)
				{
					string parentPart;
					if (!TryParsePath(callExpression.Arguments[0], out parentPart))
					{
						return false;
					}
					if (parentPart != null)
					{
						var subExpression = callExpression.Arguments[1] as LambdaExpression;
						if (subExpression != null)
						{
							string thisPart;
							if (!TryParsePath(subExpression.Body, out thisPart))
							{
								return false;
							}
							if (thisPart != null)
							{
								path = parentPart + "." + thisPart;
								return true;
							}
						}
					}
				}
				return false;
			}

			return true;
		}
	}
}
