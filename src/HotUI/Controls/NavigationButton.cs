using System;
namespace HotUI 
{
	public class NavigationButton : ContentView 
	{
		public NavigationButton(Func<View> destination)
		{
			Destination = destination;
		}
		public NavigationButton (Func<string> text,Func<View> destination)
		{
			Content = new Button (text, Navigate);

			Destination = destination;
		}
		public NavigationButton (string text, Func<View> destination)
		{
			Content = new Button (text,Navigate);
			Destination = destination;
		}

		public Func<View> Destination { get; }
        protected override View GetRenderView ()
		{
            if (IsDisposed)
                return null;
			if (Content == null)
				throw new Exception ("You are required to pass in Test or a Body");
			return base.GetRenderView ();
		}

		public void Navigate()
		{
			var view = Destination?.Invoke ();
			if(view is ModalView modal) {
				ModalView.Present (modal.Content);
			}
			else if(this.Navigation != null) {
				Navigation.Navigate(view);
			} else {
				ModalView.Present (view);
			}
		}
	}
}
