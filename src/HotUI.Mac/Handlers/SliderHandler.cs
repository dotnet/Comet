using System;
using AppKit;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.Mac.Handlers
{
    public class SliderHandler : AbstractControlHandler<Slider, NSSlider>
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

        protected override NSSlider CreateView()
        {
            var slider = new NSSlider();
            slider.Continuous = true;
            slider.Activated += HandleValueChanged;
            return slider;
        }

        protected override void DisposeView(NSSlider nativeView)
        {
            nativeView.Activated -= HandleValueChanged;
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            VirtualView?.OnEditingChanged?.Invoke(TypedNativeView.FloatValue);
        }
        
        public static void MapValueProperty(IViewHandler viewHandler, Slider virtualView)
        {
            var nativeView = (NSSlider) viewHandler.NativeView;
            nativeView.FloatValue = virtualView.Value;
        }
        
        public static void MapFromProperty(IViewHandler viewHandler, Slider virtualView)
        {
            var nativeView = (NSSlider) viewHandler.NativeView;
            nativeView.MinValue = virtualView.From;
        }
        
        public static void MapThroughProperty(IViewHandler viewHandler, Slider virtualView)
        {
            var nativeView = (NSSlider) viewHandler.NativeView;
            nativeView.MaxValue = virtualView.Through;
        }
    }
}