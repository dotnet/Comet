using Android.Content;
using AView = Android.Views.View;
using AScrollView = Android.Widget.ScrollView;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using static Android.Views.ViewGroup;

namespace Comet.Handlers
{
	public partial class ScrollViewHandler : ViewHandler<ScrollView, AScrollView>
	{

		private AView _content;

		protected override AScrollView CreateNativeView() => new AScrollView(Context);
		
		protected override void DisconnectHandler(AScrollView view)
		{
			if (_content != null)
			{
				view.RemoveView(_content);
				_content = null;
			}

			base.DisconnectHandler(view);
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
