using System;
using System.Collections.Generic;

namespace Comet {
	public interface IContainerView
	{ 
		IReadOnlyList<View> GetChildren();
	}
}
