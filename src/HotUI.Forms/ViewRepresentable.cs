using System;
using FView = Xamarin.Forms.View;

namespace HotUI.Forms
{
    public class ViewRepresentable<T> : ViewRepresentable where T: FView
    {
        public delegate void UpdateAView(T view, object state);

        public new Func<FView> MakeView
        {
            get => () => base.MakeView?.Invoke() as T;
            set => base.MakeView = () => value?.Invoke();            
        }

        public new UpdateAView UpdateView
        {
            get => (view, state) => base.UpdateView?.Invoke(view, state);
            set => base.UpdateView = (view, state) => value?.Invoke(view as T, state);
        }
    }
}
