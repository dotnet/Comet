using System.Collections.Generic;
using HotUI.Skia;
using SkiaSharp;

namespace HotUI.Samples.Skia
{
    public class BindableFingerPaint : SimpleFingerPaint
    {
        private readonly List<List<PointF>> _pointsLists = new List<List<PointF>>();
        private Binding<float> _strokeWidth = 2f;
        private Binding<string> _strokeColor = "#00FF00";
        
        public BindableFingerPaint (
            Binding<float> strokeSize = null,
            Binding<string> strokeColor = null)
        {
            StrokeWidth = strokeSize;
            StrokeColor = strokeColor;
        }
        
        public Binding<float> StrokeWidth
        {
            get => _strokeWidth;
            private set => SetBindingValue(ref _strokeWidth, value);
        }
        
        public Binding<string> StrokeColor
        {
            get => _strokeColor;
            private set => SetBindingValue(ref _strokeColor, value);
        }
        
        public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
        {
            canvas.Clear(SKColors.White);

            var color = new Color(_strokeColor);
            
            var paint = new SKPaint()
            {
                Color = color.ToSKColor(),
                StrokeWidth = StrokeWidth,
                Style = SKPaintStyle.Stroke
            };
            
            foreach (var pointsList in _pointsLists)
            {
                var path = new SKPath();
                for (var i = 0; i < pointsList.Count; i++)
                {
                    var point = pointsList[i];
                    if (i == 0)
                        path.MoveTo(point.X, point.Y);
                    else
                        path.LineTo(point.X, point.Y);
                }
                canvas.DrawPath(path, paint);
            }
        }

        public override bool StartInteraction(PointF[] points)
        {
            var pointsList = new List<PointF> {points[0]};
            _pointsLists.Add(pointsList);
            
            Invalidate();
            return true;
        }

        public override void DragInteraction(PointF[] points)
        {
            var pointsList = _pointsLists[_pointsLists.Count - 1];
            pointsList.Add(points[0]);
            
            Invalidate();
        }

        public override void EndInteraction(PointF[] points)
        {
            var pointsList = _pointsLists[_pointsLists.Count - 1];
            pointsList.Add(points[0]);
            
            Invalidate();
        }

        public void Reset()
        {
            _pointsLists.Clear();
            Invalidate();
        }
    }
}