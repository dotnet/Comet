using System;
using System.Collections.Generic;
using SkiaSharp;

namespace Comet.Skia
{
    public static class SkiaText
    {
        public static List<LineInfo> GetLines(string text,FontAttributes fontAttributes,TextAlignment alignment, LineBreakMode lineBreakMode,float maxWidth, float height = -1)
        {

			var lineHeight = (float)(fontAttributes.Size * 1.25f);

			var paint = new SKPaint
			{
				Color = Color.Black.ToSKColor(),
				IsAntialias = true,
				TextSize = fontAttributes.Size,
			};
			

			paint.Typeface = fontAttributes.ToSKTypeface();


			var lines = new List<LineInfo>();


			var remaining = text;
			float y = paint.TextSize;
			float x = 0;
			while (!string.IsNullOrEmpty(remaining))
			{
				paint.BreakText(remaining, (float)maxWidth, out var measuredWidth, out var measuredText);

				if (measuredText.Length == 0)
					break;

				if (lineBreakMode == LineBreakMode.NoWrap)
				{
					lines.Add(new LineInfo(measuredText, measuredWidth, lineHeight, new SKPoint(x, y)));
					break;
				}
				else if (lineBreakMode == LineBreakMode.WordWrap)
				{
					if (measuredText.Length != remaining.Length)
					{
						for (int i = measuredText.Length - 1; i >= 0; i--)
						{
							if (char.IsWhiteSpace(measuredText[i]))
							{
								measuredText = measuredText.Substring(0, i + 1);
								break;
							}
						}
					}

					lines.Add(new LineInfo(measuredText, paint.MeasureText(measuredText), lineHeight, new SKPoint(x, y)));
				}
				else if (lineBreakMode == LineBreakMode.HeadTruncation)
				{
					throw new NotImplementedException();
				}
				else if (lineBreakMode == LineBreakMode.TailTruncation)
				{
					throw new NotImplementedException();
				}
				else if (lineBreakMode == LineBreakMode.MiddleTruncation)
				{
					throw new NotImplementedException();
				}

				remaining = remaining.Substring(measuredText.Length);

				y += lineHeight;
			}

            if (lines.Count > 0 && height > 0 && (alignment != TextAlignment.Left))
            {
                float vOffset = 0;
                //switch (data.VAlign)
                //{
                //    case TextAlignment.Center:
                        vOffset = (float)(height - (lines.Count * lineHeight)) / 2f;
                //        break;

                //    case TextAlignment.End:
                //        vOffset = (float)(data.Rect.Height - (lines.Count * lineHeight));
                //        break;
                //}

                foreach (var line in lines)
                {
                    float hOffset = 0;
                    switch (alignment)
                    {
                        case TextAlignment.Center:
                            hOffset = (float)((maxWidth - line.Width) / 2);
                            break;

                        case TextAlignment.Right:
                            hOffset = (float)(maxWidth - line.Width);
                            break;
                    }

                    line.Origin = new SKPoint(line.Origin.X + hOffset, line.Origin.Y + vOffset);
                }
            }
            return lines;
        }

        public static SKTypeface ToSKTypeface(this FontAttributes attributes)
            => SKTypeface.FromFamilyName(attributes.Family ?? "System",
                attributes.Weight.ToSKFontStyleWeight(),
				SKFontStyleWidth.Normal,
				attributes.Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright);


		public static SKFontStyleWeight ToSKFontStyleWeight(this Weight weight) =>
			(SKFontStyleWeight)(int)weight;
	}

	public class LineInfo
	{
		public LineInfo(string text, float width, float height, SKPoint origin)
		{
			Text = text;
			Width = width;
			Origin = origin;
			Height = height;
		}

		public float Height { get; set; }
		public SKPoint Origin { get; set; }
		public string Text { get; set; }
		public float Width { get; set; }
	}
}
