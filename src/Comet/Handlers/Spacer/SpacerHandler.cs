using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
#if __IOS__

using NativeView = UIKit.UIView;
#elif MONOANDROID
using NativeView = Android.Views.View;
#elif WINDOWS
using NativeView = Microsoft.UI.Xaml.Controls.Panel;

#else
using NativeView = System.Object;
#endif


namespace Comet.Handlers
{
	public partial class SpacerHandler : ViewHandler<Spacer, NativeView>
	{
		public static readonly PropertyMapper<Spacer, SpacerHandler> Mapper = new PropertyMapper<Spacer, SpacerHandler>(ViewHandler.ViewMapper);
		public SpacerHandler() : base(Mapper)
		{

		}

		protected override NativeView CreateNativeView() =>
#if MONOANDROID
			new NativeView(Context.ApplicationContext);
#elif WINDOWS
			new LayoutPanel();
#else
			new NativeView();
#endif

	}
}
