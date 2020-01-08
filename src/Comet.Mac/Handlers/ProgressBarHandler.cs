using System;
using AppKit;
using Comet.Mac.Handlers;

namespace Comet.Mac.Handlers
{
	public class ProgressBarHandler : AbstractControlHandler<ProgressBar, NSProgressIndicator>
	{
		public static readonly PropertyMapper<ProgressBar> Mapper = new PropertyMapper<ProgressBar>(ViewHandler.Mapper)
		{
			[nameof(ProgressBar.Value)] = MapValueProperty,
		};

		public ProgressBarHandler() : base(Mapper)
		{

		}

		protected override NSProgressIndicator CreateView()
		{
			var progressView = new NSProgressIndicator()
			{
				Style = NSProgressIndicatorStyle.Bar,
				MinValue = 0,
				MaxValue = 1,
				Indeterminate = false,
			};
			return progressView;
		}


		public static void MapValueProperty(IViewHandler viewHandler, ProgressBar virtualView)
		{
			var nativeView = (NSProgressIndicator)viewHandler.NativeView;

			nativeView.DoubleValue = virtualView.Value?.CurrentValue ?? 0;
		}

		protected override void DisposeView(NSProgressIndicator nativeView)
		{

		}
	}
}
