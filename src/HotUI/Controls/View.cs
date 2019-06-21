using System;
using System.Collections.Generic;
using System.Linq;

namespace HotForms {

	public enum LayoutOptions {
		Start,
		Center,
		End,
		Fill,
		StartAndExpand,
		CenterAndExpand,
		EndAndExpand,
		FillAndExpand
	}

	public abstract class View  {

		protected State State { get; set; }

		public View ()
		{
			State = StateBuilder.CurrentState;
			State?.StartBuildingView ();
		}

		public bool IsControlCreated => formsView != null;

		IViewHandler formsView;
		public IViewHandler ViewHandler {
			get => formsView;
			set {
				if (formsView == value)
					return;
				formsView?.Remove (this);
				formsView = value;
				WillUpdateView ();
				formsView?.SetView (this);
			}
		}

		LayoutOptions verticalOptions;
		public LayoutOptions VerticalOptions {
			get => verticalOptions;
			set => this.SetValue (State, ref verticalOptions, value, ViewPropertyChanged);
		}

		LayoutOptions horizontalOptions;
		public LayoutOptions HorizontalOptions{
			get => horizontalOptions;
			set => this.SetValue (State, ref horizontalOptions, value, ViewPropertyChanged);
		}

		protected void ViewPropertyChanged (string property, object value)
		{
			//TODO fix this for real
			var prop = property.Split ('.').Last ();
			ViewHandler?.UpdateValue (prop, value);
		}

		protected virtual void WillUpdateView()
		{

		}

	}
}
