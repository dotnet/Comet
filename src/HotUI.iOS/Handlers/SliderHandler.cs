using System;
using UIKit;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.iOS
{
    public class SliderHandler : AbstractHandler<Slider, UISlider>
    {
        public static readonly PropertyMapper<Slider> Mapper = new PropertyMapper<Slider>(ViewHandler.Mapper)
        {
            [nameof(Slider.Value)] = MapValueProperty,
            [nameof(Slider.Value)] = MapFromProperty,
            [nameof(Slider.Value)] = MapThroughProperty,
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

        private void HandleValueChanged(object sender, EventArgs e)
        {
            VirtualView?.OnEditingChanged?.Invoke(TypedNativeView.Value);
        }
        
        public static void MapValueProperty(IViewHandler viewHandler, Slider virtualView)
        {
            var nativeView = (UISlider) viewHandler.NativeView;
            nativeView.Value = virtualView.Value;
            nativeView.SizeToFit();
        }
        
        public static void MapFromProperty(IViewHandler viewHandler, Slider virtualView)
        {
            var nativeView = (UISlider) viewHandler.NativeView;
            nativeView.MinValue = virtualView.From;
        }
        
        public static void MapThroughProperty(IViewHandler viewHandler, Slider virtualView)
        {
            var nativeView = (UISlider) viewHandler.NativeView;
            nativeView.MaxValue = virtualView.Through;
        }
    }
}