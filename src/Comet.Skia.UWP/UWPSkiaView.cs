using SkiaSharp.Views.UWP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet.Skia.UWP
{
	public class UWPSkiaView : SkiaSharp.Views.UWP.SKXamlCanvas
	{
		SkiaView _virtualView;
        public SkiaView VirtualView
        {
            get => _virtualView;
            set
            {
                if (_virtualView != null)
                {
                    _virtualView.Invalidated -= HandleInvalidated;
                    _virtualView.NeedsLayout -= NeedsLayout;
                }

                _virtualView = value;

                if (_virtualView != null)
                {
                    _virtualView.Invalidated += HandleInvalidated;
                    _virtualView.NeedsLayout += NeedsLayout;
                }

                HandleInvalidated();
            }
        }
        private void NeedsLayout(object sender, EventArgs e)
        {

        }

        private void HandleInvalidated()
        {
            var control = _virtualView as SkiaControl;
            if (control != null)
            {
                //this.AccessibilityLabel = control.AccessibilityText();
                ////this.AccessibilityTraits = UIAccessibilityTrait.StaticText;
                //this.IsAccessibilityElement = true;
            }
            this.Invalidate();
        }

        static long paintCount = 0;
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            if (_virtualView == null) return;
            var canvas = e.Surface.Canvas;

            canvas.Save();
            var scale = CanvasSize.Width / (float)ActualSize.X;
            canvas.Scale(scale, scale);
            _virtualView.Draw(canvas, new System.Drawing.RectangleF(0,0,ActualSize.X,ActualSize.Y ));
            canvas.Restore();
            Debug.WriteLine($"Painting :{paintCount++}");
        }


    }
}
