using System;
using System.Drawing;
using System.Linq;
using Comet.Internal;
using SkiaSharp;

namespace Comet.Skia
{
	public class ToggleHandler : SkiaAbstractControlHandler<Toggle>
	{
		static Color defaultThumbOnColor = Color.FromBytes(3, 218, 197, 255);
		static Color defaultThumbOffColor = Color.White;
		static Color defaultTrackOnColor = Color.FromBytes(3, 218, 197, 97);
		static Color defaultTrackOffColor = Color.FromBytes(151, 151, 151, 100);

		const float trackHeight = 14f;
		static float thumbSize = 20;
		static float thumbStretched = 24;
		const float width = 60;
		const float height = 44;
		Shape TrackShape = new Pill(Orientation.Horizontal);
		Shape thumbCircle = new Pill(Orientation.Horizontal);
		RectangleF thumbRect = new RectangleF(0, 0, thumbSize, thumbSize);
		RectangleF trackRect = new RectangleF(0, 0, 34, trackHeight);

		public static new readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle>(SkiaControl.Mapper)
		{
			[nameof(Toggle.IsOn)] = MapIsOnProperty,
		};

		public static DrawMapper<Toggle> ToggleDrawMapper = new DrawMapper<Toggle>(SkiaControl.DrawMapper)
		{
			[SkiaEnvironmentKeys.Toggle.Layers.Track] = DrawTrack,
			[SkiaEnvironmentKeys.Toggle.Layers.Thumb] = DrawThumb,
		};

		protected override string[] LayerDrawingOrder() => DefaultToggleLayerDrawingOrder;

		public static string[] DefaultToggleLayerDrawingOrder =
			DefaultLayerDrawingOrder.ToList().InsertAfter(new string[] {
				SkiaEnvironmentKeys.Toggle.Layers.Track,
				SkiaEnvironmentKeys.Toggle.Layers.Thumb,
				}, SkiaEnvironmentKeys.Text).ToArray();

		public ToggleHandler() : base(ToggleDrawMapper, Mapper) { }

		public override SizeF GetIntrinsicSize(SizeF availableSize) => new SizeF(width, height);

		protected virtual void DrawTrack(SKCanvas canvas, RectangleF rectangle)
		{
			//TODO: Get colors from environment
			var progress = this.GetEnvironment<float>(toggleProgress, 0);
			var fillColor = defaultTrackOffColor.Lerp(defaultTrackOnColor, progress);
			trackRect.Center(rectangle.Center());
			canvas.DrawShape(TrackShape, trackRect, fill: fillColor);
		}

		protected virtual void DrawThumb(SKCanvas canvas, RectangleF rectangle)
		{
			//TODO: Get colors from environment
			thumbRect.Center(rectangle.Center());
			var thumbProgress = this.GetEnvironment<float>(thumbProgressAnimation, 0);
			thumbRect.Width = thumbSize.Lerp(thumbStretched, thumbProgress);

			var offX = trackRect.X;
			var onX = trackRect.Right - thumbRect.Width;
			var progress = this.GetEnvironment<float>(toggleProgress, 0);
			var fillColor = defaultThumbOffColor.Lerp(defaultThumbOnColor, progress);
			var strokeColor = Color.LightGrey.Lerp(Color.Transparent, progress);
			thumbRect.X = offX.Lerp(onX, progress);
			canvas.DrawShape(thumbCircle, thumbRect, Graphics.DrawingStyle.StrokeFill, strokeColor: strokeColor, strokeWidth: .5f, fill: fillColor);
		}

		public override string AccessibilityText() => $"{TypedVirtualView.IsOn?.CurrentValue ?? false}";

		const string toggleProgress = "Toggle.AnimationProgress";
		const string thumbProgressAnimation = "Toggle.ThumbAnimationProgress";
		bool hasSet = false;
		bool currentState;
		public void SetState(bool state)
		{
			currentState = state;
			var progress = state ? 1f : 0f;
			if (!hasSet)
			{
				hasSet = true;
				this.SetEnvironment(toggleProgress, progress);
			}
			this.Animate((t) => {
				this.SetEnvironment(toggleProgress, progress);
			});

		}
		void ToggleValue()
		{
			//TODO: Change this to a method on Toggle
			var binding = TypedVirtualView?.IsOn;
			if (binding != null)
				binding.Set(!currentState);
			else
				SetState(!currentState);
		}

		public override void EndInteraction(PointF[] points, bool inside)
		{
			if (inside)
				ToggleValue();
			this.Animate(t => {
				t.SetEnvironment(thumbProgressAnimation, 0f);
			});
			base.EndInteraction(points, inside);
		}

		public override bool StartInteraction(PointF[] points)
		{
			if (thumbRect.Contains(points))
			{
				this.Animate(t => {
					t.SetEnvironment(thumbProgressAnimation, 1f);
				});
			}
			return base.StartInteraction(points);
		}

		public static void DrawThumb(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, Toggle view)
		{
			var slider = control as ToggleHandler;
			slider?.DrawThumb(canvas, dirtyRect);
		}

		public static void DrawTrack(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, Toggle view)
		{
			var slider = control as ToggleHandler;
			slider?.DrawTrack(canvas, dirtyRect);
		}

		public static void MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
		{
			var control = viewHandler as ToggleHandler;
			control.SetState(virtualView?.IsOn?.CurrentValue ?? false);
		}
	}
}
