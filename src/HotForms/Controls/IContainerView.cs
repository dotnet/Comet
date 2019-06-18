using System;
using System.Collections.Generic;

namespace HotForms {
	public interface IContainerView
	{ 
		IReadOnlyList<View> GetChildren();
	}
}
