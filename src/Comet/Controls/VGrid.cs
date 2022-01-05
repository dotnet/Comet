using System.Linq;
using Comet.Layout;
using Microsoft.Maui.Layouts;

namespace Comet;
public class VGrid : AbstractLayout, IAutoGrid
{
	public VGrid(int columns,
		float? spacing = null,
		object columnWidth = null,
		object rowHeight = null)
	{
		columnCount = columns;
		Spacing = spacing;
		var layout = (Layout.GridLayoutManager)LayoutManager;
		layout.DefaultRowHeight = rowHeight ?? "*";
		layout.DefaultColumnWidth = columnWidth ?? "*";
		//layout.AddColumns(Enumerable.Range(0,columns));
	}
	public VGrid(object[] columns,
		float? spacing = null,
		object columnWidth = null,
		object rowHeight = null)
	{
		columnCount = columns.Length;
		Spacing = spacing;
		var layout = (Layout.GridLayoutManager)LayoutManager;
		layout.DefaultRowHeight = rowHeight ?? "*";
		layout.DefaultColumnWidth = columnWidth ?? "*";
		layout.AddColumns(columns);
	}
	public float? Spacing { get; }
	readonly int columnCount;
	int currentRowSpan = 1;

	public void SetupConstraints(View view, ref int currentColumn, ref int currentRow, ref GridConstraints constraint)
	{

		if (constraint.Row > 0)
			currentRow = constraint.Row;
		if (constraint.Column > 0)
			currentColumn = constraint.Column;
		//currentRowSpan = 1, so we will use that value.
		var rowSkip = view.GetIsNextRow() -1;
		if (rowSkip >= 0)
		{
			currentRow += currentRowSpan + rowSkip;
			currentRowSpan = 1;
			currentColumn = 0;
		}

		var columnSkip = view.GetIsNextColumn();
		if (columnSkip > 0)
		{
			currentColumn += columnSkip;
		}

		var columnsNeeded = constraint.ColumnSpan + currentColumn;
		if (columnsNeeded > columnCount)
		{
			currentRow += currentRowSpan;
			currentRowSpan = 1;
			currentColumn = 0;
		}

		currentRowSpan = Math.Max(currentRowSpan, constraint.RowSpan);
		constraint.Column = currentColumn;
		constraint.Row = currentRow;
		currentColumn += constraint.ColumnSpan;
		if (currentColumn >= columnCount)
		{
			currentRow++;
			currentColumn = 0;
		}
	}
	protected override ILayoutManager CreateLayoutManager() => new Comet.Layout.GridLayoutManager(this, Spacing);
}

