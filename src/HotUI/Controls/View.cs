using System;
using System.Collections.Generic;
using System.Linq;

namespace HotUI {

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

		public bool IsViewHandlerCreated => viewHandler != null;

		IViewHandler viewHandler;
		public IViewHandler ViewHandler {
			get => viewHandler;
			set {
				if (viewHandler == value)
					return;
				viewHandler?.Remove (this);
				viewHandler = value;
				WillUpdateView ();
				viewHandler?.SetView (this);
			}
		}
		internal void UpdateFromOldView(IViewHandler handler) => viewHandler = handler;

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
