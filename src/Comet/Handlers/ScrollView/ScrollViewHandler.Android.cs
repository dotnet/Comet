using Android.Content;
using AView = Android.Views.View;
using AScrollView = Comet.Android.Controls.CometScrollView;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using static Android.Views.ViewGroup;

namespace Comet.Handlers
{
	public partial class ScrollViewHandler : ViewHandler<ScrollView, AScrollView>
	{

		private AView _content;

		protected override AScrollView CreateNativeView() => new AScrollView(Context)
		{
			CrossPlatformArrange = Arange,
		};
		
		protected override void DisconnectHandler(AScrollView view)
		{
			if (_content != null)
			{
				view.RemoveView(_content);
				_content = null;
			}

			base.DisconnectHandler(view);
		}

		void Arange(Rectangle rect)
		{
			var sizeAllowed = this.VirtualView.Orientation == Orientation.Vertical ? new Size(rect.Width, double.MaxValue) : new Size(double.MaxValue, rect.Height);
			var measuredSize = VirtualView?.Content?.Measure(sizeAllowed.Width, sizeAllowed.Height) ?? Size.Zero;
			//Make sure we at least fit the scroll view
			if (double.IsInfinity(measuredSize.Width))
				measuredSize.Width = rect.Width;
			if (double.IsInfinity(measuredSize.Height))
				measuredSize.Height = rect.Height;
			measuredSize.Width = Math.Max(measuredSize.Width, rect.Width);
			measuredSize.Height = Math.Max(measuredSize.Height, rect.Height);
			if (VirtualView?.Content != null)
				VirtualView.Content.Frame = new Rectangle(Point.Zero, measuredSize);
			//NativeView.v = measuredSize.ToCGSize();
			//_content.Frame = new CGRect(CGPoint.Empty, measuredSize);
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			var newContent = VirtualView.Content?.ToNative(MauiContext);
			if (_content == null || newContent != _content)
			{
				if (_content != null)
					NativeView.RemoveView(_content);

				_content = newContent;

				if (_content != null)
					NativeView.AddView(_content);
			}
		}
	}
}
