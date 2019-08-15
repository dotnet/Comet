using Comet.Layout;

namespace Comet 
{
	public class Grid : AbstractLayout 
	{
        public Grid(
            object[] columns = null,
            object[] rows = null,
            float? spacing = null,
            object defaultRowHeight = null,
            object defaultColumnWidth = null) : base(new GridLayoutManager(spacing))
        {
            var layout = (GridLayoutManager) LayoutManager;

            layout.DefaultRowHeight = defaultRowHeight ?? "*";
            layout.DefaultColumnWidth = defaultColumnWidth ?? "*";
            
            if (columns != null)
                layout.AddColumns(columns);
            
            if (rows != null)
                layout.AddRows(rows);
        }
    }
}
