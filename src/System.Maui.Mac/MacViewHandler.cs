using System;
using AppKit;
using System.Maui.Mac.Controls;

namespace System.Maui.Mac
{
	public interface MacViewHandler : IViewHandler
	{
		event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		NSView View { get; }

		CUIContainerView ContainerView { get; }
	}
}
