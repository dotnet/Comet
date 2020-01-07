using System;
using System.Drawing;
using System.Threading.Tasks;
using Comet.Skia.Internal;
using SkiaSharp;
using Topten.RichTextKit;

namespace Comet.Skia
{
	public class TextFieldHandler : SkiaAbstractControlHandler<TextField>, ITextFieldHandler
	{
		public static new readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>(SkiaControl.Mapper)
		{
			[nameof(Comet.TextField.Text)] = MapResetText,
			[EnvironmentKeys.Colors.Color] = MapResetText
		};
		public int CaretPosition { get; set; }
		public TextFieldHandler() : base(null, Mapper)
		{

		}
		static float hPadding = 40;
		static float minHPadding = 10;
		static float vPadding = 10;

		static FontAttributes defaultFont = new FontAttributes
		{
			Family = SkiaTextHelper.GetDefaultFontFamily,
			Size = 16,
			Weight = Weight.Regular,
		};


		public override string AccessibilityText() => TypedVirtualView?.Text;

		TextBlock textBlock;
		public TextBlock TextBlock
		{
			get => textBlock ??= CreateTextBlock();
			set => textBlock = value;
		}

		public VerticalAlignment VerticalAlignment => VerticalAlignment.Center;

		public bool IsSecure => throw new NotImplementedException();

		public TextBlock CreateTextBlock()
		{
			var font = VirtualView.GetFont(defaultFont);
			var color = VirtualView.GetColor(Color.Black);
			var alignment = VirtualView.GetTextAlignment(TextAlignment.Center) ?? TextAlignment.Center;
			var tb = new TextBlock();
			tb.AddText(TypedVirtualView.Text?.CurrentValue, font.ToStyle(color));
			tb.MaxWidth = VirtualView.Frame.Width;
			tb.MaxHeight = VirtualView.Frame.Height;
			tb.MaxLines = null;
			tb.Alignment = alignment.ToTextAlignment();
			tb.Layout();
			return tb;
		}

		bool isVisible;
		string curserPercent = "textFieldCurserFade";

		public override void ViewDidDisappear()
		{
			isVisible = false;
			base.ViewDidDisappear();
		}
		bool carretIsVisible = false;
		protected override void DrawText(TextBlock tb, SKCanvas canvas, VerticalAlignment verticalAlignment)
		{
			base.DrawText(tb, canvas, verticalAlignment);
			if (!carretIsVisible)
				return;
			var curser = tb.GetCaretInfo(CaretPosition);
			var percent = this.GetEnvironment<float>(curserPercent,false);
			var cursorColor = Color.Blue;
			using var paint = new SKPaint
			{
				IsAntialias = true,
				StrokeWidth = 1.5f,
				Style = SKPaintStyle.Fill,
				Color = cursorColor.Lerp(cursorColor.WithAlpha(.2f), percent).ToSKColor()
			};
			var rect = curser.CaretRectangle;
			var y = verticalAlignment switch
			{
				VerticalAlignment.Bottom => VirtualView.Frame.Height - tb.MeasuredHeight,
				VerticalAlignment.Center => (VirtualView.Frame.Height - tb.MeasuredHeight) / 2,
				_ => 0
			};
			var start = new SKPoint(rect.Right, rect.Top + y);
			var end = new SKPoint(rect.Left, rect.Bottom + y);


			canvas.DrawLine(start, end, paint);
			//Console.WriteLine(percent);
		}

		public override bool StartInteraction(PointF[] points)
		{
			UpdateCaret(points);
			return base.StartInteraction(points);
		}
		public override void DragInteraction(PointF[] points)
		{
			UpdateCaret(points);
			base.DragInteraction(points);
		}
		void UpdateCaret(PointF[] points)
		{
			var first = points[0];
			var result = TextBlock.HitTest(first.X, first.Y);
			CaretPosition = result.ClosestCodePointIndex;
		}

		public void InsertText(string text)
		{
			var oldText = TypedVirtualView.Text?.CurrentValue ?? string.Empty;
			var newText = oldText.Insert(CaretPosition,text);
			TypedVirtualView.ValueChanged(newText);
		}

		public void Backspace()
		{
			var oldText = TypedVirtualView.Text?.CurrentValue ?? string.Empty;
			var pos = CaretPosition - 1;
			if (pos < 0)
				return;
			CaretPosition = pos;
			var newText = oldText.Remove(pos, 1);
			TypedVirtualView.ValueChanged(newText);
		}
		public void StartInput()
		{
			carretIsVisible = true;
			this.RemoveAnimations();
			this.SetEnvironment(curserPercent, 0f,false);
			(this).Animate((t) => {
				t.SetEnvironment(curserPercent, 1f,false);
			}, repeats: true, autoReverses: true, duration: .5f);
		}

		public async void EndInput()
		{
			carretIsVisible = false;
			this.RemoveAnimations();
			this.SetEnvironment(curserPercent, 0f,false);
			MapResetText(this, VirtualView);
		}
	}
}
