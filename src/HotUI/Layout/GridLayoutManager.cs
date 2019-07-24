using System;
using System.Collections.Generic;
using System.Linq;

namespace HotUI.Layout
{
    public class GridLayoutManager : ILayoutManager
    {
        private readonly List<GridConstraints> _constraints = new List<GridConstraints>();
        private readonly List<object> _definedRows = new List<object>();
        private readonly List<object> _definedColumns = new List<object>();
        private SizeF _lastSize;
        private float[] _gridX;
        private float[] _gridY;
        private float[] _widths;
        private float[] _heights;
        private float _width;
        private float _height;
        
        private readonly float _spacing;
    
        public GridLayoutManager(
            float? spacing)
        {
            _spacing = spacing ?? 4;
        }

        public object DefaultRowHeight { get; set; }
        
        public object DefaultColumnWidth { get; set; }

        public void Invalidate()
        {
            _constraints.Clear();
            _gridX = null;
            _gridY = null;
            _widths = null;
            _heights = null;
        }

        public SizeF Measure(AbstractLayout layout, SizeF available)
        {
            if (_constraints.Count == 0)
            {
                var maxRow = 0;
                var maxColumn = 0;
                
                for (var index = 0; index < layout.Count; index++)
                {
                    var view = layout[index];
                    var constraint = view.LayoutConstraints as GridConstraints ?? GridConstraints.Default;
                    _constraints.Add(constraint);
                    
                    maxRow = Math.Max(maxRow, constraint.Row + constraint.RowSpan - 1);
                    maxColumn = Math.Max(maxColumn, constraint.Column + constraint.ColumnSpan - 1);
                }
                
                while (maxRow >= _definedRows.Count)
                    _definedRows.Add(DefaultRowHeight);
                
                while (maxColumn >= _definedColumns.Count)
                    _definedColumns.Add(DefaultColumnWidth);
            }
            
            if (_gridX == null || !_lastSize.Equals(available))
            {
                ComputeGrid(available.Width, available.Height);
                _lastSize = available;
            }

            for (var index = 0; index < _constraints.Count; index++)
            {
                var position = _constraints[index];
                var view = layout[index];

                var x = _gridX[position.Column];
                var y = _gridY[position.Row];

                var w = 0f;
                for (var i = 0; i < position.ColumnSpan; i++)
                    w += GetColumnWidth(position.Column + i);

                var h = 0f;
                for (var i = 0; i < position.RowSpan; i++)
                    h += GetRowHeight(position.Row + i);

                if (position.WeightX < 1 || position.WeightY < 1)
                {
                    var viewSize = view.MeasuredSize;

                    if (!view.MeasurementValid)
                        viewSize = view.Measure(available);

                    var cellWidth = w;
                    var cellHeight = h;

                    if (position.WeightX <= 0)
                        w = viewSize.Width;
                    else
                        w *= position.WeightX;

                    if (position.WeightY <= 0)
                        h = viewSize.Height;
                    else
                        h *= position.WeightY;

                    if (position.PositionX > 0)
                    {
                        var availWidth = cellWidth - w;
                        x += (float)Math.Round(availWidth * position.PositionX);
                    }

                    if (position.PositionY > 0)
                    {
                        var availHeight = cellHeight - h;
                        y += (float)Math.Round(availHeight * position.PositionY);
                    }

                    view.MeasuredSize = new SizeF(w, h);
                    view.MeasurementValid = true;
                }

                view.Frame = new RectangleF(x, y, w, h);
            }

            return new SizeF(_width, _height);
        }

        public void Layout(AbstractLayout layout, SizeF measured)
        {
           var size = measured;
            if (_gridX == null || !_lastSize.Equals(size))
            {
                ComputeGrid(size.Width, size.Height);
                _lastSize = size;
            }

            for (var index = 0; index < _constraints.Count; index++)
            {
                var position = _constraints[index];
                var view = layout[index];

                var x = _gridX[position.Column];
                var y = _gridY[position.Row];

                var w = 0f;
                for (var i = 0; i < position.ColumnSpan; i++)
                    w += GetColumnWidth(position.Column + i);

                var h = 0f;
                for (var i = 0; i < position.RowSpan; i++)
                    h += GetRowHeight(position.Row + i);

                if (position.WeightX < 1 || position.WeightY < 1)
                {
                    var viewSize = view.MeasuredSize;
                    var cellWidth = w;
                    var cellHeight = h;

                    if (position.WeightX <= 0)
                        w = viewSize.Width;
                    else
                        w *= position.WeightX;

                    if (position.WeightY <= 0)
                        h = viewSize.Height;
                    else
                        h *= position.WeightY;

                    if (position.PositionX > 0)
                    {
                        var availWidth = cellWidth - w;
                        x += (float) Math.Round(availWidth * position.PositionX);
                    }

                    if (position.PositionY > 0)
                    {
                        var availHeight = cellHeight - h;
                        y += (float) Math.Round(availHeight * position.PositionY);
                    }
                }

                var padding = view.Padding;
                if (!padding.IsEmpty)
                {
                    x += padding.Left;
                    y += padding.Top;
                    w -= padding.HorizontalThickness;
                    h -= padding.VerticalThickness;
                }

                view.Frame = new RectangleF(x, y, w, h);
            }
        }
        
        public int AddRow(object row)
        {
            if (row == null)
                return -1;

            _definedRows.Add(row);
            Invalidate();            
            
            return _definedRows.Count - 1;
        }

        public void AddRows(params object[] rows)
        {
            if (rows == null)
                return;

            foreach (var row in rows)
                _definedRows.Add(row ?? DefaultRowHeight);

            Invalidate();            
        }

        public void SetRowHeight(int index, object value)
        {
            if (index >= 0 && index < _definedRows.Count)
            {
                _definedRows[index] = value;
                Invalidate();            
            }
        }

        public int AddColumn(object column)
        {
            if (column == null)
                return -1;

            _definedColumns.Add(column);
            
            Invalidate();            
            return _definedColumns.Count - 1;
        }

        public void AddColumns(params object[] columns)
        {
            if (columns == null)
                return;

            foreach (var column in columns)
                _definedColumns.Add(column ?? DefaultColumnWidth);

            Invalidate();            
        }

        public void SetColumnWidth(int index, object value)
        {
            if (index >= 0 && index < _definedColumns.Count)
            {
                _definedColumns[index] = value;
                Invalidate();            
            }
        }
        
        private float GetColumnWidth(int column)
        {
            return _widths[column];
        }

        private float GetRowHeight(int row)
        {
            return _heights[row];
        }

        private void ComputeGrid(float width, float height)
        {
            var rows = _definedRows.Count;
            var columns = _definedColumns.Count;
            
            _gridX = new float[columns];
            _gridY = new float[rows];
            _widths = new float[columns];
            _heights = new float[rows];
            _width = 0;
            _height = 0;
                
            float takenX = 0;
            var calculatedColumns = new List<int>();
            var calculatedColumnFactors = new List<float>();
            for (var c = 0; c < columns; c++)
            {
                var w = _definedColumns[c];
                if (!w.ToString().EndsWith("*"))
                {
                    if (float.TryParse(w.ToString(), out var value))
                    {
                        takenX += value;
                        _widths[c] = value;
                    }
                    else
                    {
                        calculatedColumns.Add(c);
                        calculatedColumnFactors.Add(GetFactor(w));
                    }
                }
                else
                {
                    calculatedColumns.Add(c);
                    calculatedColumnFactors.Add(GetFactor(w));
                }
            }

            var availableWidth = width - takenX;
            var columnFactor = calculatedColumnFactors.Sum(f => f);
            var columnWidth = availableWidth / columnFactor;
            var factorIndex = 0;
            foreach (var c in calculatedColumns)
            {
                _widths[c] = columnWidth * calculatedColumnFactors[factorIndex++];
            }

            float takenY = 0;
            var calculatedRows = new List<int>();
            var calculatedRowFactors = new List<float>();
            for (var r = 0; r < rows; r++)
            {
                var h = _definedRows[r];
                if (!h.ToString().EndsWith("*"))
                {
                    if (float.TryParse(h.ToString(), out var value))
                    {
                        takenY += value;
                        _heights[r] = value;
                    }
                    else
                    {
                        calculatedRows.Add(r);
                        calculatedRowFactors.Add(GetFactor(h));
                    }
                }
                else
                {
                    calculatedRows.Add(r);
                    calculatedRowFactors.Add(GetFactor(h));
                }
            }

            var availableHeight = height - takenY;
            var rowFactor = calculatedRowFactors.Sum(f => f);
            var rowHeight = availableHeight / rowFactor;
            factorIndex = 0;
            foreach (var r in calculatedRows)
            {
                _heights[r] = rowHeight * calculatedRowFactors[factorIndex++];
            }

            float x = 0;
            for (var c = 0; c < columns; c++)
            {
                _gridX[c] = x;
                x += _widths[c];
            }

            float y = 0;
            for (var r = 0; r < rows; r++)
            {
                _gridY[r] = y;
                y += _heights[r];
            }

            _width = _widths.Sum();
            _height = _heights.Sum();
        }

        private float GetFactor(object value)
        {
            if (value != null)
            {
                var str = value.ToString();
                if (str.EndsWith("*"))
                {
                    str = str.Substring(0, str.Length - 1);
                    if (float.TryParse(str, out var f))
                    {
                        return f;
                    }
                }
            }

            return 1;
        }
        
        public float CalculateWidth()
        {
            float width = 0;

            if (_widths != null)
            {
                foreach (var value in _widths)
                {
                    width += value;
                }
            }
            else
            {
                var columns = _definedColumns.Count;
                for (var c = 0; c < columns; c++)
                {
                    var w = _definedColumns[c];
                    if (!"*".Equals(w))
                    {
                        if (float.TryParse(w.ToString(), out var value))
                        {
                            width += value;
                        }
                    }
                }
            }

            return width;
        }

        public float CalculateHeight()
        {
            float height = 0;

            if (_heights != null)
            {
                foreach (var value in _heights)
                {
                    height += value;
                }
            }
            else
            {
                var rows = _definedRows.Count;
                for (var r = 0; r < rows; r++)
                {
                    var h = _definedRows[r];
                    if (!"*".Equals(h))
                    {
                        if (float.TryParse(h.ToString(), out var value))
                        {
                            height += value;
                        }
                    }
                }
            }

            return height;
        }
    }
}