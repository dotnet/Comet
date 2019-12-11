using System;
using UIKit;

namespace Comet.iOS
{
	public class UIViewRepresentable<T> : ViewRepresentable where T : UIView
	{
		public delegate void UpdateUIView(T view, object state);

		public new Func<UIView> MakeView
		{
			get => () => base.MakeView?.Invoke() as T;
			set => base.MakeView = () => value?.Invoke();
		}

		public new UpdateUIView UpdateView
		{
			get => (view, state) => base.UpdateView?.Invoke(view, state);
			set => base.UpdateView = (view, state) => value?.Invoke(view as T, state);
		}
	}
}
