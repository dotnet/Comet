using System;
using System.Windows;

namespace System.Maui.WPF
{
	public interface WPFViewHandler : IViewHandler
	{
		event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		UIElement View { get; }
	}
}
