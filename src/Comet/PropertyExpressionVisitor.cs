using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comet
{
	internal class PropertyExpressionVisitor : ExpressionVisitor
	{
		HashSet<Expression> nodesToSkip = new();
		public PropertyExpressionVisitor(bool isForBody)
		{
			IsForBody = isForBody;
		}

		public List<MemberExpression> MemberExpressions { get; } = new();
		public bool IsForBody { get; }

		public override Expression Visit(Expression? node)
		{
			if (nodesToSkip.Contains(node))
				return node;
			var type = node?.GetType();

			//If we are building the body we need to skip things
			//This is so the contructor/extension method will be able to capture it's own!
			{
				if (node is UnaryExpression ue && IsForBody)
				{
					nodesToSkip.Add(ue.Operand);
					return node;
				}

				if (node is NewExpression ne)
				{
					foreach (var n in ne.Arguments)
						nodesToSkip.Add(n);

					return node;
				}
			}
			AddIfMemberExpression(node);
			return base.Visit(node);
		}
		public IReadOnlyList<(INotifyPropertyChanged o, string property)> GetBoundProperties()
		{
			return MemberExpressions
				.SelectMany(GetObservablePath)
				.Distinct()
				.ToList();
		}

		private IEnumerable<(INotifyPropertyChanged o, string property)> GetObservablePath(MemberExpression member)
		{
			if (member.Expression is MemberExpression parent)
			{
				var parentObject = GetValue(parent);
				foreach (var p in GetObservablePath(parent))
					yield return p;
				yield return (parentObject as INotifyPropertyChanged, $"{member.Member.Name}");
			}



		}
		object GetValue(MemberExpression member)
		{
			var objectMember = Expression.Convert(member, typeof(object));

			var getterLambda = Expression.Lambda<Func<object>>(objectMember);

			var getter = getterLambda.Compile();

			return getter();
		}

		private void AddIfMemberExpression(Expression? node)
		{
			if (node is not MemberExpression member)
			{
				return;
			}
			if(IsObservable(member))
				MemberExpressions.Add(member);
		}

		public static bool IsObservable(MemberExpression member) => IsObservable(member.Member.DeclaringType);

		public static bool IsObservable(Type type) => typeof(INotifyPropertyChanged).IsAssignableFrom(type);

	}
}
