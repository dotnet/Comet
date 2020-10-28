using System;
using Foundation;

namespace Comet
{
	public partial class CometPlatform
	{
		static NSObject _invoker = new NSObject();
		static partial void nativeInit()
		{
			ThreadHelper.SetFireOnMainThread(_invoker.BeginInvokeOnMainThread);
		}
	}
}
