using System;
namespace HotUI.Forms 
{
	public interface FormsViewHandler  : IViewHandler
	{
		event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		Xamarin.Forms.View View { get; }
	}
}
