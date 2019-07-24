using HotUI.Blazor.Components;
using System;
using System.Collections.Generic;

namespace HotUI.Blazor.Handlers
{
    internal class ListViewHandler : BlazorHandler<ListView, BListView>
    {
        public static readonly PropertyMapper<ListView> Mapper = new PropertyMapper<ListView>
        {
            { "List", MapListProperty },
        };

        public ListViewHandler()
            : base(Mapper)
        {
        }

        public static void MapListProperty(IViewHandler viewHandler, ListView virtualView)
        {
            var nativeView = (BListView)viewHandler.NativeView;

            nativeView.List = virtualView;
        }
    }
}
