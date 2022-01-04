using System.Linq;
using Comet.Layout;
using Microsoft.Maui.Layouts;

namespace Comet;
public class VGrid : AbstractLayout, IAutoGrid
{
	public VGrid(int columns,
		float? spacing = null,
		object defaultColumnWidth = null,
		object defaultRowHeight = null)
	{
		columnCount = columns;
		Spacing = spacing;
		var layout = (Layout.GridLayoutManager)LayoutManager;
		layout.DefaultRowHeight = defaultRowHeight ?? "*";
		layout.DefaultColumnWidth = defaultColumnWidth ?? "*";
		//layout.AddColumns(Enumerable.Range(0,columns));
	}
	public VGrid(object[] columns,
		float? spacing = null,
		object defaultColumnWidth = null,
		object defaultRowHeight = null)
	{
		columnCount = columns.Length;
		Spacing = spacing;
		var layout = (Layout.GridLayoutManager)LayoutManager;
		layout.DefaultRowHeight = defaultRowHeight ?? "*";
		layout.DefaultColumnWidth = defaultColumnWidth ?? "*";
		layout.AddColumns(columns);
	}
	public float? Spacing { get; }
	readonly int columnCount;

	public void SetupConstraints(View view, ref int currentColumn, ref int currentRow, ref GridConstraints constraint)
	{

		if (constraint.Row > 0)
			currentRow = constraint.Row;
		if (constraint.Column > 0)
			currentColumn = constraint.Column;

		if (constraint.Row > 0)
			currentRow = constraint.Row;
		if (constraint.Column > 0)
		if (view.GetIsNextRow())
		{
			currentRow++;
			currentColumn = 0;
		}
		else if (view.GetIsNextColumn())
		{
			currentColumn++;
		}

		var columnsNeeded = constraint.ColumnSpan + currentColumn;
		if (columnsNeeded > columnCount)
		{
			currentRow++;
			currentColumn = 0;
		}
		constraint.Column = currentColumn;
		constraint.Row = currentRow;
		currentColumn += constraint.ColumnSpan;
	}
	protected override ILayoutManager CreateLayoutManager() => new Comet.Layout.GridLayoutManager(this, Spacing);
}

