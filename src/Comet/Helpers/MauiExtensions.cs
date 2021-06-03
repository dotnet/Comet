using System;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{
	public static class MauiExtensions
	{
		public static Size GetDesiredSize(this IViewHandler handler, Size size) => handler.GetDesiredSize(size.Width, size.Height);
	}
}
