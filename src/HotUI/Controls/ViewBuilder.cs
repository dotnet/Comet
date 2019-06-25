//using System;
//using System.Collections.Generic;
//using System.Diagnostics;

//namespace HotUI {
//	public abstract class ViewBuilder : State {
//		public ViewBuilder ()
//		{
//			StateChanged = Reload;
//		}
//		protected abstract View Build ();
//		public void Reload ()
//		{
//			ReBuildView ();
//		}
//		View view;
//		public View View {
//			get => view;
//			protected set {
//				if (view == value)
//					return;
//				view = value;
//				ViewHandler?.SetView (value);
				
//			}
//		}

//		public void ReBuildView ()
//		{
//			var oldView = View;
//			BindingState.Clear ();
//			using (new StateBuilder (this)) {
//				var start = DateTime.Now;
//				var newView = Build ();
//				if (oldView != null) {
//					newView.Diff (oldView);
//				}
//				var end = DateTime.Now;
//				Debug.WriteLine ($"View Diffing took: {(end - start).TotalMilliseconds} ms");
//				View = newView;
//			}
//		}


//		IViewBuilderHandler formsView;
//		public IViewBuilderHandler ViewHandler {
//			get => formsView;
//			set {
//				if (formsView == value)
//					return;
//				formsView?.Remove (View);
//				formsView = value;
//				formsView?.SetViewBuilder (this);
//			}
//		}

//		protected void ViewPropertyChanged (string property, object value)
//		{
//			ViewHandler?.UpdateValue (property, value);
//		}


//	}
//}
