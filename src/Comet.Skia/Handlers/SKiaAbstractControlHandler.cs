using System;
using System.Drawing;
using SkiaSharp;

namespace Comet.Skia
{
	public abstract class SkiaAbstractControlHandler<TVirtualView> : SkiaControl, IViewHandler
		where TVirtualView : View
	{
		readonly PropertyMapper<TVirtualView> mapper;
		protected readonly DrawMapper<TVirtualView> drawMapper;

		protected SkiaAbstractControlHandler()
		{
			drawMapper = new DrawMapper<TVirtualView>(SkiaControl.DrawMapper);
			mapper = new PropertyMapper<TVirtualView>(SkiaControl.Mapper);
		}

		protected SkiaAbstractControlHandler(DrawMapper<TVirtualView> drawMapper, PropertyMapper<TVirtualView> mapper)
		{
			this.drawMapper = drawMapper ?? new DrawMapper<TVirtualView>(SkiaControl.DrawMapper);
			this.mapper = mapper ?? new PropertyMapper<TVirtualView>(SkiaControl.Mapper);
		}


		protected virtual string[] LayerDrawingOrder() => DefaultLayerDrawingOrder;

		protected SizeF GetIntrinsicSize(float defaultWidth = 100, float defaultHeight = 44)
		{
			var minHeight = TypedVirtualView.GetEnvironment<float?>(SkiaEnvironmentKeys.IntrinsicSize.Height) ?? defaultHeight;
			var minWidth = TypedVirtualView.GetEnvironment<float?>(SkiaEnvironmentKeys.IntrinsicSize.Width) ?? defaultWidth;
			return new SizeF(minWidth, minHeight);
		}

		public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
		{
			canvas.Clear(Color.Transparent.ToSKColor());
			if (TypedVirtualView == null || drawMapper == null)
				return;
			canvas.Save();
			var layers = LayerDrawingOrder();
			var padding = this.GetPadding();
			var rect = dirtyRect.ApplyPadding(padding);
			foreach (var layer in layers)
			{
				drawMapper.DrawLayer(canvas, rect, this, TypedVirtualView, layer);
			}

			var clipShape = VirtualView?.GetClipShape() ?? VirtualView?.GetBorder();
			if (clipShape != null)
				canvas.ClipPath(clipShape.PathForBounds(rect).ToSKPath());

			drawMapper.DrawLayer(canvas, rect, this, TypedVirtualView, SkiaEnvironmentKeys.Border);

			canvas.Restore();
		}

		public TVirtualView TypedVirtualView { get; private set; }

		public object NativeView => throw new NotImplementedException();

		public bool HasContainer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override void SetView(View view)
		{
			TypedVirtualView = view as TVirtualView;
			mapper?.UpdateProperties(this, TypedVirtualView);
			base.SetView(view);
		}

		public void UpdateValue(string property, object value)
		{
			mapper?.UpdateProperty(this, TypedVirtualView, property);
		}
		public override void ViewPropertyChanged(string property, object value)
		{
			Invalidate();
		}
		public virtual void Remove(View view)
		{
			throw new NotImplementedException();
		}

		public void SetFrame(RectangleF frame)
		{
			throw new NotImplementedException();
		}
	}
}
