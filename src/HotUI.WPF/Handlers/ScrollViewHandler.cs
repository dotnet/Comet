using WPFScrollView = System.Windows.Controls.ScrollViewer;
// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.WPF.Handlers
{
    public class ScrollViewHandler : AbstractHandler<ScrollView, WPFScrollView>
    {
        public static readonly PropertyMapper<ScrollView> Mapper = new PropertyMapper<ScrollView>()
        {
            [nameof(ScrollView.View)] = MapViewProperty

        };

        public ScrollViewHandler() : base(Mapper)
        {
        }

        protected override WPFScrollView CreateView()
        {
            return new WPFScrollView();
        }

        public override void Remove(View view)
        {
            TypedNativeView.Content = null;
            base.Remove(view);
        }

        public static void MapViewProperty(IViewHandler viewHandler, ScrollView virtualView)
        {
            var scrollViewHandler = (ScrollViewHandler)viewHandler;
            var nativeView = (WPFScrollView)viewHandler.NativeView;
            var virtualScrollView = scrollViewHandler.VirtualView;
            var content = virtualScrollView?.View?.ToEmbeddableView();
            if (content != null)
                nativeView.Content = content;
        }
    }
}