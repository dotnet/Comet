using System;
using FView = Xamarin.Forms.View;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.Forms
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

        public static void MapDataProperty(IViewHandler viewHandler, ViewRepresentable virtualView)
        {
            var data = virtualView.Data;
            virtualView.UpdateView?.Invoke(viewHandler.NativeView, data);
        }
    }
}