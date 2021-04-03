using System;
using Microsoft.Maui;

namespace Comet
{
	public class CometWindow : IWindow
	{
		public IMauiContext MauiContext { get; set; }
		public IPage Page { get; set; }
	}
}
