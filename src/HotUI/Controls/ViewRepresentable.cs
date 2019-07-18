using System;
namespace HotUI
{
    public delegate void UpdateView(object view, object state);

	public class ViewRepresentable : Control
    {
        public ViewRepresentable()
        {

        }

        public ViewRepresentable(object data) : base (true)
		{
            Data = data;
		}

		public ViewRepresentable(Func<object> dataBuilder)
		{
			DataBinding = dataBuilder;
		}

        public Func<object> MakeView { get; set; }

        public UpdateView UpdateView { get; set; }

        private object context;
		public object Data
        {
			get => context;
			private set => (this).SetValue(base.State, ref context, value);
		}

		public Func<object> DataBinding { get; private set; }

		protected override void WillUpdateView ()
		{
			base.WillUpdateView ();
			if (DataBinding != null)
            {
				base.State.StartProperty ();
				var text = DataBinding.Invoke ();
				var props = base.State.EndProperty ();
				var propCount = props.Length;
				if (propCount > 0)
                {
					base.State.BindingState.AddViewProperty (props, this, nameof(DataBinding));
				}
				Data = text;
			}
		}
        protected override void ViewPropertyChanged(string property, object value)
        {
            if(property == nameof(DataBinding))
            {
                Data = DataBinding.Invoke();
                return;
            }
            base.ViewPropertyChanged(property, value);
        }

    }
}
