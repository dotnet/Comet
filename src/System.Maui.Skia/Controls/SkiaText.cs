using System;
using System.Collections.Generic;
using SkiaSharp;
using Topten.RichTextKit;
using System.Maui.Skia.Internal;
using System.Drawing;

namespace System.Maui.Skia
{
	public static class SkiaTextHelper
	{
		static SKTypeface defaultTypeface = SKTypeface.CreateDefault();
		public static string GetDefaultFontFamily => defaultTypeface.FamilyName;
		public static SizeF GetTextSize(string text, FontAttributes fontAttributes, TextAlignment alignment, LineBreakMode lineBreakMode, float maxWidth, float height = -1)
		{
			var tb = new TextBlock();
			tb.AddText(text, fontAttributes.ToStyle());
			tb.Alignment = alignment.ToTextAlignment();
			tb.MaxWidth = maxWidth;

			tb.MaxLines = null;
			tb.Layout();
			return new SizeF(tb.MeasuredWidth, tb.MeasuredHeight);
		}


		public static SKTypeface ToSKTypeface(this FontAttributes attributes)
			=> SKTypeface.FromFamilyName(attributes.Family ?? "System",
				attributes.Weight.ToSKFontStyleWeight(),
				SKFontStyleWidth.Normal,
				attributes.Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright);


		public static SKFontStyleWeight ToSKFontStyleWeight(this Weight weight) =>
			(SKFontStyleWeight)(int)weight;
	}
}
