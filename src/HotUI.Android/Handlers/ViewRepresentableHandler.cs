using System;
using Android.Content;
using AView = Android.Views.View;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.Android
{
    public class ViewRepresentableHandler : AbstractHandler<ViewRepresentable, AView>
    {
        public static readonly PropertyMapper<ViewRepresentable> Mapper = new PropertyMapper<ViewRepresentable>(ViewHandler.Mapper)
        {
            [nameof(ViewRepresentable.Data)] = MapDataProperty
        };
        
        public ViewRepresentableHandler() : base(Mapper)
        {

        }

        protected override AView CreateView(Context context)
        {
            return VirtualView?.MakeView() as AView;
        }
        
        public static void MapDataProperty(IViewHandler viewHandler, ViewRepresentable virtualView)
        { 
            var data = virtualView.Data;
            virtualView.UpdateView?.Invoke(viewHandler.NativeView, data);
        }
    }
}