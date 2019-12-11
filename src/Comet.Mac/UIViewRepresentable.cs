using System;
using AppKit;

namespace Comet.Mac
{
	public class NSViewRepresentable<T> : ViewRepresentable where T : NSView
	{
		public delegate void UpdateNSView(T view, object state);

		public new Func<NSView> MakeView
		{
			get => () => base.MakeView?.Invoke() as T;
			set => base.MakeView = () => value?.Invoke();
		}

		public new UpdateNSView UpdateView
		{
			get => (view, state) => base.UpdateView?.Invoke(view, state);
			set => base.UpdateView = (view, state) => value?.Invoke(view as T, state);
		}
	}
}
