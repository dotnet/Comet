using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using AScrollView = Android.Widget.ScrollView;

namespace Comet.Android.Controls
{
	public class CometScrollView : AScrollView
	{
		public CometScrollView(Context context) : base(context)
		{
		}

		internal Action<Rectangle> CrossPlatformArrange { get; set; }

		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			if (CrossPlatformArrange == null || Context == null)
			{
				return;
			}

			var deviceIndependentLeft = Context.FromPixels(left);
			var deviceIndependentTop = Context.FromPixels(top);
			var deviceIndependentRight = Context.FromPixels(right);
			var deviceIndependentBottom = Context.FromPixels(bottom);

			var destination = Rectangle.FromLTRB(0, 0,
				deviceIndependentRight - deviceIndependentLeft, deviceIndependentBottom - deviceIndependentTop);

			CrossPlatformArrange(destination);
		}
	}
}
