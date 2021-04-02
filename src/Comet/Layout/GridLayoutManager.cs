//using System;
//using System.Collections.Generic;

//using System.Linq;
//using Microsoft.Maui.Graphics;
//using Microsoft.Maui.Layouts;

//namespace Comet.Layout
//{
//	public class GridLayoutManager : ILayoutManager
//	{
//		private readonly List<GridConstraints> _constraints = new List<GridConstraints>();
//		private readonly List<object> _definedRows = new List<object>();
//		private readonly List<object> _definedColumns = new List<object>();
//		private Size _lastSize;
//		private double[] _gridX;
//		private double[] _gridY;
//		private double[] _widths;
//		private double[] _heights;
//		private double _width;
//		private double _height;

//		private readonly double _spacing;
//		private readonly Grid grid;

//		public GridLayoutManager(Grid grid,
//			double? spacing)
//		{
//			this.grid = grid;
//			_spacing = spacing ?? 4;
//		}

//		public object DefaultRowHeight { get; set; }

//		public object DefaultColumnWidth { get; set; }

//		public void Invalidate()
//		{
//			_constraints.Clear();
//			_gridX = null;
//			_gridY = null;
//			_widths = null;
//			_heights = null;
//		}

//		public Size Measure(double widthConstraint, double heightConstraint)
//		{
//			var available = new Size(widthConstraint, heightConstraint);
//			var layout = grid;
//			if (_constraints.Count == 0)
//			{
//				var maxRow = 0;
//				var maxColumn = 0;

//				for (var index = 0; index < layout.Count; index++)
//				{
//					var view = layout[index];
//					var constraint = view.GetLayoutConstraints() as GridConstraints ?? GridConstraints.Default;
//					_constraints.Add(constraint);

//					maxRow = Math.Max(maxRow, constraint.Row + constraint.RowSpan - 1);
//					maxColumn = Math.Max(maxColumn, constraint.Column + constraint.ColumnSpan - 1);
//				}

//				while (maxRow >= _definedRows.Count)
//					_definedRows.Add(DefaultRowHeight);

//				while (maxColumn >= _definedColumns.Count)
//					_definedColumns.Add(DefaultColumnWidth);
//			}

//			if (_gridX == null || !_lastSize.Equals(available))
//			{
//				ComputeGrid(available.Width, available.Height);
//				_lastSize = available;
//			}

//			for (var index = 0; index < _constraints.Count; index++)
//			{
//				var position = _constraints[index];
//				var view = layout[index];

//				var x = _gridX[position.Column];
//				var y = _gridY[position.Row];

//				double w = 0;
//				for (var i = 0; i < position.ColumnSpan; i++)
//					w += GetColumnWidth(position.Column + i);

//				double h = 0;
//				for (var i = 0; i < position.RowSpan; i++)
//					h += GetRowHeight(position.Row + i);

//				if (position.WeightX < 1 || position.WeightY < 1)
//				{
//					var viewSize = view.MeasuredSize;

//					if (!view.MeasurementValid)
//						viewSize = view.Measure(widthConstraint,heightConstraint);

//					var cellWidth = w;
//					var cellHeight = h;

//					if (position.WeightX <= 0)
//						w = viewSize.Width;
//					else
//						w *= position.WeightX;

//					if (position.WeightY <= 0)
//						h = viewSize.Height;
//					else
//						h *= position.WeightY;

//					if (position.PositionX > 0)
//					{
//						var availWidth = cellWidth - w;
//						x += (double)Math.Round(availWidth * position.PositionX);
//					}

//					if (position.PositionY > 0)
//					{
//						var availHeight = cellHeight - h;
//						y += (double)Math.Round(availHeight * position.PositionY);
//					}

//					view.MeasuredSize = new Size(w, h);
//					view.MeasurementValid = true;
//				}

//				view.Frame = new Rectangle(x, y, w, h);
//			}

//			return new Size(_width, _height);
//		}

//		public void ArrangeChildren(Rectangle rect)
//		{
//			var layout = grid;
//			var measured = rect.Size;
//			var size = rect.Size;
//			if (_gridX == null || !_lastSize.Equals(size))
//			{
//				ComputeGrid(size.Width, size.Height);
//				_lastSize = size;
//			}

//			for (var index = 0; index < _constraints.Count; index++)
//			{
//				var position = _constraints[index];
//				var view = layout[index];

//				var viewSize = view.MeasuredSize;
//				if (!view.MeasurementValid)
//				{
//					view.MeasuredSize = viewSize = view.Measure(measured.Width, measured.Height);
//					view.MeasurementValid = true;
//				}

//				var x = _gridX[position.Column];
//				var y = _gridY[position.Row];

//				double w = 0;
//				for (var i = 0; i < position.ColumnSpan; i++)
//					w += GetColumnWidth(position.Column + i);

//				double h = 0;
//				for (var i = 0; i < position.RowSpan; i++)
//					h += GetRowHeight(position.Row + i);

//				if (position.WeightX < 1 || position.WeightY < 1)
//				{
//					var cellWidth = w;
//					var cellHeight = h;

//					if (position.WeightX <= 0)
//						w = viewSize.Width;
//					else
//						w *= position.WeightX;

//					if (position.WeightY <= 0)
//						h = viewSize.Height;
//					else
//						h *= position.WeightY;

//					if (position.PositionX > 0)
//					{
//						var availWidth = cellWidth - w;
//						x += (double)Math.Round(availWidth * position.PositionX);
//					}

//					if (position.PositionY > 0)
//					{
//						var availHeight = cellHeight - h;
//						y += (double)Math.Round(availHeight * position.PositionY);
//					}
//				}

//				var margin = view.GetMargin();
//				if (!margin.IsEmpty)
//				{
//					x += margin.Left;
//					y += margin.Top;
//					w -= margin.HorizontalThickness;
//					h -= margin.VerticalThickness;
//				}

//				view.Frame = new Rectangle(x, y, w, h);
//			}
//		}

//		public int AddRow(object row)
//		{
//			if (row == null)
//				return -1;

//			_definedRows.Add(row);
//			Invalidate();

//			return _definedRows.Count - 1;
//		}

//		public void AddRows(params object[] rows)
//		{
//			if (rows == null)
//				return;

//			foreach (var row in rows)
//				_definedRows.Add(row ?? DefaultRowHeight);

//			Invalidate();
//		}

//		public void SetRowHeight(int index, object value)
//		{
//			if (index >= 0 && index < _definedRows.Count)
//			{
//				_definedRows[index] = value;
//				Invalidate();
//			}
//		}

//		public int AddColumn(object column)
//		{
//			if (column == null)
//				return -1;

//			_definedColumns.Add(column);

//			Invalidate();
//			return _definedColumns.Count - 1;
//		}

//		public void AddColumns(params object[] columns)
//		{
//			if (columns == null)
//				return;

//			foreach (var column in columns)
//				_definedColumns.Add(column ?? DefaultColumnWidth);

//			Invalidate();
//		}

//		public void SetColumnWidth(int index, object value)
//		{
//			if (index >= 0 && index < _definedColumns.Count)
//			{
//				_definedColumns[index] = value;
//				Invalidate();
//			}
//		}

//		private double GetColumnWidth(int column)
//		{
//			return _widths[column];
//		}

//		private double GetRowHeight(int row)
//		{
//			return _heights[row];
//		}

//		private void ComputeGrid(double width, double height)
//		{
//			var rows = _definedRows.Count;
//			var columns = _definedColumns.Count;

//			_gridX = new double[columns];
//			_gridY = new double[rows];
//			_widths = new double[columns];
//			_heights = new double[rows];
//			_width = 0;
//			_height = 0;

//			double takenX = 0;
//			var calculatedColumns = new List<int>();
//			var calculatedColumnFactors = new List<double>();
//			for (var c = 0; c < columns; c++)
//			{
//				var w = _definedColumns[c];
//				if (!w.ToString().EndsWith("*", StringComparison.Ordinal))
//				{
//					if (double.TryParse(w.ToString(), out var value))
//					{
//						takenX += value;
//						_widths[c] = value;
//					}
//					else
//					{
//						calculatedColumns.Add(c);
//						calculatedColumnFactors.Add(GetFactor(w));
//					}
//				}
//				else
//				{
//					calculatedColumns.Add(c);
//					calculatedColumnFactors.Add(GetFactor(w));
//				}
//			}

//			var availableWidth = width - takenX;
//			var columnFactor = calculatedColumnFactors.Sum(f => f);
//			var columnWidth = availableWidth / columnFactor;
//			var factorIndex = 0;
//			foreach (var c in calculatedColumns)
//			{
//				_widths[c] = columnWidth * calculatedColumnFactors[factorIndex++];
//			}

//			double takenY = 0;
//			var calculatedRows = new List<int>();
//			var calculatedRowFactors = new List<double>();
//			for (var r = 0; r < rows; r++)
//			{
//				var h = _definedRows[r];
//				if (!h.ToString().EndsWith("*", StringComparison.Ordinal))
//				{
//					if (double.TryParse(h.ToString(), out var value))
//					{
//						takenY += value;
//						_heights[r] = value;
//					}
//					else
//					{
//						calculatedRows.Add(r);
//						calculatedRowFactors.Add(GetFactor(h));
//					}
//				}
//				else
//				{
//					calculatedRows.Add(r);
//					calculatedRowFactors.Add(GetFactor(h));
//				}
//			}

//			var availableHeight = height - takenY;
//			var rowFactor = calculatedRowFactors.Sum(f => f);
//			var rowHeight = availableHeight / rowFactor;
//			factorIndex = 0;
//			foreach (var r in calculatedRows)
//			{
//				_heights[r] = rowHeight * calculatedRowFactors[factorIndex++];
//			}

//			double x = 0;
//			for (var c = 0; c < columns; c++)
//			{
//				_gridX[c] = x;
//				x += _widths[c];
//			}

//			double y = 0;
//			for (var r = 0; r < rows; r++)
//			{
//				_gridY[r] = y;
//				y += _heights[r];
//			}

//			_width = _widths.Sum();
//			_height = _heights.Sum();
//		}

//		private double GetFactor(object value)
//		{
//			if (value != null)
//			{
//				var str = value.ToString();
//				if (str.EndsWith("*", StringComparison.Ordinal))
//				{
//					str = str.Substring(0, str.Length - 1);
//					if (double.TryParse(str, out var f))
//					{
//						return f;
//					}
//				}
//			}

//			return 1;
//		}

//		public double CalculateWidth()
//		{
//			double width = 0;

//			if (_widths != null)
//			{
//				foreach (var value in _widths)
//				{
//					width += value;
//				}
//			}
//			else
//			{
//				var columns = _definedColumns.Count;
//				for (var c = 0; c < columns; c++)
//				{
//					var w = _definedColumns[c];
//					if (!"*".Equals(w))
//					{
//						if (double.TryParse(w.ToString(), out var value))
//						{
//							width += value;
//						}
//					}
//				}
//			}

//			return width;
//		}

//		public double CalculateHeight()
//		{
//			double height = 0;

//			if (_heights != null)
//			{
//				foreach (var value in _heights)
//				{
//					height += value;
//				}
//			}
//			else
//			{
//				var rows = _definedRows.Count;
//				for (var r = 0; r < rows; r++)
//				{
//					var h = _definedRows[r];
//					if (!"*".Equals(h))
//					{
//						if (double.TryParse(h.ToString(), out var value))
//						{
//							height += value;
//						}
//					}
//				}
//			}

//			return height;
//		}

//	}
//}
