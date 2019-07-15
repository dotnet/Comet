using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HotUI {

	public class State<T> : BindingObject 
	{
		public State (T value)
		{
			Value = value;
		}
		
		public State ()
		{		

		}
		
		public T Value 
		{
			get => GetProperty<T> ();
			set => SetProperty (value);
		}
		
		public static implicit operator T(State<T> state) => state.Value;
		public static implicit operator Action<T>(State<T> state) => value => state.Value = value;
		public static implicit operator State<T>(T value) => new State<T>(value);

		public static implicit operator Binding<T>(State<T> state) => new Binding<T>(
			getValue: () => state.Value,
			setValue: state);
	}
	
	public class StateBuilder : IDisposable {
		static List<State> currentStates = new List<State> ();
		public static State CurrentState => currentStates.LastOrDefault ();
		public StateBuilder (State state)
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
	public class State : BindingObjectManager {
		public State()
		{

		}
		internal object GetValue (string property)
		{
			return parent?.GetPropertyValue(property) ?? this.GetPropertyValue (property);
			//var bindingParts = property.Split ('.');
			//var dict = dictionary;
			//for (var i = 0; i < bindingParts.Length - 1; i++) {
			//	var part = bindingParts [i];
			//	dict.TryGetValue (part, out var val);
			//	var child = val as BindingObject;
			//	dict = child.dictionary;
			//}
			//dict.TryGetValue (bindingParts.Last (), out var value);
			//return value;
		}
	}
}
