using System;
using System.Collections.Generic;
using System.Linq;
using Comet.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;

namespace Comet
{

	public class CometApp : View, IApplication, IMauiContextHolder
	{
		const string MauiWindowIdKey = "__MAUI_WINDOW_ID__";
		public CometApp()
		{
			CurrentApp = this;
		}
		public static CometApp CurrentApp { get; protected set; }
		public static CometWindow CurrentWindow { get; protected set; }
		public static IMauiContext MauiContext => StateManager.CurrentContext ?? CurrentWindow?.MauiContext ?? ((IMauiContextHolder)CurrentApp).MauiContext;

		public static float DisplayScale => CurrentWindow?.DisplayScale ?? 1;
		List<IWindow> windows = new List<IWindow>();
		public IReadOnlyList<IWindow> Windows => windows;


		IWindow IApplication.CreateWindow(IActivationState activationState)
		{
			((IMauiContextHolder)this).MauiContext = activationState.Context;

			windows.Add(CurrentWindow = new CometWindow
			{
				MauiContext = activationState.Context,
				Content = this,
			});
			return CurrentWindow;
		}

		void IApplication.ThemeChanged()
		{
			//TODO: apply new theme
		}

		IMauiContext IMauiContextHolder.MauiContext { get; set; }

		IReadOnlyList<IWindow> IApplication.Windows => windows;
		void IApplication.OpenWindow(IWindow window)
		{
			if (window is CometWindow cwindow)
				OpenWindow(cwindow);
		}

		public virtual void OpenWindow(CometWindow window)
		{


			var state = new PersistedState
			{
				[MauiWindowIdKey] = window.Id
			};

			ViewHandler?.Invoke(nameof(IApplication.OpenWindow), new OpenWindowRequest(State: state));
		}

		void IApplication.CloseWindow(IWindow window)
		{

		}
}
}
