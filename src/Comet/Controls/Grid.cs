using Comet.Layout;
using Xamarin.Platform.Layouts;

namespace Comet
{
	public class Grid : AbstractLayout
	{
		public Grid(
			object[] columns = null,
			object[] rows = null,
			float? spacing = null,
			object defaultRowHeight = null,
			object defaultColumnWidth = null)
		{
			var layout = (GridLayoutManager)LayoutManager;

			layout.DefaultRowHeight = defaultRowHeight ?? "*";
			layout.DefaultColumnWidth = defaultColumnWidth ?? "*";

			if (columns != null)
				layout.AddColumns(columns);

			if (rows != null)
				layout.AddRows(rows);
			Spacing = spacing;
		}

		public float? Spacing { get; }

		protected override ILayoutManager CreateLayoutManager() => new GridLayoutManager(this,Spacing);
	}
}
