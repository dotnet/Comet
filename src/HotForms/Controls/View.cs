using System;
namespace HotForms {

	public enum LayoutOptions {
		Start,
		Center,
		End,
		Fill,
		StartAndExpand,
		CenterAndExpand,
		EndAndExpand,
		FillAndExpand
	}

	public abstract class View {
		
		protected State State { get; set; }

		public View ()
		{
			State = StateBuilder.CurrentState;
			State.StartBuildingView ();
		}


		protected bool IsControlCreated => formsView != null;

		object formsView;
		public object FormsView {
			get => formsView ?? (formsView = CreateFormsView ());
			protected set => formsView = value;
		}
		protected abstract object CreateFormsView ();

		public static implicit operator Xamarin.Forms.View (View control )=> (Xamarin.Forms.View)control.FormsView;
	}

	//Right now this directly ties to Xamarin.Forms. I plan on changing this!
	public class View<T> : View where T : Xamarin.Forms.View, new() {


		protected override object CreateFormsView () => new T ();
		protected T FormsControl {
			get => (FormsView as T);
			set => FormsView = value;
		}


		public double HeightReqeust {
			get => FormsControl.HeightRequest;
			set => FormsControl.HeightRequest = value;
		}

		public double WidthRequest {
			get => FormsControl.WidthRequest;
			set => FormsControl.WidthRequest = value;
		}

		public double MinimumHeightRequest {
			get => FormsControl.MinimumHeightRequest;
			set => FormsControl.MinimumHeightRequest = value;
		}

		public double MinimumWidthRequest {
			get => FormsControl.MinimumWidthRequest;
			set => FormsControl.MinimumWidthRequest = value;
		}

		public LayoutOptions VerticalOptions {
			get => FormsControl.VerticalOptions.Convert();
			set => FormsControl.VerticalOptions = value.Convert();
		}
		public LayoutOptions HorizontalOptions {
			get => FormsControl.HorizontalOptions.Convert();
			set => FormsControl.HorizontalOptions = value.Convert();
		}

	}
}
