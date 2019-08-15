using System;
using System.Collections;
using System.Collections.Generic;

namespace Comet {
	public class ContentView : View, IEnumerable {
		IEnumerator IEnumerable.GetEnumerator () => new [] { Content }.GetEnumerator ();
		public View Content { get; set; }
		public virtual void Add (View view)
		{
			if (view == null)
				return;
			view.Parent = this;
			view.Navigation = Parent?.Navigation;
			Content = view;
		}
		protected override void OnParentChange (View parent)
		{
			base.OnParentChange (parent);
			if (Content != null) {
				Content.Parent = this;
			}
		}
		
		internal override void ContextPropertyChanged (string property, object value)
		{
			base.ContextPropertyChanged (property, value);
			Content?.ContextPropertyChanged (property, value);
		}

        protected override void Dispose(bool disposing)
        {
            Content?.Dispose();
            Content = null;
            base.Dispose(disposing);
        }

        public override void LayoutSubviews(RectangleF bounds)
        {
            if (Content != null)
	             Content.Frame = bounds;
        }
	}
}
