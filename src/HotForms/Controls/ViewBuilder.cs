using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HotForms {
	public abstract class ViewBuilder : State {
		protected abstract View Build ();

		public void Reload ()
		{
			ReBuildView ();
		}
		protected View View {
			get => GetProperty<View> ();
			set => SetProperty (value);
		}

		public View ReBuildView()
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
			return View;
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
		public HashSet<string> GlobalProperties { get; set; } = new HashSet<string> ();
		public Dictionary<string, HashSet<Action<object>>> ViewUpdateProperties = new Dictionary<string, HashSet<Action<object>>> ();
		public void AddGlobalProperty (string property)
		{
			GlobalProperties.Add (property);
		}
		public void AddGlobalProperties(IEnumerable<string> properties)
		{
			foreach(var prop in properties)
				GlobalProperties.Add (prop);
		}
		public void AddViewProperty(string property, Action<object> update)
		{
			if (!ViewUpdateProperties.TryGetValue (property, out var actions))
				ViewUpdateProperties[property]  = actions = new HashSet<Action<object>> ();
			actions.Add (update);
		}

		public void AddViewProperty (string[] properties, Action<object> update)
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
							a.Invoke (update.value);
					didUpdate = true;
				}
			}
			return didUpdate;
		}
	}
}
