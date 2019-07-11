using System;
using UIKit;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
{
    public class ViewRepresentableHandler : AbstractHandler<ViewRepresentable, UIView>
    {
        public static readonly PropertyMapper<ViewRepresentable> Mapper = new PropertyMapper<ViewRepresentable>(ViewHandler.Mapper)
        {
            [nameof(ViewRepresentable.Data)] = MapDataProperty
        };
        
        public ViewRepresentableHandler() : base(Mapper)
        {

        }

        protected override UIView CreateView()
        {
            return VirtualView?.MakeView() as UIView;
        }
        
        public static void MapDataProperty(IViewHandler viewHandler, ViewRepresentable virtualView)
        { 
            var data = virtualView.Data;
            virtualView.UpdateView?.Invoke(viewHandler.NativeView, data);
        }
    }
}