using System;
using Comet.iOS.Handlers;
using UIKit;

namespace Comet.iOS
{
	public class ActivityIndicatorHandler : AbstractControlHandler<ActivityIndicator, UIActivityIndicatorView>
	{
		public static readonly PropertyMapper<ActivityIndicator> Mapper = new PropertyMapper<ActivityIndicator>(ViewHandler.Mapper)
		{
			[EnvironmentKeys.Colors.Color] = MapColorProperty,
		};

		private static Color DefaultColor;

		public ActivityIndicatorHandler() : base(Mapper)
		{
		}

		protected override UIActivityIndicatorView CreateView()
		{
			var activityIndicator = new UIActivityIndicatorView();
			activityIndicator.StartAnimating();

			if (DefaultColor == null)
			{
				DefaultColor = activityIndicator.Color.ToColor();
			}
			return activityIndicator;
		}

		protected override void DisposeView(UIActivityIndicatorView nativeView)
		{

		}

		public static void MapColorProperty(IViewHandler viewHandler, ActivityIndicator virtualView)
		{
			var nativeView = (UIActivityIndicatorView)viewHandler.NativeView;
			var color = virtualView.GetColor(DefaultColor);
			nativeView.Color = color.ToUIColor();
		}
	}
}
