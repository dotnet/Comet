using System;
using Android.Content;
using AView = Android.Views.View;

namespace Comet.Android
{
	public class ViewRepresentable<T> : ViewRepresentable where T : AView
	{
		public delegate void UpdateAView(T view, object state);

		public new Func<Context, AView> MakeView
		{
			get => (context) => base.MakeView?.Invoke() as T;
			set => base.MakeView = () => value?.Invoke(AndroidContext.CurrentContext);
		}

		public new UpdateAView UpdateView
		{
			get => (view, state) => base.UpdateView?.Invoke(view, state);
			set => base.UpdateView = (view, state) => value?.Invoke(view as T, state);
		}
	}
}
