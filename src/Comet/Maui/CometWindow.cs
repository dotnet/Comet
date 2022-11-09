using System;
using System.Collections.Generic;
using Comet.Internal;
using Microsoft.Maui;
using Microsoft.Maui.Platform;

namespace Comet
{
	public class CometWindow : ContentView, IWindow, IToolbarElement
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

		bool IWindow.BackButtonClicked()
		{
			var nav = this.Content.FindNavigation();
			nav.Pop();
			return true;
		}
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

		void IWindow.DisplayDensityChanged(float displayDensity) => DisplayScale = displayDensity;
		float IWindow.RequestDisplayDensity() => ViewHandler?.InvokeWithResult(nameof(IWindow.RequestDisplayDensity), new DisplayDensityRequest()) ?? DisplayScale;
		void IWindow.FrameChanged(Rect frame) => this.Frame = frame;

		public static IToolbar Toolbar = new Toolbar(true, true);
		IToolbar IToolbarElement.Toolbar => this.GetProperty<IToolbar>(nameof(IToolbarElement.Toolbar), false) ?? Toolbar;

		FlowDirection IWindow.FlowDirection => this.GetEnvironment<FlowDirection>(nameof(IWindow.FlowDirection));

		double IWindow.X => this.Frame.X;

		double IWindow.Y => this.Frame.Y;

		double IWindow.Width => this.Frame.Width;

		double IWindow.MinimumWidth => this.GetEnvironment<double>(nameof(IWindow.MinimumWidth));

		double IWindow.MaximumWidth => this.GetEnvironment<double?>(nameof(IWindow.MaximumWidth)) ?? double.MaxValue;

		double IWindow.Height => this.Frame.Height;

		double IWindow.MinimumHeight => this.GetEnvironment<double>(nameof(IWindow.MinimumHeight));

		double IWindow.MaximumHeight => this.GetEnvironment<double?>(nameof(IWindow.MaximumHeight)) ?? double.MaxValue;
	}
}
