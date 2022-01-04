namespace Comet.Layout
{
	public class GridConstraints
	{
		public static GridConstraints Default => new GridConstraints();

		public GridConstraints(
			int row = 0,
			int column = 0,
			int rowSpan = 1,
			int colSpan = 1,
			float weightX = 1,
			float weightY = 1,
			float positionX = 0,
			float positionY = 0)
		{
			Row = row;
			Column = column;
			RowSpan = rowSpan;
			ColumnSpan = colSpan;
			WeightX = weightX;
			WeightY = weightY;
			PositionX = positionX;
			PositionY = positionY;
		}

		public int Row { get; internal set; }
		public int Column { get; internal set; }
		public int RowSpan { get; }
		public int ColumnSpan { get; }
		public float WeightX { get; }
		public float WeightY { get; }
		public float PositionX { get; }
		public float PositionY { get; }
	}
}
