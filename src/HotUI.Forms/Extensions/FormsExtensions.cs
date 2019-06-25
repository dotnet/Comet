using System;
using FImage = Xamarin.Forms.Image;
using FButton = Xamarin.Forms.Button;
using FLabel = Xamarin.Forms.Label;
using FStack = Xamarin.Forms.StackLayout;
using FEntry = Xamarin.Forms.Entry;
using FWebview = Xamarin.Forms.WebView;
using FView = Xamarin.Forms.View;
using FPage = Xamarin.Forms.Page;
using HotUI;

namespace HotUI.Forms {
	public static class FormsExtensions {
		static FormsExtensions()
		{
			UI.Init ();
		}
		public static FPage ToPage (this View view)
		{
			var v = view.ToIFormsView ();
			return new HotUIPage {
				Content = v?.View,
			};
		}

		public static FView ToForms (this View view) => view.ToIFormsView ()?.View;

		public static IFormsView ToIFormsView(this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null) {

				handler = Registrar.Handlers.GetRenderer (view.GetType ()) as IViewHandler;
				view.ViewHandler = handler;
			}
			return handler as IFormsView;
		}

	}
}