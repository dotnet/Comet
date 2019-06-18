
using System;
using System.Dynamic;
using ImpromptuInterface;
using Xamarin.Forms;

namespace HotForms {
	//For now this subclasses Content Page.  I pln on changing that and having an implicit cast.
	public abstract class HotPage : ContentPage {
		//public new View Content {
		//	get => base.Content;// ?? (base.Content = Build());
		//	set => throw new Exception ("Don't do this!!!");
		//}
		protected abstract View Build ();

		public void Reload()
		{
			SetupView ();
		}
		View currentView;
		void SetupView()
		{
			var oldView = currentView;
			var newView = Build ();
			if(oldView != null) {
				newView.DiffUpdate (oldView);
			}
			base.Content = currentView = newView;
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

		View currentView;
		void SetupView ()
		{
			var oldView = currentView;
			//It is always a state!!!
			var state = (State)(object)State;
			state.BindingState.Clear ();
			using (new StateBuilder (state)) {
				var newView = Build (State);
				if (oldView != null)
					newView.DiffUpdate (oldView);
				base.Content = currentView = newView;
			}
			Console.WriteLine ("Page is recreated");
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
					state.ResetChangeDictionary ();
					state.StateChanged = Reload;
				}
				return state;
			}
			set => state = value;
		}

		public void Reload ()
		{
			SetupView ();
		}


		View currentView;
		void SetupView ()
		{
			Console.WriteLine ("Setup View is Called");
			var oldView = currentView;
			//It is always a state!!!
			State.BindingState.Clear ();
			using (new StateBuilder (State)) {
				var newView = Build (State);
				if (oldView != null)
					newView.DiffUpdate (oldView);
				base.Content = currentView = newView;
			}
			Console.WriteLine ("Page is recreated");
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
