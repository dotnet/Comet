using System;
using UIKit;
namespace Comet.iOS
{
	public class CUITapGesture : UITapGestureRecognizer
	{
		public CUITapGesture(TapGesture gesture) : base(() => gesture.Invoke())
		{
			gesture.NativeGesture = this;
		}
		//TODO SetGesture Tate
		public override UIGestureRecognizerState State { get => base.State; set => base.State = value; }

	}
}
