using System;
using UIKit;
namespace HotUI.iOS {
	public interface IUIView : IViewHandler {
		UIView View { get; }
	}
}
