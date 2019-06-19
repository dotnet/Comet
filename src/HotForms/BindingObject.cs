using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HotForms {

	public interface INotifyPropertyRead : INotifyPropertyChanged {
		event PropertyChangedEventHandler PropertyRead;
	}

	public class BindingObject : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;

		internal Action StateChanged;

		internal protected Action<BindingObject,List<(string property, object value)>> UpdateParentValueChanged;
		internal protected string ParentProperty { get; set; }

		public BindingState BindingState { get; } = new BindingState ();

		public IEnumerable<KeyValuePair<string, object>> ChangedProperties => changeDictionary;

		internal void ResetChangeDictionary ()
		{
			changeDictionary.Clear ();
		}


		Dictionary<string, object> changeDictionary = new Dictionary<string, object> ();
		internal protected Dictionary<string, object> dictionary = new Dictionary<string, object> ();
		List<(string property, object value)> pendingUpdates = new List<(string, object)> ();



		bool isBuilding;
		public bool IsBuilding => isBuilding;

		internal void StartBuildingView ()
		{
			CheckForStateAttributes ();
			isBuilding = true;
			bindableChildren.ForEach (x => x.StartBuildingView ());
			if (listProperties.Any ()) {
				BindingState.AddGlobalProperties (listProperties);
			}
			listProperties.Clear ();
		}

		bool hasChecked = false;
		void CheckForStateAttributes()
		{
			if (hasChecked)
				return;
			var type = this.GetType ();
			var properties = type.GetProperties (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
				Where (x => Attribute.IsDefined (x, typeof (StateAttribute))).ToList ();
			var fields = type.GetFields (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
				Where (x => Attribute.IsDefined (x, typeof (StateAttribute))).ToList ();
			if (properties.Any()) {
				foreach(var prop in properties) {
					var child = prop.GetValue (this) as BindingObject;
					if (child != null) {
						SetProperty(child,prop.Name);
					}
				}
			}

			if (fields.Any ()) {
				foreach (var field in fields) {
					var child = field.GetValue (this) as BindingObject;
					if (child != null) {
						SetProperty (child, field.Name);
					}
				}
			}
			Console.WriteLine (type);
			hasChecked = true;
		}

		internal void EndBuildingView ()
		{

			bindableChildren.ForEach (x => x.EndBuildingView ());
			listProperties.Clear ();
			isBuilding = false;
		}

		List<string> listProperties = new List<string> ();
		internal void StartProperty ()
		{
			isBuilding = true;
			if (listProperties.Any ()) {
				BindingState.AddGlobalProperties (listProperties);
			}
			listProperties.Clear ();

			bindableChildren.ForEach (x => x.StartProperty ());
		}

		internal string [] EndProperty (bool includeParent = false)
		{
			var children = bindableChildren.SelectMany (x => x.EndProperty (true));
			var props = listProperties.Select(x=> includeParent ? $"{ParentProperty}.{x}" : x).Union(children).Distinct ().ToArray ();
			listProperties.Clear ();
			return props;

		}

		//private List<string> CheckChildrenProperties(List<string> properties)
		//{
		//	List<string> realProperties = new List<string> ();
		//	foreach(var prop in properties) {
		//		if(bindableChildren.TryGetValue(prop, out var child)) {
		//			var childProps = 
		//		}
		//		realProperties.Add (prop);
		//	}
		//}


		bool isUpdating;
		public void StartUpdate ()
		{
			isUpdating = true;
		}

		public void EndUpdate ()
		{
			isUpdating = false;


			if (pendingUpdates.Any ()) {
				if (UpdateParentValueChanged != null) {
					UpdateParentValueChanged (this,pendingUpdates);
				} else if (!BindingState.UpdateValues (pendingUpdates)) {
					StateChanged?.Invoke ();
				}
			}
			pendingUpdates.Clear ();
		}


		protected T GetProperty<T> ([CallerMemberName] string propertyName = "")
		{
			if (isBuilding) {
				listProperties.Add (propertyName);
			}

			if (dictionary.TryGetValue (propertyName, out var val))
				return (T)val;
			return default (T);
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
					return true;
				if (val is BindingObject oldChild) {
					UntrackChild (propertyName, oldChild);
				}
			}
			if (value is BindingObject b) {
				TrackChild (propertyName, b);
			}
			dictionary [propertyName] = value;
			changeDictionary [propertyName] = value;
			//If this is tied to a parent, we need to send that notification as well
			if(!string.IsNullOrWhiteSpace(ParentProperty))
				pendingUpdates.Add (($"{ParentProperty}.{propertyName}", value));
			pendingUpdates.Add ((propertyName, value));
			if (!isUpdating) {
				EndUpdate ();
			}
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
			return true;
		}

		List<BindingObject> bindableChildren = new List<BindingObject> ();

		void TrackChild (string propertyName, BindingObject child)
		{
			//This should fail if there are two properties with the same binding object as it's value. But that should never happen!
			child.ParentProperty = propertyName;			
			child.UpdateParentValueChanged = Child_PropertiesChanged;
			bindableChildren.Add (child);
		}

		private void Child_PropertiesChanged (BindingObject child, List<(string property, object value)> changes)
		{
			pendingUpdates.AddRange (changes);
			EndUpdate ();
		}

		void UntrackChild (string propertyName, BindingObject child)
		{
			child.UpdateParentValueChanged = null;
			child.ParentProperty = null;
			bindableChildren.Remove (child);
		}



	}
}
