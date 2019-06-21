using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HotForms {
	public abstract class ViewBuilder : State {
		public ViewBuilder ()
		{
			StateChanged = Reload;
		}
		protected abstract View Build ();
		public void Reload ()
		{
			ReBuildView ();
		}

		public View View {
			get => GetProperty<View> ();
			protected set {
				if (SetProperty (value)) {
					ViewHandler?.SetView (value);
				}
			}
		}

		public void ReBuildView()
		{
			var oldView = View;
			BindingState.Clear ();
			using (new StateBuilder (this)) {
				var newView = Build ();
				if (oldView != null) {
					newView.DiffUpdate (oldView);
				}
				View = newView;
			}
		}


		IViewBuilderHandler formsView;
		public IViewBuilderHandler ViewHandler {
			get => formsView;
			set {
				if (formsView == value)
					return;
				formsView?.Remove (View);
				formsView = value;
				formsView?.SetView (View);
			}
		}

		protected void ViewPropertyChanged (string property, object value)
		{
			ViewHandler?.UpdateValue (property, value);
		}

	}



public abstract class StateViewBuilder {

		State state;
		public State State {
			get {
				if (state == null) {
					state = new State ();
					CreateState (state);
					//state.StateChanged = Reload;
					state.ResetChangeDictionary ();
				}
				return state;
			}
			set => state = value;
		}

		protected abstract void CreateState (dynamic state);

		protected abstract View Build (dynamic state);
	}

	public class BindingState {
		public List<string> GlobalProperties { get; set; } = new List<string> ();
		public Dictionary<string, List<Action<string,object>>> ViewUpdateProperties = new Dictionary<string, List<Action<string,object>>> ();
		public void AddGlobalProperty (string property)
		{
			Debug.WriteLine ($"Adding Global Property: {property}");
			GlobalProperties.Add (property);
		}
		public void AddGlobalProperties(IEnumerable<string> properties)
		{
			foreach(var prop in properties)
				AddGlobalProperty (prop);
		}
		public void AddViewProperty(string property, Action<string,object> update)
		{
			if (!ViewUpdateProperties.TryGetValue (property, out var actions))
				ViewUpdateProperties[property]  = actions = new List<Action<string,object>> ();
			actions.Add (update);
		}

		public void AddViewProperty (string[] properties, Action<string,object> update)
		{
			foreach(var property in properties) {
				AddViewProperty (property, update);
			}
		}
		public void Clear()
		{
			GlobalProperties?.Clear ();
			foreach(var key  in ViewUpdateProperties) {
				key.Value.Clear ();
			}
			ViewUpdateProperties.Clear ();
		}
		/// <summary>
		/// This returns true, if it updated the UI based on the changes
		/// False, if it couldnt update, or the value was global so the whole UI needs refreshed
		/// </summary>
		/// <param name="updates"></param>
		/// <returns></returns>
		public bool UpdateValues(IEnumerable<(string property,object value)> updates)
		{
			bool didUpdate = true;
			foreach(var update in updates) {
				if (GlobalProperties.Contains (update.property))
					return false;
				if(ViewUpdateProperties.TryGetValue(update.property, out var actions)) {
					foreach (var a in actions)
							a.Invoke (update.property, update.value);
					didUpdate = true;
				}
			}
			return didUpdate;
		}
	}
}
