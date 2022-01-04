using Comet.Layout;
using Microsoft.Maui.Layouts;

namespace Comet;

public class HGrid : AbstractLayout, IAutoGrid
{
	public HGrid(int rows,
		float? spacing = null,
		object defaultRowHeight = null,
		object defaultColumnWidth = null)
	{
		rowCount = rows;
		Spacing = spacing;
		var layout = (Layout.GridLayoutManager)LayoutManager;
		layout.DefaultRowHeight = defaultRowHeight ?? "*";
		layout.DefaultColumnWidth = defaultColumnWidth ?? "*";
	}
	public HGrid(object[] rows,
		float? spacing = null,
		object defaultRowHeight = null,
		object defaultColumnWidth = null)
	{
		Spacing = spacing;
		var layout = (Layout.GridLayoutManager)LayoutManager;
		layout.DefaultRowHeight = defaultRowHeight ?? "*";
		layout.DefaultColumnWidth = defaultColumnWidth ?? "*";
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

		if (view.GetIsNextColumn())
		{
			currentColumn += currentColumnSpan;
			currentColumnSpan = 1;
			currentRow = 0;
		}
		else if (view.GetIsNextRow())
		{
			currentRow++;
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