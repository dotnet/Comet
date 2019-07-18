using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using UWPScrollView = Windows.UI.Xaml.Controls.ScrollViewer;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.UWP.Handlers
{
    public class ScrollViewHandler : AbstractHandler<ScrollView, UWPScrollView>
    {
        public static readonly PropertyMapper<ScrollView> Mapper = new PropertyMapper<ScrollView>()
            {
                [nameof(ScrollView.View)] = MapViewProperty

        };

        public ScrollViewHandler() : base(Mapper)
        {
        }

        protected override UWPScrollView CreateView()
        {
            return new UWPScrollView();
        }

        public override void Remove(View view)
        {
            TypedNativeView.Content = null;
            base.Remove(view);
        }


        public static void MapViewProperty(IViewHandler viewHandler, ScrollView virtualView)
        {
            var scrollViewHandler = (ScrollViewHandler) viewHandler;
            var nativeView = (UWPScrollView)viewHandler.NativeView;
            var virtualScrollView = scrollViewHandler.VirtualView;
            var content = virtualScrollView?.View?.ToEmbeddableView();
            if (content != null)
                nativeView.Content = content;
        }
    }
}