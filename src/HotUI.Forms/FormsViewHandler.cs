using System;
namespace HotUI.Forms {
	public interface FormsViewHandler  : IViewHandler{
		Xamarin.Forms.View View { get; }
	}
}
