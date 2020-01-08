using System;
using UIKit;

namespace Comet.iOS.Handlers
{
	public class ProgressBarHandler : AbstractControlHandler<ProgressBar, UIProgressView>
	{
		public static readonly PropertyMapper<ProgressBar> Mapper = new PropertyMapper<ProgressBar>(ViewHandler.Mapper)
		{
			[nameof(ProgressBar.Value)] = MapValueProperty,
		};

		public ProgressBarHandler() : base(Mapper)
		{
		}

		protected override UIProgressView CreateView()
		{
			var progressView = new UIProgressView();

			return progressView;
		}


		public static void MapValueProperty(IViewHandler viewHandler, ProgressBar virtualView)
		{
			var nativeView = (UIProgressView)viewHandler.NativeView;

			nativeView.Progress = virtualView.Value?.CurrentValue ?? 0;
		}

		protected override void DisposeView(UIProgressView nativeView)
		{

		}
	}
}
