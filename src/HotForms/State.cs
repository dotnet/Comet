using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HotForms {

	public interface IState {
	}

	[Serializable]
	public class State : DynamicObject, IState {

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
			dictionary.TryGetValue (binder.Name, out var val);
			result = val;
			return true;
		}
		public override bool TrySetMember (SetMemberBinder binder, object value)
		{

			if (dictionary.TryGetValue (binder.Name, out var val) && val == value)
				return true;
			dictionary [binder.Name] = value;
			changeDictionary [binder.Name] = value;
			if (!isUpdating)
				EndUpdate ();
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

		bool isUpdating;
		public void StartUpdate()
		{
			isUpdating = true;
		}

		public void EndUpdate ()
		{
			isUpdating = false;
			StateChanged?.Invoke ();
		}
	}
}
