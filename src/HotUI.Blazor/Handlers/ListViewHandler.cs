using HotUI.Blazor.Components;
using System.Collections.Generic;

namespace HotUI.Blazor.Handlers
{
    internal class ListViewHandler : BlazorHandler<ListView, BContainerView>
    {
        public static readonly PropertyMapper<ListView> Mapper = new PropertyMapper<ListView>
        {
            { "List", MapListProperty }
        };

        public ListViewHandler()
            : base(Mapper)
        {
        }

        public static void MapListProperty(IViewHandler viewHandler, ListView virtualView)
        {
            var nativeView = (BContainerView)viewHandler.NativeView;

            nativeView.Views = Flatten(virtualView);
        }

        private static IEnumerable<View> Flatten(IListView list)
        {
            for (int section = 0; section < list.Sections(); section++)
            {
                for (int row = 0; row < list.Rows(section); row++)
                {
                    yield return list.ViewFor(section, row);
                }
            }
        }
    }
}
