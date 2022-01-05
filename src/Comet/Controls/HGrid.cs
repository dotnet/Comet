using Comet.Layout;
using Microsoft.Maui.Layouts;

namespace Comet;

public class HGrid : AbstractLayout, IAutoGrid
{
	public HGrid(int rows,
		float? spacing = null,
		object rowHeight = null,
		object columnWidth = null)
	{
		rowCount = rows;
		Spacing = spacing;
		var layout = (Layout.GridLayoutManager)LayoutManager;
		layout.DefaultRowHeight = rowHeight ?? "*";
		layout.DefaultColumnWidth = columnWidth ?? "*";
	}
	public HGrid(object[] rows,
		float? spacing = null,
		object rowHeight = null,
		object columnWidth = null)
	{
		Spacing = spacing;
		var layout = (Layout.GridLayoutManager)LayoutManager;
		layout.DefaultRowHeight = rowHeight ?? "*";
		layout.DefaultColumnWidth = columnWidth ?? "*";
		layout.AddRow(rows);
		rowCount = rows.Length;
	}
	public float? Spacing { get; }
	readonly int rowCount;
	int currentColumnSpan = 1;
	public void SetupConstraints(View view, ref int currentColumn, ref int currentRow, ref GridConstraints constraint)
	{
		//Use values specified
		if (constraint.Row > 0)
			currentRow = constraint.Row;
		if (constraint.Column > 0)
			currentColumn = constraint.Column;



		var columnSkip = view.GetIsNextColumn() - 1;
		if (columnSkip >= 0)
		{
			currentColumn += currentColumnSpan + columnSkip;
			currentColumnSpan = 1;
			currentRow = 0;
		}

		var rowSkip = view.GetIsNextRow();
		if (rowSkip > 0)
		{
			currentRow += rowSkip;
		}

		var rowsNeeded = constraint.RowSpan + currentRow;
		if (rowsNeeded > rowCount)
		{
			currentColumn += currentColumnSpan;
			currentColumnSpan = 1;
			currentRow = 0;
		}

		currentColumnSpan = Math.Max(currentColumnSpan, constraint.ColumnSpan);
		constraint.Column = currentColumn;
		constraint.Row = currentRow;
		currentRow += constraint.RowSpan;

		if (currentRow >= rowCount)
		{
			currentColumn++;
			currentRow = 0;
		}
	}
	protected override ILayoutManager CreateLayoutManager() => new Comet.Layout.GridLayoutManager(this, Spacing);
}