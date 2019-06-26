using System;
using System.Collections.Generic;
using System.Linq;

namespace HotUI {

	public class View {

		protected State State { get; set; }
		public View (bool hasConstructors)
		{
			State = StateBuilder.CurrentState ?? new State {
				StateChanged = ResetView
			};
			if(!hasConstructors)
				State.StartBuildingView ();

		}
		public View () : this (false)
		{
		}


		IViewHandler viewHandler;
		public IViewHandler ViewHandler {
			get => viewHandler;
			set {
				if (viewHandler == value)
					return;
				viewHandler?.Remove (this);
				viewHandler = value;
				WillUpdateView ();
				viewHandler?.SetView (this.GetRenderView());
			}
		}
		internal void UpdateFromOldView (IViewHandler handler) => viewHandler = handler;
		View builtView;
		void ResetView()
		{
			builtView = null;
			if (ViewHandler == null)
				return;
			ViewHandler.Remove (this);
			WillUpdateView ();
			ViewHandler?.SetView (this.GetRenderView ());
		}

		Func<View> body;
		public Func<View> Body {
			get => body;
			set => this.SetValue(State,ref body, value, (s,o)=> ResetView());
		}
		internal View GetView () => GetRenderView ();
		protected View GetRenderView ()
		{
			if (Body == null)
				return this;
			if (builtView != null)
				return builtView;
			using (new StateBuilder (State)) {
				State.SetParent (this);
				State.StartProperty ();
				var view = Body.Invoke ();
				var props = State.EndProperty ();
				var propCount = props.Length;
				if (propCount > 0) {
					State.BindingState.AddViewProperty (props, (s, o) => ResetView ());
				}
				return builtView = view;
			}
		}

		protected virtual void WillUpdateView ()
		{

		}
		protected void ViewPropertyChanged (string property, object value)
		{
			//TODO fix this for real
			var prop = property.Split ('.').Last ();
			//Lets only set this in debug mode. It is really only used by tests.
#if DEBUG
			this.SetPropertyValue (prop, value);
#endif
			ViewHandler?.UpdateValue (prop, value);
		}
	}



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

	//public abstract class View  {

	//	protected State State { get; set; }

	//	public View ()
	//	{
	//		State = StateBuilder.CurrentState;
	//		State?.StartBuildingView ();
	//	}

	//	public bool IsViewHandlerCreated => viewHandler != null;

	//	IViewHandler viewHandler;
	//	public IViewHandler ViewHandler {
	//		get => viewHandler;
	//		set {
	//			if (viewHandler == value)
	//				return;
	//			viewHandler?.Remove (this);
	//			viewHandler = value;
	//			WillUpdateView ();
	//			viewHandler?.SetView (this);
	//		}
	//	}
	//	internal void UpdateFromOldView(IViewHandler handler) => viewHandler = handler;

	//	LayoutOptions verticalOptions;
	//	public LayoutOptions VerticalOptions {
	//		get => verticalOptions;
	//		set => this.SetValue (State, ref verticalOptions, value, ViewPropertyChanged);
	//	}

	//	LayoutOptions horizontalOptions;
	//	public LayoutOptions HorizontalOptions{
	//		get => horizontalOptions;
	//		set => this.SetValue (State, ref horizontalOptions, value, ViewPropertyChanged);
	//	}

	//	protected void ViewPropertyChanged (string property, object value)
	//	{
	//		//TODO fix this for real
	//		var prop = property.Split ('.').Last ();
	//		ViewHandler?.UpdateValue (prop, value);
	//	}

	//	protected virtual void WillUpdateView()
	//	{

	//	}

	//}
}
