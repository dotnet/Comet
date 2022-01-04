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

	public void SetupConstraints(View view, ref int currentColumn, ref int currentRow, ref GridConstraints constraint)
	{
		//Use values specified
		if (constraint.Row > 0)
			currentRow = constraint.Row;
		if (constraint.Column > 0)
			currentColumn = constraint.Column;

		if (view.GetIsNextColumn())
		{
			currentColumn++;
			currentRow = 0;
		}
		else if (view.GetIsNextRow())
		{
			currentRow++;
		}

		var rowsNeeded = constraint.RowSpan + currentRow;
		if (rowsNeeded > rowCount)
		{
			currentColumn++;
			currentRow = 0;
		}
		constraint.Column = currentColumn;
		constraint.Row = currentRow;
		currentRow += constraint.RowSpan;
	}
	protected override ILayoutManager CreateLayoutManager() => new Comet.Layout.GridLayoutManager(this, Spacing);
}