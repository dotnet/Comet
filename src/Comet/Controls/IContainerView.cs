using System;
using System.Collections.Generic;

namespace Comet
{
	public interface IContainerView : IView, IVisualTreeElement
	{

		IReadOnlyList<IVisualTreeElement> IVisualTreeElement.GetVisualChildren() => Array.Empty<IVisualTreeElement>();
		IVisualTreeElement IVisualTreeElement.GetVisualParent() => this.Parent as IVisualTreeElement; 
		IReadOnlyList<View> GetChildren();
	}
}
