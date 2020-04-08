using System;
using Windows.UI.Xaml;

namespace System.Maui.UWP
{
	public interface UWPViewHandler : IViewHandler
	{
		event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		UIElement View { get; }
	}
}
