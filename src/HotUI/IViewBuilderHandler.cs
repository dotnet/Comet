using System;
namespace HotForms {
	public interface IViewBuilderHandler : IViewHandler {
		void SetViewBuilder (ViewBuilder builder);
	}
}
