using System;
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

		bool IWindow.BackButtonPressed() => true;
	}
}
