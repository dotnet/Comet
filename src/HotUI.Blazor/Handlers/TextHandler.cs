using HotUI.Blazor.Components;

namespace HotUI.Blazor.Handlers
{
    internal class TextHandler : BlazorHandler<Text, BLabel>
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>
        {
            [nameof(Text.Value)] = MapValueProperty
        };

        public TextHandler()
            : base(Mapper)
        {
        }

        public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (BLabel)viewHandler.NativeView;

            if (nativeView is null)
            {
                return;
            }

            nativeView.Value = virtualView.Value;
        }
    }
}
