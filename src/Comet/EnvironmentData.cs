using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Comet
{

	public static class EnvironmentKeys
	{
		public const string DocumentsFolder = "DocumentsFolder";
		public const string UserFolder = "UserFolder";
		public const string OS = "OS";

		public static class Fonts
		{
			public const string Font = "Font";
			public const string Size = "Font.Size";
			public const string Weight = "Font.Weight";
			public const string Family = "Font.Family";
			public const string Slant = "Font.Slant";
			//public const string Attributes = "Font.Attributes";
		}

		public static class LineBreakMode
		{
			public const string Mode = "Mode";
		}

		public static class Colors
		{
			public const string Color = "Color";
			public const string Background = nameof(Background);
		}

		public static class Animations
		{
			public const string Animation = "Animation";
		}

		public static class Layout
		{
			public const string Margin = "Layout.Margin";
			public const string Padding = "Layout.Padding";
			public const string Constraints = "Layout.Constraints";
			public const string HorizontalLayoutAlignment = "Layout.HorizontalSizing";
			public const string VerticalLayoutAlignment = "Layout.VerticalSizing";
			public const string FrameConstraints = "Layout.FrameConstraints";
			public const string IgnoreSafeArea = "Layout.IgnoreSafeArea";
		}

		public static class View
		{
			public const string ClipShape = "ClipShape";
			public const string Shadow = "Shadow";
			public const string Title = "Title";
			public const string Border = "Border";
			public const string StyleId = "StyleId";
			public const string AutomationId = nameof(AutomationId);
			public const string Opacity = nameof(Microsoft.Maui.IView.Opacity);
		}

		public static class Button
		{
			public const string Padding = "Padding";
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

		public static class Text
		{
			public const string HorizontalAlignment = nameof(Microsoft.Maui.ITextAlignment.HorizontalTextAlignment);
			public const string VerticalAlignment = nameof(Microsoft.Maui.ITextAlignment.VerticalTextAlignment);
			public static class Style
			{
				public const string H1 = nameof(H1);
				public const string H2 = nameof(H2);
				public const string H3 = nameof(H3);
				public const string H4 = nameof(H4);
				public const string H5 = nameof(H5);
				public const string H6 = nameof(H6);
				public const string Subtitle1 = nameof(Subtitle1);
				public const string Subtitle2 = nameof(Subtitle2);
				public const string Body1 = nameof(Body1);
				public const string Body2 = nameof(Body2);
				public const string Caption = nameof(Caption);
				public const string Overline = nameof(Overline);
			}
		}
		public static class Navigation
		{
			public const string BackgroundColor = "NavigationBackgroundColor";
			public const string TextColor = "NavigationTextColor";
		}
		public static class Slider
		{
			public const string TrackColor = "MinimumTrackColor";
			public const string ProgressColor = "MaximumTrackColor";
			public const string ThumbColor = "ThumbColor";
		}
		public static class ProgressBar
		{
			public const string ProgressColor = "ProgressColor";
		}
	}

	[AttributeUsage(AttributeTargets.Field)]
	public class EnvironmentAttribute : StateAttribute
	{

		public EnvironmentAttribute(string key = null)
		{
			Key = key;
		}

		public string Key { get; }
	}

	class EnvironmentData : BindingObject
	{

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
		public View View
		{
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

		public T GetValue<T>(string key)
		{
			try
			{
				var value = GetValue(key);
				return (T)value;
			}
			catch
			{
				return default;
			}
		}

		public object GetValue(string key)
		{
			try
			{
				var value = GetValueInternal(key).value;
				return value;
			}
			catch
			{
				return null;
			}
		}
		protected override void CallPropertyRead(string propertyName)
		{
			if (View != null)
				StateManager.OnPropertyRead(View.Environment, propertyName);
			else if (isStatic)
			{

				StateManager.OnPropertyRead(View.Environment, propertyName);
				//TODO: Verify this is right. We may need a way to tell allthe views a property changed
				// View.ActiveViews.ForEach(x => x.GetState()?.OnPropertyRead(this, propertyName));
			}
			base.CallPropertyRead(propertyName);
		}

		public bool SetValue(string key, object value, bool cascades)
		{
			//if Nothing changed, don't send on notifications
			if (!SetProperty(value, key))
				return false;
			if (!cascades)
				return true;
			if (View != null)
			{
				if (!StateManager.IsBuilding)
					StateManager.OnPropertyChanged(View.Environment, key, value);
			}
			else if (isStatic)
			{
				StateManager.OnPropertyChanged(View.Environment, key, value);
				//TODO: Verify this is right. We may need a way to tell allthe views a property changed
				//View.ActiveViews.ForEach(x => x.GetState()?.OnPropertyChanged(this, key, value));
			}
			return true;
		}
		internal void Clear()
		{
			dictionary.Clear();
		}
	}
}
