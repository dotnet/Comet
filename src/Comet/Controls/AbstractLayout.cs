using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Platform;
using Xamarin.Platform.Layouts;

namespace Comet
{
	public abstract class AbstractLayout : ContainerView, ILayout
	{
		private ILayoutManager _layout;

		public ILayoutManager LayoutManager => _layout??= CreateLayoutManager();

		IReadOnlyList<IView> ILayout.Children => this.GetChildren();

		protected override void OnAdded(View view) => this.InvalidateMeasurement();

		protected override void OnClear() => this.InvalidateMeasurement();

		protected override void OnRemoved(View view) => this.InvalidateMeasurement();

		protected override void OnInsert(int index, View item) => this.InvalidateMeasurement();

		public override void LayoutSubviews(Xamarin.Forms.Rectangle frame)
		{
			base.LayoutSubviews(frame);
			var padding = this.GetPadding();
			var bounds = new Xamarin.Forms.Rectangle(
				padding.Left,
				padding.Right,
				frame.Width - padding.HorizontalThickness,
				frame.Height - padding.VerticalThickness);
			_layout.Arrange(bounds);
			//_layout?.Layout(this, bounds);
		}

		public override Xamarin.Forms.Size GetDesiredSize(Xamarin.Forms.Size availableSize)
		{
			if (IsMeasureValid)
			{
				return MeasuredSize;
			}
			MeasuredSize = LayoutManager.Measure(availableSize.Width, availableSize.Height);
			IsMeasureValid = true;
			return MeasuredSize;
		}

		public abstract ILayoutManager CreateLayoutManager();

	}
}
