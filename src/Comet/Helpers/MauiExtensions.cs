using System;
using System.Graphics;

namespace Xamarin.Platform
{
	public static class MauiExtensions
	{
		public static Size GetDesiredSize(this IViewHandler handler, Size size) => handler.GetDesiredSize(size.Width, size.Height);
	}
}
