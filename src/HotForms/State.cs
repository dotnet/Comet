using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HotForms {

	public interface IState {

	}

	public class StateBuilder : IDisposable {
		static List<State> currentStates = new List<State> ();
		public static State CurrentState => currentStates.LastOrDefault ();
		public StateBuilder(State state)
		{
			State = state;
			state.StartBuildingView ();
			currentStates.Add (state);
		}

		public State State { get; }

		public void Dispose ()
		{
			State.EndBuildingView ();
			currentStates.Remove (State);
		}
	}


	[Serializable]
	public class State : DynamicObject, IState {


		public BindingState BindingState { get; } = new BindingState ();

		public IEnumerable<KeyValuePair<string,object>> ChangedProperties => changeDictionary;

		internal void ResetChangeDictionary ()
		{
			changeDictionary.Clear ();
		}
		internal Action StateChanged;
		Dictionary<string, object> dictionary = new Dictionary<string, object> ();

		protected Dictionary<string, object> changeDictionary = new Dictionary<string, object> ();

		public override bool TryGetMember (GetMemberBinder binder, out object result)
		{
			if (isBuilding)
				listProperties.Add (binder.Name);
			dictionary.TryGetValue (binder.Name, out var val);
			result = val;
			return true;
		}


		public object GetValue (string property)
		{
			dictionary.TryGetValue (property, out var val);
			return val;
		}



		List<(string property, object value)> pendingUpdates = new List<(string, object)> ();
		public override bool TrySetMember (SetMemberBinder binder, object value)
		{
			if (dictionary.TryGetValue (binder.Name, out var val) && val == value)
				return true;
			dictionary [binder.Name] = value;
			changeDictionary [binder.Name] = value;
			pendingUpdates.Add ((binder.Name, value));
			if (!isUpdating) {
				EndUpdate ();
			}
			return true;
		}
		internal void Apply(State state)
		{
			foreach(var pair in state.changeDictionary) {
				changeDictionary [pair.Key] = dictionary [pair.Key] = pair.Value;
			}
		}
		public override IEnumerable<string> GetDynamicMemberNames () => dictionary.Keys;

		protected bool UpdatePropertyValue<T> (ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals (currentValue, newValue))
				return false;
			currentValue = newValue;

			if (!isUpdating)
				EndUpdate ();
			return true;
		}

		public override bool TryConvert (ConvertBinder binder, out object result)
		{
			return base.TryConvert (binder, out result);
		}
		bool isBuilding;
		public bool IsBuilding => isBuilding;
		internal void StartBuildingView()
		{
			isBuilding = true;
			if (listProperties.Any ()) {
				BindingState.AddGlobalProperties (listProperties);
			}
			listProperties.Clear ();
		}
		internal void EndBuildingView()
		{
			listProperties.Clear ();
			isBuilding = false;
		}

		List<string> listProperties = new List<string> ();
		internal void StartProperty()
		{
			isBuilding = true;
			if(listProperties.Any()) {
				BindingState.AddGlobalProperties (listProperties);
			}
			listProperties.Clear ();
		}

		internal string[] EndProperty()
		{
			var props = listProperties.Distinct().ToArray();
			listProperties.Clear ();
			return props;

		}


		bool isUpdating;
		public void StartUpdate()
		{
			isUpdating = true;
		}

		public void EndUpdate ()
		{
			isUpdating = false;

			if (pendingUpdates.Any () && !BindingState.UpdateValues (pendingUpdates)) {
				StateChanged?.Invoke ();
			}
			pendingUpdates.Clear ();
		}
	}
}
