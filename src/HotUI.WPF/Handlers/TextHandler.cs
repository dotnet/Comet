using WPFLabel = System.Windows.Controls.Label;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.WPF.Handlers
{
    public class TextHandler : AbstractHandler<Text, WPFLabel>
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>()
            {
                [nameof(Text.Value)] = MapValueProperty
            };

        public TextHandler() : base(Mapper)
        {
        }

        protected override WPFLabel CreateView() => new WPFLabel();

        protected override void DisposeView(WPFLabel nativeView)
        {
        }

        public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (WPFLabel)viewHandler.NativeView;
            nativeView.Content = virtualView.Value;
        }
    }
}