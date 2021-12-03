using System;
using System.Collections.Generic;
using Microsoft.Maui;

namespace Comet
{
	public class CometWindow : ContentView, IWindow
	{
		public float DisplayScale { get; private set; } = 1;
		private IMauiContext mauiContext;
		public IMauiContext MauiContext
		{
			get => mauiContext;
			set
			{
				mauiContext = value;
#if __IOS__

#elif __ANDROID__
				DisplayScale = mauiContext?.Context?.Resources.DisplayMetrics.Density ?? 1;
#endif
			}
		}
		IView IWindow.Content
		{
			get => this.Content;
		}


		public new string Title => this.Content.GetTitle();

		HashSet<IWindowOverlay> _overlays = new HashSet<IWindowOverlay>();

		IVisualDiagnosticsOverlay IWindow.VisualDiagnosticsOverlay { get; }

		IReadOnlyCollection<IWindowOverlay> IWindow.Overlays => _overlays;

		void IWindow.Created()
		{

		}
		void IWindow.Resumed()
		{
		}
		void IWindow.Activated()
		{

		}
		void IWindow.Deactivated()
		{

		}
		void IWindow.Stopped()
		{

		}
		void IWindow.Destroying()
		{

		}

		bool IWindow.BackButtonClicked() => true;
		void IWindow.Backgrounding(IPersistedState state)
		{

		}

		bool IWindow.AddOverlay(IWindowOverlay overlay)
		{
			if (overlay is IVisualDiagnosticsOverlay)
				return false;

			// Add the overlay. If it's added, 
			// Initalize the native layer if it wasn't already,
			// and call invalidate so it will be drawn.
			var result = _overlays.Add(overlay);
			if (result)
			{
				overlay.Initialize();
				overlay.Invalidate();
			}

			return result;
		}
		bool IWindow.RemoveOverlay(IWindowOverlay overlay)
		{
			if (overlay is IVisualDiagnosticsOverlay)
				return false;

			var result = _overlays.Remove(overlay);
			if (result)
				overlay.Deinitialize();

			return result;

		}
	}
}
