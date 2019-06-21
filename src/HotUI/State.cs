using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HotUI {

	[AttributeUsage (AttributeTargets.Field)]
	public class StateAttribute : Attribute {

	}

	public class State<T> : BindingObject {
		public State(T value)
		{
			Value = value;
		}
		public State ()
		{

		}
		public T Value {
			get => GetProperty<T> ();
			set => SetProperty (value);
		}
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
	public class State : BindingObject {
		internal object GetValue (string property) {
			var bindingParts = property.Split ('.');
			var dict = dictionary;
			for(var i = 0; i < bindingParts.Length - 1; i++) {
				var part = bindingParts [i];
				dict.TryGetValue (part, out var val);
				var child = val as BindingObject;
				dict = child.dictionary;
			}
			dict.TryGetValue (bindingParts.Last(), out var value);
			return value;
		}
	}
}
