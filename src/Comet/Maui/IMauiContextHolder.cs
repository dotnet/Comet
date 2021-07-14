using System;
using Microsoft.Maui;

namespace Comet
{
	public interface IMauiContextHolder
	{
		IMauiContext MauiContext {get;set;}
	}
}
