using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HotUI {


	public interface INotifyPropertyRead : INotifyPropertyChanged {
		event PropertyChangedEventHandler PropertyRead;
	}
	public class BindingObject : INotifyPropertyRead {

		public event PropertyChangedEventHandler PropertyRead;
		public event PropertyChangedEventHandler PropertyChanged;

		internal protected Dictionary<string, object> dictionary = new Dictionary<string, object> ();

		protected T GetProperty<T> ([CallerMemberName] string propertyName = "")
		{
			PropertyRead?.Invoke (this, new PropertyChangedEventArgs (propertyName));
			if (dictionary.TryGetValue (propertyName, out var val))
				return (T)val;
			return default;
		}

		internal object GetValue (string propertyName)
		{
			dictionary.TryGetValue (propertyName, out var val);
			return val;
		}
		/// <summary>
		/// Returns true if the value changed
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="currentValue"></param>
		/// <param name="newValue"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		protected bool SetProperty<T> (T value, [CallerMemberName] string propertyName = "")
		{
			if (dictionary.TryGetValue (propertyName, out object val)) {
				if (EqualityComparer<T>.Default.Equals ((T)val, value))
					return false;
			}
			dictionary [propertyName] = value;
			OnPropertyChanged?.Invoke ((this, propertyName, value));
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
			return true;
		}

		internal Action<(object Sender, string PropertyName, object Value)> OnPropertyChanged { get; set; }
	}

	public class BindingObjectManager {
		public BindingObjectManager()
		{

		}
		protected View parent;
		public void SetParent (View parent)
		{
			if(this.parent == parent) {
				return;
			}
			this.parent = parent;
			CheckForStateAttributes (parent);
		}
		public BindingState BindingState { get; set; } = new BindingState ();
		internal Action StateChanged;

		bool isBuilding;
		public bool IsBuilding => isBuilding;


		public IEnumerable<KeyValuePair<string, object>> ChangedProperties => changeDictionary;
		Dictionary<string, object> changeDictionary = new Dictionary<string, object> ();
		List<(string property, object value)> pendingUpdates = new List<(string, object)> ();


		List<string> listProperties = new List<string> ();

		internal void ResetChangeDictionary ()
		{
			changeDictionary.Clear ();
		}

		internal void StartBuildingView ()
		{
			CheckForStateAttributes (this);
			isBuilding = true;
			if (listProperties.Any ()) {
				BindingState.AddGlobalProperties (listProperties);
			}
			listProperties.Clear ();
		}

		List<INotifyPropertyRead> children = new List<INotifyPropertyRead> ();
		Dictionary<object, string> childrenProperty = new Dictionary<object, string> ();

		public void StartMonitoring (INotifyPropertyRead obj)
		{
			if (children.Contains (obj))
				return;
			children.Add (obj);
			//Check in for more properties!
			CheckForStateAttributes (obj);

			if (obj is BindingObject bobj) {
				bobj.OnPropertyChanged = (s) => OnPropertyChanged (s.Sender, s.PropertyName, s.Value);
			} else {
				obj.PropertyChanged += Obj_PropertyChanged;
			}
			obj.PropertyRead += Obj_PropertyRead;
		}

		private void Obj_PropertyRead (object sender, PropertyChangedEventArgs e) => OnPropertyRead (sender, e.PropertyName);

		private void Obj_PropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (sender is BindingObject b) {
				OnPropertyChanged (sender, e.PropertyName, b.GetValue (e.PropertyName));
				return;
			}

			var value = sender.GetPropertyValue (e.PropertyName);
			OnPropertyChanged (sender, e.PropertyName, value);
		}

		public void StopMonitoring (INotifyPropertyRead obj)
		{
			if (!children.Contains (obj))
				return;
			children.Remove (obj);
			if (obj is BindingObject b) {
				b.OnPropertyChanged = null;
			} else {
				obj.PropertyChanged -= Obj_PropertyRead;
			}
			obj.PropertyRead -= Obj_PropertyRead;

		}

		internal void EndBuildingView ()
		{
			listProperties.Clear ();
			isBuilding = false;
		}

		internal void StartProperty ()
		{
			isBuilding = true;
			if (listProperties.Any ()) {
				BindingState.AddGlobalProperties (listProperties);
			}
			listProperties.Clear ();
		}

		internal string [] EndProperty (bool endIsBuilding = true)
		{
			if (endIsBuilding)
				isBuilding = false;
			var  changed =  listProperties.Distinct ().ToArray ();
			listProperties.Clear ();
			return changed;

		}

		bool isUpdating;
		public void StartUpdate ()
		{
			isUpdating = true;
		}

		public void EndUpdate ()
		{
			isUpdating = false;
			if (pendingUpdates.Any ()) {
				if (!BindingState.UpdateValues (pendingUpdates)) {
					pendingUpdates.Clear ();
					StateChanged?.Invoke ();
					return;
				}
			}
			pendingUpdates.Clear ();
		}


		void OnPropertyChanged (object sender, string propertyName, object value)
		{
			if (value?.GetType () == typeof (View))
				return;

			changeDictionary [propertyName] = value;
			childrenProperty.TryGetValue (sender, out var parentproperty);
			var prop = string.IsNullOrWhiteSpace (parentproperty) ? propertyName : $"{parentproperty}.{propertyName}";
			pendingUpdates.Add ((prop, value));
			if (!isUpdating) {
				EndUpdate ();
			}

		}

		void OnPropertyRead (object sender, string propertyName)
		{
			if (!isBuilding)
				return;
			childrenProperty.TryGetValue (sender, out var parentproperty);
			var prop = string.IsNullOrWhiteSpace (parentproperty) ? propertyName : $"{parentproperty}.{propertyName}";
			listProperties.Add (prop);
		}


		bool hasChecked;
		static Assembly HotUIAssembly = typeof (BindingObject).Assembly;
		void CheckForStateAttributes (object obj)
		{
			if (hasChecked && obj == this)
				return;
			var type = obj.GetType ();
			//var properties = type.GetProperties (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
			//	Where (x => Attribute.IsDefined (x, typeof (StateAttribute))).ToList ();
			var fields = type.GetFields (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
				//ToList ();
				Where (x => (x.FieldType.Assembly == HotUIAssembly && x.FieldType.Name == "State`1") || Attribute.IsDefined (x, typeof (StateAttribute))).ToList ();
			//if (properties.Any()) {
			//	foreach(var prop in properties) {
			//		var child = prop.GetValue (this) as BindingObject;
			//		if (child != null) {
			//			SetProperty(child,prop.Name);
			//		}
			//	}
			//}

			if (fields.Any ()) {
				foreach (var field in fields) {
					var child = field.GetValue (obj) as INotifyPropertyRead;
					if (child != null) {
						childrenProperty [child] = field.Name;
						StartMonitoring (child);
					}
				}
			}
			Console.WriteLine (type);
			hasChecked = true;
		}

	}

	public class BindingState {
		public List<string> GlobalProperties { get; set; } = new List<string> ();
		public Dictionary<string, List<Action<string, object>>> ViewUpdateProperties = new Dictionary<string, List<Action<string, object>>> ();
		public void AddGlobalProperty (string property)
		{
			if (GlobalProperties.Contains (property))
				return;
			Debug.WriteLine ($"Adding Global Property: {property}");
			GlobalProperties.Add (property);
		}
		public void AddGlobalProperties (IEnumerable<string> properties)
		{
			foreach (var prop in properties)
				AddGlobalProperty (prop);
		}
		public void AddViewProperty (string property, Action<string, object> update)
		{
			if (!ViewUpdateProperties.TryGetValue (property, out var actions))
				ViewUpdateProperties [property] = actions = new List<Action<string, object>> ();
			actions.Add (update);
		}

		public void AddViewProperty (string [] properties, Action<string, object> update)
		{
			foreach (var property in properties) {
				AddViewProperty (property, update);
			}
		}
		public void Clear ()
		{
			GlobalProperties?.Clear ();
			foreach (var key in ViewUpdateProperties) {
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
		public bool UpdateValues (IEnumerable<(string property, object value)> updates)
		{
			bool didUpdate = true;
			foreach (var update in updates) {
				if (GlobalProperties.Contains (update.property))
					return false;
				if (ViewUpdateProperties.TryGetValue (update.property, out var actions)) {
					foreach (var a in actions)
						a.Invoke (update.property, update.value);
					didUpdate = true;
				}
			}
			return didUpdate;
		}
	}
}
