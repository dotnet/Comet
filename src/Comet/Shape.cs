using System.Collections.Generic;
using System.Graphics;
using Comet.Graphics;
using Xamarin.Platform.Shapes;

namespace Comet
{
	public abstract class Shape : ContextualObject, IShape
	{
		protected Shape()
		{

		}

		internal override void ContextPropertyChanged(string property, object value, bool cascades)
		{

		}

		public abstract PathF PathForBounds(RectangleF rect);
	}
}
