using System;

namespace HotUI 
{
	public interface IViewHandler 
	{
		void SetView (View view);
		void UpdateValue (string property, object value);
		void Remove (View view);
		object NativeView { get; }
		bool HasContainer { get; set; }
	}
}
