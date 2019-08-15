using UWPLabel = Windows.UI.Xaml.Controls.TextBlock;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.UWP.Handlers
{
    public class TextHandler : AbstractControlHandler<Text, UWPLabel>
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>()
            {
                [nameof(Text.Value)] = MapValueProperty
            };

        public TextHandler() : base(Mapper)
        {
        }

        protected override UWPLabel CreateView() => new UWPLabel();

        protected override void DisposeView(UWPLabel nativeView)
        {
            
        }

        public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (UWPLabel)viewHandler.NativeView;
            nativeView.Text = virtualView.Value;
        }
    }
}
