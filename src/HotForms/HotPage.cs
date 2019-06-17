
using System;
using System.Dynamic;
using ImpromptuInterface;
using Xamarin.Forms;

namespace HotForms {
	//For now this subclasses Content Page.  I pln on changing that and having an implicit cast.
	public abstract class HotPage : ContentPage {
		public new View Content {
			get => base.Content;// ?? (base.Content = Build());
			set => throw new Exception ("Don't do this!!!");
		}
		protected abstract View Build ();

		public void Reload()
		{
			SetupView ();
		}

		void SetupView()
		{
			var oldView = base.Content;
			var newView = Build ();
			//TODO compare and update old View;
			base.Content = newView;
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			SetupView ();
		}
	}

	public abstract class StateHotPage<T> : ContentPage where T: IState {

		T state;
		public T State {
			get {
				if (state == null) {
					var s = new State ();
					state = s.ActLike (typeof(T));
					CreateState (state);
					s.ResetChangeDictionary ();
					s.StateChanged = Reload;
				}
				return state;
			}
			set => state = value;
		}

		public void Reload ()
		{
			SetupView ();
		}

		void SetupView ()
		{
			var oldView = base.Content;
			var newView = Build (State);
			//TODO compare and update old View;
			base.Content = newView;
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			SetupView ();
		}


		protected abstract void CreateState (T state);

		protected abstract View Build (T state);
	}


	public abstract class StateHotPage : ContentPage {

		State state;
		public State State {
			get {
				if (state == null) {
					state = new State ();
					CreateState (state);
					state.StateChanged = Reload;
					state.ResetChangeDictionary ();
				}
				return state;
			}
			set => state = value;
		}

		public void Reload ()
		{
			SetupView ();
		}

		void SetupView ()
		{
			var oldView = base.Content;
			var newView = Build (State);
			//TODO compare and update old View;
			base.Content = newView;
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			SetupView ();
		}


		protected abstract void CreateState (dynamic state);

		protected abstract View Build (dynamic state);
	}
}
