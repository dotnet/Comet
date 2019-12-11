using System;
using Topten.RichTextKit;

namespace Comet.Skia
{
	public interface ITextHandler
	{
		TextBlock TextBlock { get; set; }
		TextBlock CreateTextBlock();
		VerticalAlignment VerticalAlignment { get; }
	}
}
