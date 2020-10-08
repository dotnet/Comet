using System.Collections.Generic;
using System.Drawing;
using Comet.Graphics;

namespace Comet
{
	public abstract class Shape : ContextualObject
	{
		protected Shape()
		{

		}

		internal override void ContextPropertyChanged(string property, object value, bool cascades)
		{

		}

		public abstract PathF PathForBounds(Xamarin.Forms.Rectangle rect);
	}
}
