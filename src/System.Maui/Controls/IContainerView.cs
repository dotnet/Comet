using System;
using System.Collections.Generic;

namespace System.Maui
{
	public interface IContainerView
	{
		IReadOnlyList<View> GetChildren();
	}
}
