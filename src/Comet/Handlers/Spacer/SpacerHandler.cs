using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
#if __IOS__

using PlatformView = UIKit.UIView;
#elif ANDROID
using PlatformView = Android.Views.View;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.Controls.Panel;

#else
using PlatformView = System.Object;
#endif


namespace Comet.Handlers
{
	public partial class SpacerHandler : ViewHandler<Spacer, PlatformView>
	{
		public static readonly PropertyMapper<Spacer, SpacerHandler> Mapper = new PropertyMapper<Spacer, SpacerHandler>(ViewHandler.ViewMapper);
		public SpacerHandler() : base(Mapper)
		{

		}

		protected override PlatformView CreatePlatformView() =>
#if ANDROID
			new PlatformView(Context.ApplicationContext);
#elif WINDOWS
			new LayoutPanel();
#else
			new PlatformView();
#endif

	}
}
