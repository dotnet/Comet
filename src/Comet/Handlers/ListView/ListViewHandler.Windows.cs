using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using UWPListView = Microsoft.UI.Xaml.Controls.ListView;

namespace Comet.Handlers
{
	public partial class ListViewHandler : ViewHandler<IListView, UWPListView>
	{


		public static void MapListViewProperty(IElementHandler viewHandler, IListView virtualView)
		{
			setupView(viewHandler, virtualView);
		}

		static void setupView(IElementHandler viewHandler, IListView virtualView)
		{
			//var nativeView = (UWPListView)viewHandler.NativeView;
			//var sections = virtualView?.Sections() ?? 0;
			//for (var s = 0; s < sections; s++)
			//{
			//	var section = virtualView?.HeaderFor(s);
			//	if (section != null)
			//		nativeView.Items?.Add(new ListViewHandlerItem((ListViewHandler)viewHandler, section));

			//	var rows = virtualView.Rows(s);
			//	for (var r = 0; r < rows; r++)
			//	{
			//		var v = virtualView.ViewFor(s, r);
			//		nativeView.Items?.Add(new ListViewHandlerItem((ListViewHandler)viewHandler, v));
			//	}
			//	var footer = virtualView?.FooterFor(s);
			//	if (footer != null)
			//		nativeView.Items?.Add(new ListViewHandlerItem((ListViewHandler)viewHandler, footer));
			//}
		}

		public static void MapReloadData(IElementHandler viewHandler, IListView virtualView)
		{
			var nativeView = (UWPListView)viewHandler.NativeView;
			nativeView.Items.Clear();
			setupView(viewHandler, virtualView);
		}

		protected override UWPListView CreateNativeView()
		{
			var listView = new UWPListView();
			listView.SelectionChanged += ListView_SelectionChanged;
			return listView;
		}

		private void ListView_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e)
		{

			VirtualView?.OnSelected(0, NativeView.SelectedIndex);
		}
		//public class ListViewHandlerItem : ListViewItem
		//{
		//	public ListViewHandlerItem(ListViewHandler handler, View view)
		//	{
		//		RemoveViewHandlers(view);
		//		var nativeView = new IListCell(view);
		//		Content = nativeView;
		//	}

		//	private void RemoveViewHandlers(View view)
		//	{
		//		view.ViewHandler = null;
		//		if (view is AbstractLayout layout)
		//			foreach (var subview in layout)
		//				RemoveViewHandlers(subview);
		//	}
		//}
	}
}
