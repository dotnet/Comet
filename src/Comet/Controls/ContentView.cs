using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class ContentView : View, IEnumerable, IContainerView, IContentView
	{
		IEnumerator IEnumerable.GetEnumerator() => new[] { Content }.GetEnumerator();
		public View Content { get; set; }

		object IContentView.Content => Content;

		IView IContentView.PresentedContent => Content;

		Thickness IPadding.Padding => this.GetPadding();

		public virtual void Add(View view)
		{
			if (view == null)
				return;
			view.Parent = this;
			view.Navigation = Parent?.Navigation;
			Content = view;
			TypeHashCode = view.GetContentTypeHashCode();
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

		public override void LayoutSubviews(Rect frame)
		{
			this.Frame = frame;
			Content?.LayoutSubviews(frame);
		}
		public override Size GetDesiredSize(Size availableSize)
		{
			if (Content != null)
			{
				var margin = Content.GetMargin();
				availableSize.Width -= margin.HorizontalThickness;
				availableSize.Height -= margin.VerticalThickness;
				MeasuredSize = Content.Measure(availableSize, true);
				return MeasuredSize;
			}

			return base.GetDesiredSize(availableSize);
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

		public IReadOnlyList<View> GetChildren() => new[] { Content };
		Size ICrossPlatformLayout.CrossPlatformMeasure(double widthConstraint, double heightConstraint) => this.Measure(widthConstraint, heightConstraint);
		Size ICrossPlatformLayout.CrossPlatformArrange(Rect bounds)
		{
			if (!this.MeasurementValid)
				Measure(bounds.Width, bounds.Height);
			this.LayoutSubviews(bounds);
			return this.MeasuredSize;
		}
		Size IContentView.CrossPlatformMeasure(double widthConstraint, double heightConstraint) => this.Measure(widthConstraint, heightConstraint);
		Size IContentView.CrossPlatformArrange(Rect bounds) => ((ICrossPlatformLayout)this).CrossPlatformArrange(bounds);
	}
}
