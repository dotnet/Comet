using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HotForms {
	public static class DatabindingExtensions {
		public static void SetValue<T>(this View view, State state, ref T currentValue, T newValue, Action<object> onUpdate, [CallerMemberName] string propertyName = "")
		{
			if (state?.IsBuilding ?? false) {
				var props = state.EndProperty ();
				var propCount = props.Length;
				//This is databound!
				if (propCount > 0) {
					bool isGlobal = propCount > 1;
					if (propCount == 1) {
						var prop = props [0];
						var stateValue = (T)state.GetValue (prop);
						//1 to 1 binding!
						if (EqualityComparer<T>.Default.Equals (stateValue, newValue)) {
							state.BindingState.AddViewProperty (prop, onUpdate);
						} else {
							Debug.WriteLine ($"Warning: {nameof (propertyName)} is using formated Text. For performance reasons, please switch to TextBinding");
							isGlobal = true;
						}
					} else {
						Debug.WriteLine ($"Warning: {nameof (propertyName)} is using Multiple state Variables. For performance reasons, please switch to TextBinding");
					}

					if (isGlobal) {

						state.BindingState.AddGlobalProperties (props);
					}
				}
			}
			currentValue = newValue;
			onUpdate (newValue);
		}
	}
}
