using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HotUI {

	public static class EnvironmentKeys {
		public const string DocumentsFolder = "DocumentsFolder";
		public const string UserFolder = "UserFolder";
		public const string OS = "OS";

		public static class Fonts
        {
			public const string Font = "Font";
		}

        public static class Colors
        {
            public const string Color = "Color";
            public const string BackgroundColor = "BackgroundColor";
        }

        public static class Layout
        {
            public const string Padding = "Padding";
        }
        
        public static class View
        {
	        public const string ClipShape = "ClipShape";
            public const string Shadow = "Shadow";
            public const string Overlay = "Overlay";
            public const string Title = "Title";
        }

        public static class Shape
        {
            public const string LineWidth = "Shape.LineWidth";
            public const string Color = "Shape.Color";
        }

        public static class TabView
        {
            public const string Image = "TabView.Item.Image";
            public const string Title = "TabView.Item.Title";
        }
    }

	[AttributeUsage (AttributeTargets.Field)]
	public class EnvironmentAttribute : StateAttribute {

		public EnvironmentAttribute(string key = null)
		{
			Key = key;
		}

		public string Key { get; }
	}

	class EnvironmentData : BindingObject {

        public EnvironmentData()
        {
            isStatic = true;
        }
        bool isStatic = false;
        public EnvironmentData(ContextualObject contextualObject)
        {
            View = contextualObject as View;
        }

        WeakReference _viewRef;
        public View View {
            get => _viewRef?.Target as View;
            private set => _viewRef = new WeakReference(value);
        }

		protected ICollection<string> GetAllKeys ()
		{
			//This is the global Environment
			if (View?.Parent == null)
				return dictionary.Keys;

			//TODO: we need a fancy way of collapsing this. This may be too slow
			var keys = new HashSet<string> ();
			var localKeys = dictionary?.Keys;
			if (localKeys != null)
				foreach (var k in localKeys)
					keys.Add (k);

			var parentKeys = View?.Parent?.Context?.GetAllKeys () ?? View.Environment.GetAllKeys ();
			if (parentKeys != null)
				foreach (var k in parentKeys)
					keys.Add (k);
			return keys;
		}

		public T GetValue<T> (string key)
		{
			try {
				var value = GetValue (key);
				return (T)value;
			} catch (Exception ex) {
				return default;
			}
		}

		public object GetValue (string key)
		{
			try {
				var value = GetValueInternal (key);
				if (value == null) {
					if (View?.Parent == null)
						return View.Environment.GetValueInternal (key);
					value = View?.Parent?.Context?.GetValue (key);
				}
				return value;
			} catch (Exception ex) {
				return null;
			}
		}
        protected override void CallPropertyRead(string propertyName)
        {
            if (View != null)
                View?.GetState().OnPropertyRead(this, propertyName);
            else if (isStatic)
            {
                View.ActiveViews.ForEach(x => x.GetState()?.OnPropertyRead(this, propertyName));
            }
            base.CallPropertyRead(propertyName);
        }

        public void SetValue(string key, object value)
        {
            SetProperty(value, key);
            if (View != null)
                View?.GetState().OnPropertyChanged(this, key,value);
            else if(isStatic)
            {
                View.ActiveViews.ForEach(x => x.GetState()?.OnPropertyChanged(this, key, value));
            }
        }
		internal void Clear()
		{
			dictionary.Clear ();
		}
	}
}
