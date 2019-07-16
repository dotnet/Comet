using FView = Xamarin.Forms.View;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.Forms.Handlers
{
    public class ViewRepresentableHandler : AbstractHandler<ViewRepresentable, FView>
    {
        public static readonly PropertyMapper<ViewRepresentable> Mapper = new PropertyMapper<ViewRepresentable>(ViewHandler.Mapper)
        {
            [nameof(ViewRepresentable.Data)] = MapDataProperty
        };

        public ViewRepresentableHandler() : base(Mapper)
        {

        }

        protected override FView CreateView()
        {
            return VirtualView?.MakeView() as FView;
        }

        protected override void DisposeView(FView nativeView)
        {
            
        }

        public static void MapDataProperty(IViewHandler viewHandler, ViewRepresentable virtualView)
        {
            var data = virtualView.Data;
            virtualView.UpdateView?.Invoke(viewHandler.NativeView, data);
        }
    }
}