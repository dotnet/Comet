using Xamarin.Forms;
using FLabel = Xamarin.Forms.Label;

// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Forms.Handlers
{
    public class TextHandler : AbstractControlHandler<Text, FLabel>
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>(ViewHandler.Mapper)
        {
            [nameof(Comet.Text.Value)] = MapValueProperty,
            [EnvironmentKeys.Fonts.Font] = MapFontProperty,
            [EnvironmentKeys.Colors.Color] = MapColorProperty,
        };

        public TextHandler() : base(Mapper)
        {

        }

        protected override FLabel CreateView() => new FLabel();
        
        protected override void DisposeView(Label nativeView)
        {
            
        }
        
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
