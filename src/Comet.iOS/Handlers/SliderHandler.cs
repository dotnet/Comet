using System;
using UIKit;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.iOS.Handlers
{
	public class SliderHandler : AbstractControlHandler<Slider, UISlider>
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

		protected override UISlider CreateView()
		{
			var slider = new UISlider();
			slider.Continuous = true;
			slider.ValueChanged += HandleValueChanged;
			return slider;
		}

		protected override void DisposeView(UISlider nativeView)
		{
			nativeView.ValueChanged -= HandleValueChanged;
		}

		private void HandleValueChanged(object sender, EventArgs e) =>
			VirtualView.ValueChanged(TypedNativeView.Value);

		public static void MapValueProperty(IViewHandler viewHandler, Slider virtualView)
		{
			var nativeView = (UISlider)viewHandler.NativeView;
			nativeView.Value = virtualView.Value.CurrentValue;
		}

		public static void MapFromProperty(IViewHandler viewHandler, Slider virtualView)
		{
			var nativeView = (UISlider)viewHandler.NativeView;
			nativeView.MinValue = virtualView.From.CurrentValue;
		}

		public static void MapThroughProperty(IViewHandler viewHandler, Slider virtualView)
		{
			var nativeView = (UISlider)viewHandler.NativeView;
			nativeView.MaxValue = virtualView.Through.CurrentValue;
		}
	}
}
