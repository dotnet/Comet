using System.Collections.Generic;

using Comet.Graphics;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public abstract class Shape : ContextualObject, IShape, IBorderStroke
	{
		internal View view;

		Paint IStroke.Stroke => this.GetEnvironment<Paint>(view, EnvironmentKeys.Shape.StrokeColor);

		double IStroke.StrokeThickness => this.GetEnvironment<float>(view, EnvironmentKeys.Shape.LineWidth);

		LineCap IStroke.StrokeLineCap => this.GetEnvironment<LineCap?>(view, nameof(IStroke.StrokeLineCap)) ?? LineCap.Butt;

		LineJoin IStroke.StrokeLineJoin => this.GetEnvironment<LineJoin?>(view, nameof(IStroke.StrokeLineJoin)) ?? LineJoin.Miter;

		float[] IStroke.StrokeDashPattern => this.GetEnvironment<float[]>(view, nameof(IStroke.StrokeDashPattern));

		float IStroke.StrokeDashOffset => this.GetEnvironment<float>(view, nameof(IStroke.StrokeDashOffset));

		float IStroke.StrokeMiterLimit => this.GetEnvironment<float?>(view, nameof(IStroke.StrokeMiterLimit)) ?? 10;

		IShape IBorderStroke.Shape => this;

		protected Shape()
		{

		}

		internal override void ContextPropertyChanged(string property, object value, bool cascades)
		{

		}

		public abstract PathF PathForBounds(Rect rect);
	}
}
