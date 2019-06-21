using System;
using System.Collections.Generic;

namespace HotUI {
	public interface IContainerView
	{ 
		IReadOnlyList<View> GetChildren();
	}
}
