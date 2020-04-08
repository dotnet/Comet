using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWPListView = Windows.UI.Xaml.Controls.ListView;
// ReSharper disable ClassNeverInstantiated.Global

namespace System.Maui.UWP.Handlers
{
	public class ListViewHandler : AbstractHandler<ListView, UWPListView>
	{
		public static readonly PropertyMapper<ListView> Mapper = new PropertyMapper<ListView>()
		{
			["List"] = MapListProperty
		};

		public ListViewHandler() : base(Mapper)
		{

		}

		public IListView ListView => VirtualView;

		protected override UWPListView CreateView()
		{
			var listView = new UWPListView();
			listView.SelectionChanged += HandleSelectionChanged;
			return listView;
		}

		protected override void DisposeView(UWPListView listView)
		{
			listView.SelectionChanged -= HandleSelectionChanged;
		}

		public static void MapListProperty(IViewHandler viewHandler, IListView virtualView)
		{
			var nativeView = (UWPListView)viewHandler.NativeView;
			var sections = virtualView?.Sections() ?? 0;
			for (var s = 0; s < sections; s++)
			{
				var section = virtualView?.HeaderFor(s);
				if (section != null)
					nativeView.Items?.Add(new ListViewHandlerItem((ListViewHandler)viewHandler, section));

				var rows = virtualView.Rows(s);
				for (var r = 0; r < rows; r++)
				{
					var v = virtualView.ViewFor(s, r);
					nativeView.Items?.Add(new ListViewHandlerItem((ListViewHandler)viewHandler, v));
				}
				var footer = virtualView?.FooterFor(s);
				if (footer != null)
					nativeView.Items?.Add(new ListViewHandlerItem((ListViewHandler)viewHandler, footer));
			}
		}

		private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListView?.OnSelected(0, TypedNativeView.SelectedIndex);
		}
	}

	public class ListViewHandlerItem : ListViewItem
	{
		public ListViewHandlerItem(ListViewHandler handler, View view)
		{
			RemoveViewHandlers(view);
			var nativeView = new CUIListCell(view);
			Content = nativeView;
		}

		private void RemoveViewHandlers(View view)
		{
			view.ViewHandler = null;
			if (view is AbstractLayout layout)
				foreach (var subview in layout)
					RemoveViewHandlers(subview);
		}
	}
}
