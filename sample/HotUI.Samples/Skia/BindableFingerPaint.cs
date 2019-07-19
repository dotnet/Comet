using System;
using System.Collections.Generic;
using HotUI.Skia;
using SkiaSharp;

namespace HotUI.Samples.Skia
{
    public class BindableFingerPaint : SimpleFingerPaint
    {
        private readonly List<List<PointF>> _pointsLists = new List<List<PointF>>();
        private float _strokeWidth = 2;
        private string _strokeColor = "#00FF00";
        
        public BindableFingerPaint (
            Binding<float> strokeSize = null,
            Binding<string> strokeColor = null)
        {
            Bind(strokeSize, nameof(StrokeWidth), value => StrokeWidth = value);
            Bind(strokeColor, nameof(StrokeColor), value => StrokeColor = value);
        }
        
        public float StrokeWidth
        {
            get => _strokeWidth;
            private set
            {
                SetValue(ref _strokeWidth, value); 
                Invalidate();
            }
        }
        
        public string StrokeColor
        {
            get => _strokeColor;
            private set
            {
                SetValue(ref _strokeColor, value); 
                Invalidate();
            }
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