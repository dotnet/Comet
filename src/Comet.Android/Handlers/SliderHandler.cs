using System;
using Android.Content;
using Android.Widget;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.Android.Handlers
{
	public class SliderHandler : AbstractControlHandler<Slider, SeekBar>
	{
		public static readonly PropertyMapper<Slider> Mapper = new PropertyMapper<Slider>(ViewHandler.Mapper)
		{
			[nameof(Slider.Value)] = MapValueProperty,
			[nameof(Slider.From)] = MapFromProperty,
			[nameof(Slider.Through)] = MapThroughProperty,
		};

		public SliderHandler() : base(Mapper)
		{

		}

		protected override SeekBar CreateView(Context context)
		{
			var slider = new SeekBar(context);
			slider.ProgressChanged += HandleValueChanged;
			return slider;
		}

		protected override void DisposeView(SeekBar nativeView)
		{
			nativeView.ProgressChanged -= HandleValueChanged;
		}

		private void HandleValueChanged(object sender, EventArgs e)
			=> VirtualView?.ValueChanged(TypedNativeView.Progress);

		public static void MapValueProperty(IViewHandler viewHandler, Slider virtualView)
		{
			var nativeView = (SeekBar)viewHandler.NativeView;
			nativeView.Progress = (int)virtualView.Value;
		}

		public static void MapFromProperty(IViewHandler viewHandler, Slider virtualView)
		{
			var nativeView = (SeekBar)viewHandler.NativeView;
			nativeView.Min = (int)virtualView.From;
		}

		public static void MapThroughProperty(IViewHandler viewHandler, Slider virtualView)
		{
			var nativeView = (SeekBar)viewHandler.NativeView;
			nativeView.Max = (int)virtualView.Through;
		}
	}
}
