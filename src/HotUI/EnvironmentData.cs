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
            //public const string Font = "Font";
            public const string Size = "Font.Size";
            public const string Family = "Font.Family";
            public const string Weight = "Font.Weight";
            public const string Italic = "Font.Italic";
        }

        public static class LineBreakMode
        {
            public const string Mode = "Mode";
        }

        public static class Colors
        {
            public const string Color = "Color";
            public const string BackgroundColor = "BackgroundColor";
        }

        public static class Animations
        {
            public const string Animation = "Animation";
        }

        public static class Layout
        {
            public const string Padding = "Layout.Padding";
            public const string Constraints = "Layout.Constraints";
            public const string HorizontalSizing = "Layout.HorizontalSizing";
            public const string VerticalSizing = "Layout.VerticalSizing";
            public const string FrameConstraints = "Layout.FrameConstraints";
        }

        public static class View
        {
	        public const string ClipShape = "ClipShape";
            public const string Shadow = "Shadow";
            public const string Overlay = "Overlay";
            public const string Title = "Title";
            public const string Border = "Border";
        }

        public static class Shape
        {
            public const string LineWidth = "Shape.LineWidth";
            public const string StrokeColor = "Shape.StrokeColor";
            public const string Fill = "Shape.Fill";
            public const string DrawingStyle = "Shape.Style";
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

		//protected ICollection<string> GetAllKeys ()
		//{
		//	//This is the global Environment
		//	if (View?.Parent == null)
		//		return dictionary.Keys;

		//	//TODO: we need a fancy way of collapsing this. This may be too slow
		//	var keys = new HashSet<string> ();
		//	var localKeys = dictionary?.Keys;
		//	if (localKeys != null)
		//		foreach (var k in localKeys)
		//			keys.Add (k);

		//	var parentKeys = View?.Parent?.Context?.GetAllKeys () ?? View.Environment.GetAllKeys ();
		//	if (parentKeys != null)
		//		foreach (var k in parentKeys)
		//			keys.Add (k);
		//	return keys;
		//}

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
