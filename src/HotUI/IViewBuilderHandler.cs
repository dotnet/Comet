using System;
namespace HotUI {
	public interface IViewBuilderHandler : IViewHandler {
		void SetViewBuilder (ViewBuilder builder);
	}
}
