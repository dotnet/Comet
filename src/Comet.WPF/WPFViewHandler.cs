using System;
using System.Windows;

namespace Comet.WPF
{
	public interface WPFViewHandler : IViewHandler
	{
		event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		UIElement View { get; }
	}
}
