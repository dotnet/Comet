using System;
using HotUI;
using Xamarin.Forms;
using FLabel = Xamarin.Forms.Label;
using HView = HotUI.View;

namespace HotUI.Forms
{
    public class TextHandler : AbstractHandler<Text, FLabel>
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>(ViewHandler.Mapper)
        {
            [nameof(HotUI.Text.Value)] = MapValueProperty,
            [EnvironmentKeys.Fonts.Font] = MapFontProperty,
            [EnvironmentKeys.Colors.Color] = MapColorProperty,
        };

        public TextHandler() : base(Mapper)
        {

        }

        protected override FLabel CreateView() => new FLabel();


        public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (FLabel)viewHandler.NativeView;
            nativeView.Text = virtualView.Value;
        }

        public static void MapFontProperty(IViewHandler viewHandler, Text virtualView)
        {
        }

        public static void MapColorProperty(IViewHandler viewHandler, Text virtualView)
        {
        }
    }
}
