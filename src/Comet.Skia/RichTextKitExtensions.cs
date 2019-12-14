using System;
using Style = Topten.RichTextKit.Style;
using RTextAlignment = Topten.RichTextKit.TextAlignment;
using TextLine = Topten.RichTextKit.TextLine;

namespace Comet.Skia.Internal
{
	public static class RichTextKitExtensions
	{
		public static Style ToStyle(this FontAttributes attributes, Color textColor = null)
		{
			return new Style
			{
				FontFamily = attributes.Family,
				FontItalic = attributes.Italic,
				FontSize = attributes.Size,
				TextColor = (textColor ?? Color.Black).ToSKColor(),
				FontWeight = (int)attributes.Weight,
			};
		}

		public static RTextAlignment ToTextAlignment(this TextAlignment alignment)
			=> alignment switch
			{
				TextAlignment.Center => RTextAlignment.Center,
				TextAlignment.Left => RTextAlignment.Left,
				TextAlignment.Right => RTextAlignment.Right,
				_ => RTextAlignment.Auto,
			};


	}
}
