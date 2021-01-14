using System;
using System.Graphics;

namespace Xamarin.Platform
{
	public static class MauiExtensions
	{
		public static SizeF GetDesiredSize(this IViewHandler handler, SizeF size) => handler.GetDesiredSize(size.Width, size.Height);
	}
}
