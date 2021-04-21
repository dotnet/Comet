using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.GraphicsControls
{
	public class MaterialSliderHandler : SliderHandler
    {
        public static DrawMapper<MaterialSliderHandler, ISlider> SliderDrawMapper = new DrawMapper<MaterialSliderHandler, ISlider>(ViewHandler.DrawMapper)
		{
			["Background"] = MapDrawBackground,
			["TrackProgress"] = MapDrawTrackProgress,
			["Thumb"] = MapDrawThumb
		};

		public MaterialSliderHandler() : base(SliderDrawMapper, null)
		{

		}

		public override string[] LayerDrawingOrder() => ViewHandler.DefaultLayerDrawingOrder;

		public static void MapDrawBackground(ICanvas canvas, RectangleF dirtyRect, MaterialSliderHandler handler, IView view) => handler.DrawBackground(canvas, dirtyRect);

		public static void MapDrawTrackProgress(ICanvas canvas, RectangleF dirtyRect, MaterialSliderHandler handler, IView view) => handler.DrawTrackProgress(canvas, dirtyRect);

		public static void MapDrawThumb(ICanvas canvas, RectangleF dirtyRect, MaterialSliderHandler handler, IView view) => handler.DrawThumb(canvas, dirtyRect);

        public virtual void DrawBackground(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            var slider = VirtualView;

            canvas.FillColor = slider.MaximumTrackColor;

            var x = dirtyRect.X;

            var width = dirtyRect.Width;
            var height = 2;

            var y = (float)((slider.Height - height) / 2);

            canvas.FillRoundedRectangle(x, y, width, height, 0);

            canvas.RestoreState();
        }

        public virtual void DrawTrackProgress(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            var slider = VirtualView;

            canvas.FillColor = slider.MinimumTrackColor;

            var x = dirtyRect.X;

            var value = ((double)slider.Value).Clamp(0, 1);
            var width = (float)(dirtyRect.Width * value);

            var height = 2;

            var y = (float)((slider.Height - height) / 2);

            canvas.FillRoundedRectangle(x, y, width, height, 0);

            canvas.RestoreState();
        }

        public virtual void DrawThumb(ICanvas canvas, RectangleF dirtyRect)
        {
			var MaterialFloatThumb = 12f;

            canvas.SaveState();

            var slider = VirtualView;

            var value = ((double)slider.Value).Clamp(0, 1);
            var x = (float)((dirtyRect.Width * value) - (MaterialFloatThumb / 2));

            if (x <= 0)
                x = 0;

            if (x >= dirtyRect.Width - MaterialFloatThumb)
                x = dirtyRect.Width - MaterialFloatThumb;

            var y = (float)((slider.Height - MaterialFloatThumb) / 2);

            canvas.FillColor = slider.ThumbColor;

            canvas.FillEllipse(x, y, MaterialFloatThumb, MaterialFloatThumb);

            canvas.RestoreState();
        }
    }
}