using System.Collections.Generic;
using Comet.Layout;
using Microsoft.Maui;
using Microsoft.Maui.Layouts;

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
			Spacing = spacing;
			var layout = (Layout.GridLayoutManager)LayoutManager;

			layout.DefaultRowHeight = defaultRowHeight ?? "*";
			layout.DefaultColumnWidth = defaultColumnWidth ?? "*";

			if (columns != null)
				layout.AddColumns(columns);

			if (rows != null)
				layout.AddRows(rows);
		}

		public float? Spacing { get; }


		protected override ILayoutManager CreateLayoutManager() => new Comet.Layout.GridLayoutManager(this, Spacing);
	}
}
