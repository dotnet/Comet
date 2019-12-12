using System;
using CoreAnimation;
using Foundation;

namespace Comet.iOS
{
	public class iOSTicker : Ticker
	{
		public iOSTicker()
		{
		}
		CADisplayLink link;

		public override bool IsRunning => link != null;
		public override void Start()
		{
			if (link != null)
				return;
			link = CADisplayLink.Create(() => Fire?.Invoke());
			link.AddToRunLoop(NSRunLoop.Current, NSRunLoop.NSRunLoopCommonModes);

		}
		public override void Stop()
		{
			if (link == null)
				return;
			link?.RemoveFromRunLoop(NSRunLoop.Current, NSRunLoop.NSRunLoopCommonModes);
			link?.Dispose();
			link = null;
		}
	}
}
