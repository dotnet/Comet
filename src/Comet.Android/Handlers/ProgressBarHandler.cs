using Android.Content;
using AProgressBar = Android.Widget.ProgressBar;
using AAttribute = Android.Resource.Attribute;

namespace Comet.Android.Handlers
{
	public class ProgressBarHandler : AbstractControlHandler<ProgressBar, AProgressBar>
	{
		public static readonly PropertyMapper<ProgressBar> Mapper = new PropertyMapper<ProgressBar>(ViewHandler.Mapper)
		{
			[nameof(ProgressBar.Value)] = MapValueProperty,
		};

		public ProgressBarHandler() : base(Mapper)
		{
		}

		protected override AProgressBar CreateView(Context context)
		{
			var progressView = new AProgressBar(context, null, AAttribute.ProgressBarStyleHorizontal);
			progressView.Max = 100;
			return progressView;
		}

		public static void MapValueProperty(IViewHandler viewHandler, ProgressBar virtualView)
		{
			var nativeView = (AProgressBar)viewHandler.NativeView;

			nativeView.Progress = (int)(virtualView.Value * 100);
		}

		protected override void DisposeView(AProgressBar nativeView)
		{
		}
	}
}
