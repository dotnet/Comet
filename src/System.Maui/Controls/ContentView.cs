using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace System.Maui
{
	public class ContentView : View, IEnumerable
	{
		IEnumerator IEnumerable.GetEnumerator() => new[] { Content }.GetEnumerator();
		public View Content { get; set; }
		public virtual void Add(View view)
		{
			if (view == null)
				return;
			view.Parent = this;
			view.Navigation = Parent?.Navigation;
			Content = view;
		}
		protected override void OnParentChange(View parent)
		{
			base.OnParentChange(parent);
			if (Content != null)
			{
				Content.Parent = this;
			}
		}

		internal override void ContextPropertyChanged(string property, object value, bool cascades)
		{
			base.ContextPropertyChanged(property, value, cascades);
			Content?.ContextPropertyChanged(property, value, cascades);
		}

		protected override void Dispose(bool disposing)
		{
			Content?.Dispose();
			Content = null;
			base.Dispose(disposing);
		}

		public override void LayoutSubviews(RectangleF frame)
		{
			if (Content != null)
			{
				var margin = Content.GetMargin();
				var bounds = new RectangleF(
					frame.Left + margin.Left,
					frame.Top + margin.Top,
					frame.Width - margin.HorizontalThickness,
					frame.Height - margin.VerticalThickness);
				Content.Frame = bounds;
			}
		}

		public override SizeF GetIntrinsicSize(SizeF availableSize)
		{
			if (Content != null)
			{
				var margin = Content.GetMargin();
				availableSize.Width -= margin.HorizontalThickness;
				availableSize.Height -= margin.VerticalThickness;
				var measuredSize = Content.Measure(availableSize, true);
				return measuredSize;
			}

			return base.GetIntrinsicSize(availableSize);
		}
		internal override void Reload(bool isHotReload)
		{
			Content?.Reload(isHotReload);
			base.Reload(isHotReload);
		}

		public override void ViewDidAppear()
		{
			Content?.ViewDidAppear();
			base.ViewDidAppear();
		}

		public override void ViewDidDisappear()
		{
			Content?.ViewDidAppear();
			base.ViewDidDisappear();
		}

        public override void PauseAnimations()
        {
			Content?.PauseAnimations();
            base.PauseAnimations();
        }
        public override void ResumeAnimations()
        {
			Content?.ResumeAnimations();
            base.ResumeAnimations();
        }
    }
}
