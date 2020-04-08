using System.Collections.Generic;
using System.Drawing;
using System.Maui.Graphics;

namespace System.Maui
{
	public abstract class Shape : ContextualObject
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
