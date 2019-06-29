using System;
using System.Collections.Generic;
using System.Linq;

namespace HotUI {

	public class View {
		HashSet<string> usedEnvironmentData = new HashSet<string> ();
		internal static readonly EnvironmentData Environment = new EnvironmentData ();
		internal readonly EnvironmentData Context = new EnvironmentData ();
		View parent;
		public View Parent {
			get => parent;
			set {
				if (parent == value)
					return;
				parent = value;
				OnParentChange (value);
			}
		}
		protected virtual void OnParentChange(View parent)
		{
			this.Navigation = parent.Navigation ?? parent as NavigationView;
		}
		public NavigationView Navigation { get; set; }
		protected State State { get; set; }
		public View (bool hasConstructors)
		{
			Context.View = this;
			State = StateBuilder.CurrentState ?? new State {
				StateChanged = ResetView
			};
			State.StartMonitoring (View.Environment);
			State.StartMonitoring (this.Context);
			SetEnvironmentFields ();
			if (!hasConstructors)
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
		internal void UpdateFromOldView (IViewHandler handler) => ViewHandler = handler;
		View builtView;
		public View BuiltView => builtView;
		void ResetView()
		{
			if (usedEnvironmentData.Any ())
				SetEnvironmentFields ();
			var oldView = builtView;
			builtView = null;
			if (ViewHandler == null)
				return;
			ViewHandler.Remove (this);
			var view = this.GetRenderView ().Diff (oldView);
			ViewHandler?.SetView (view);
		}

		Func<View> body;
		public Func<View> Body {
			get => body;
			set => this.SetValue(State,ref body, value, (s,o)=> ResetView());
		}
		internal View GetView () => GetRenderView ();

		protected virtual View GetRenderView ()
		{
			if (Body == null)
				return this;
			if (builtView != null)
				return builtView;
			using (new StateBuilder (State)) {
				State.SetParent (this);
				State.StartProperty ();
				var view = Body.Invoke ();
				view.Parent = this;
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
			this.SetPropertyValue (property, value);
			ViewHandler?.UpdateValue (property, value);
		}

		public static void SetGlobalEnvironment (string key, object value) => Environment.SetValue (key, value);
		public static void SetGlobalEnvironment (IDictionary<string, object> data)
		{
			foreach(var pair in data)
				Environment.SetValue (pair.Key, pair.Value);
		}
		public static T GetGlobalEnvironment<T> (string key) => Environment.GetValue<T> (key);

		void SetEnvironmentFields ()
		{
			var fields = this.GetFieldsWithAttribute (typeof (EnvironmentAttribute));
			if (!fields.Any ())
				return;
			foreach(var f in fields) {
				var attribute = f.GetCustomAttributes (true).OfType<EnvironmentAttribute> ().FirstOrDefault();
				var key = attribute.Key ?? f.Name;
				var value = this.GetEnvironment (key);
				State.BindingState.AddGlobalProperty (key);
				usedEnvironmentData.Add (key);
				if (value == null) {
					//Lets try again with first letter uppercased;
					key = key.FirstCharToUpper ();
					value = this.GetEnvironment (key);
					if (value != null) {
						usedEnvironmentData.Add (key);
						State.BindingState.AddGlobalProperty (key);
					}
				} 
				
				if (value != null)
					f.SetValue (this, value);

			}
		}
	}
}
