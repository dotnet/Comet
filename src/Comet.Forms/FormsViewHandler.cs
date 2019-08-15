using System;
namespace Comet.Forms 
{
	public interface FormsViewHandler  : IViewHandler
	{
		event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		Xamarin.Forms.View View { get; }
	}
}
