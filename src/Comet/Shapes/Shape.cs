using System.Collections.Generic;

using Comet.Graphics;
using Microsoft.Maui.Graphics;

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

		public abstract PathF PathForBounds(Rectangle rect);
	}
}
