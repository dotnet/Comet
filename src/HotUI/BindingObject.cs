using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HotUI.Helpers;
using HotUI.Reflection;

namespace HotUI
{


    public interface INotifyPropertyRead : INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyRead;
    }
    public class BindingObject : INotifyPropertyRead
    {

        public event PropertyChangedEventHandler PropertyRead;
        public event PropertyChangedEventHandler PropertyChanged;

        internal protected Dictionary<string, object> dictionary = new Dictionary<string, object>();

        protected T GetProperty<T>([CallerMemberName] string propertyName = "")
        {
            CallPropertyRead(propertyName);
            
            if (dictionary.TryGetValue(propertyName, out var val))
                return (T)val;
            return default;
        }

        internal object GetValueInternal(string propertyName)
        {
            dictionary.TryGetValue(propertyName, out var val);
            return val;
        }
        /// <summary>
        /// Returns true if the value changed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(T value, [CallerMemberName] string propertyName = "")
        {
            if (dictionary.TryGetValue(propertyName, out object val))
            {
                if (EqualityComparer<T>.Default.Equals((T)val, value))
                    return false;
            }
            dictionary[propertyName] = value;

            CallPropertyChanged(propertyName, value);

            return true;
        }

        protected virtual void CallPropertyChanged(string propertyName, object value)
        {
            OnPropertyChanged?.Invoke((this, propertyName, value));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void CallPropertyRead(string propertyName)
        {
            PropertyRead?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal bool SetPropertyInternal(object value, [CallerMemberName] string propertyName = "")
        {
            dictionary[propertyName] = value;

            OnPropertyChanged?.Invoke((this, propertyName, value));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }

        internal Action<(object Sender, string PropertyName, object Value)> OnPropertyChanged { get; set; }
    }

    public class BindingObjectManager
    {
        public BindingObjectManager()
        {

        }
        protected View parent;
        public void SetParent(View parent)
        {
            if (this.parent == parent)
            {
                return;
            }
            this.parent = parent;
            CheckForStateAttributes(parent);
        }
        public BindingState BindingState { get; set; } = new BindingState();
        internal Action StateChanged;

        bool isBuilding;
        public bool IsBuilding => isBuilding;


        public IEnumerable<KeyValuePair<string, object>> ChangedProperties => changeDictionary;
        Dictionary<string, object> changeDictionary = new Dictionary<string, object>();
        List<(string property, object value)> pendingUpdates = new List<(string, object)>();


        List<string> listProperties = new List<string>();

        internal void ResetChangeDictionary()
        {
            changeDictionary.Clear();
        }

        internal void StartBuildingView()
        {
            CheckForStateAttributes(this);
            isBuilding = true;
            if (listProperties.Any())
            {
                BindingState.AddGlobalProperties(listProperties);
            }
            listProperties.Clear();
        }

        WeakList<INotifyPropertyRead> children = new WeakList<INotifyPropertyRead>();
        Dictionary<WeakReference, string> childrenProperty = new Dictionary<WeakReference, string>();

        public void StartMonitoring(INotifyPropertyRead obj)
        {
            if (children.Contains(obj))
                return;
            children.Add(obj);
            //Check in for more properties!
            CheckForStateAttributes(obj);

            if (obj is BindingObject bobj)
            {
                bobj.OnPropertyChanged = (s) => OnPropertyChanged(s.Sender, s.PropertyName, s.Value);
            }
            else
            {
                obj.PropertyChanged += Obj_PropertyChanged;
            }
            obj.PropertyRead += Obj_PropertyRead;
        }

        private void Obj_PropertyRead(object sender, PropertyChangedEventArgs e) => OnPropertyRead(sender, e.PropertyName);

        private void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is BindingObject b)
            {
                OnPropertyChanged(sender, e.PropertyName, b.GetValueInternal(e.PropertyName));
                return;
            }

            var value = sender.GetPropertyValue(e.PropertyName);
            OnPropertyChanged(sender, e.PropertyName, value);
        }

        internal void Reset()
        {
            changeDictionary.Clear();
            var watchedChildren = children.ToList();
            foreach (var child in watchedChildren)
            {
                StopMonitoring(child);
            }

        }

        public void StopMonitoring(INotifyPropertyRead obj)
        {
            if (!children.Contains(obj))
                return;
            children.Remove(obj);
            if (obj is BindingObject b)
            {
                b.OnPropertyChanged = null;
            }
            else
            {
                obj.PropertyChanged -= Obj_PropertyRead;
            }
            obj.PropertyRead -= Obj_PropertyRead;

        }

        internal void EndBuildingView()
        {
            listProperties.Clear();
            isBuilding = false;
        }

        internal void StartProperty()
        {
            isBuilding = true;
            if (listProperties.Any())
            {
                BindingState.AddGlobalProperties(listProperties);
            }
            listProperties.Clear();
        }

        internal string[] EndProperty(bool endIsBuilding = true)
        {
            if (endIsBuilding)
                isBuilding = false;
            var changed = listProperties.Distinct().ToArray();
            listProperties.Clear();
            return changed;

        }

        bool isUpdating;
        public void StartUpdate()
        {
            isUpdating = true;
        }

        public void EndUpdate()
        {
            isUpdating = false;
            if (pendingUpdates.Any())
            {
                if (!BindingState.UpdateValues(pendingUpdates))
                {
                    pendingUpdates.Clear();
                    StateChanged?.Invoke();
                    return;
                }
            }
            pendingUpdates.Clear();
        }


        internal void OnPropertyChanged(object sender, string propertyName, object value)
        {
            if (value?.GetType() == typeof(View))
                return;
            var first = childrenProperty.FirstOrDefault(x => x.Key.Target == sender);
            string parentproperty = first.Value;
            var prop = string.IsNullOrWhiteSpace(parentproperty) ? propertyName : $"{parentproperty}.{propertyName}";
            changeDictionary[prop] = value;
            pendingUpdates.Add((prop, value));
            if (!isUpdating)
            {
                EndUpdate();
            }

        }

        internal void OnPropertyRead(object sender, string propertyName)
        {
            if (!isBuilding)
                return;
            var first = childrenProperty.FirstOrDefault(x => x.Key.Target == sender);
            string parentproperty = first.Value;
            var prop = string.IsNullOrWhiteSpace(parentproperty) ? propertyName : $"{parentproperty}.{propertyName}";
            listProperties.Add(prop);
        }


        bool hasChecked;
        static Assembly HotUIAssembly = typeof(BindingObject).Assembly;
        void CheckForStateAttributes(object obj)
        {
            if (hasChecked && obj == this)
                return;
            hasChecked = obj == this;
            var type = obj.GetType();
            //var properties = type.GetProperties (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
            //	Where (x => Attribute.IsDefined (x, typeof (StateAttribute))).ToList ();
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
                //ToList ();
                Where(x => (x.FieldType.Assembly == HotUIAssembly && x.FieldType.Name == "State`1") || Attribute.IsDefined(x, typeof(StateAttribute))).ToList();
            //if (properties.Any()) {
            //	foreach(var prop in properties) {
            //		var child = prop.GetValue (this) as BindingObject;
            //		if (child != null) {
            //			SetProperty(child,prop.Name);
            //		}
            //	}
            //}

            if (fields.Any())
            {
                foreach (var field in fields)
                {
                    var child = field.GetValue(obj) as INotifyPropertyRead;
                    if (child != null)
                    {
                        if (children.Contains(child))
                            continue;
                        childrenProperty[new WeakReference(child)] = field.Name;
                        StartMonitoring(child);
                    }
                }
            }
           // Console.WriteLine(type);
            hasChecked = true;
        }
        internal void DisposingObject(object obj)
        {
            var type = obj.GetType();
            //var properties = type.GetProperties (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
            //	Where (x => Attribute.IsDefined (x, typeof (StateAttribute))).ToList ();
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
                //ToList ();
                Where(x => (x.FieldType.Assembly == HotUIAssembly && x.FieldType.Name == "State`1") || Attribute.IsDefined(x, typeof(StateAttribute))).ToList();
            //if (properties.Any()) {
            //	foreach(var prop in properties) {
            //		var child = prop.GetValue (this) as BindingObject;
            //		if (child != null) {
            //			SetProperty(child,prop.Name);
            //		}
            //	}
            //}

            if (fields.Any())
            {
                foreach (var field in fields)
                {
                    var child = field.GetValue(obj) as INotifyPropertyRead;
                    if (child != null)
                    {
                        childrenProperty.Remove(new WeakReference(child));
                        StopMonitoring(child);
                    }
                }
            }
            //Console.WriteLine(type);
            hasChecked = true;
        }

    }



    public class BindingState
    {
        public HashSet<string> GlobalProperties { get; set; } = new HashSet<string>();
        public Dictionary<string, List<(string PropertyName, WeakReference ViewReference)>> ViewUpdateProperties = new Dictionary<string, List<(string PropertyName, WeakReference ViewReference)>>();
        public void AddGlobalProperty(string property)
        {
            if (GlobalProperties.Add(property))
                Debug.WriteLine($"Adding Global Property: {property}");
        }
        public void AddGlobalProperties(IEnumerable<string> properties)
        {
            foreach (var prop in properties)
                AddGlobalProperty(prop);
        } 
        public void AddViewProperty(string property, string propertyName, View view)
        {
            if (!ViewUpdateProperties.TryGetValue(property, out var actions))
                ViewUpdateProperties[property] = actions = new List<(string PropertyName, WeakReference ViewReference)>();
            actions.Add((propertyName, new WeakReference(view)));
        }

        public void AddViewProperty(string[] properties, View view, string propertyName)
        {
            foreach (var p in properties)
            {
                AddViewProperty(p, propertyName ?? p, view);
            }
        }
        public void Clear()
        {
            GlobalProperties?.Clear();
            foreach (var key in ViewUpdateProperties)
            {
                key.Value.Clear();
            }
            ViewUpdateProperties.Clear();
        }
        /// <summary>
        /// This returns true, if it updated the UI based on the changes
        /// False, if it couldnt update, or the value was global so the whole UI needs refreshed
        /// </summary>
        /// <param name="updates"></param>
        /// <returns></returns>
        public bool UpdateValues(IEnumerable<(string property, object value)> updates)
        {
            bool didUpdate = true;
            foreach (var update in updates)
            {
                if (GlobalProperties.Contains(update.property))
                    return false;
                if (ViewUpdateProperties.TryGetValue(update.property, out var actions))
                {
                    var removed = new List<(string PropertyName, WeakReference ViewReference)> ();
                    Device.InvokeOnMainThread(() => {
                        foreach (var a in actions)
                        {
                            var view = a.ViewReference.Target as View;
                            if (view == null)
                                removed.Add(a);
                            else
                                view.BindingPropertyChanged(a.PropertyName, update.value);
                        }
                    });
                    foreach (var r in removed)
                        actions.Remove(r);
                    didUpdate = true;
                }
            }
            return didUpdate;
        }
    }
}
