using System;
using System.Drawing;
using SkiaSharp;

namespace Comet.Skia
{
	public abstract class SKiaAbstractControlHandler<TVirtualView> : SkiaControl, IViewHandler
        where TVirtualView : View
    {

        PropertyMapper<TVirtualView> mapper;
		protected DrawMapper<TVirtualView> drawMapper;
		

		protected SKiaAbstractControlHandler()
		{
			drawMapper = new DrawMapper<TVirtualView>(SkiaControl.DrawMapper);
			mapper = new PropertyMapper<TVirtualView>(SkiaControl.Mapper);
		}

		protected SKiaAbstractControlHandler(DrawMapper<TVirtualView> drawMapper, PropertyMapper<TVirtualView> mapper)
		{
			this.drawMapper = drawMapper ?? new DrawMapper<TVirtualView>(SkiaControl.DrawMapper);
			this.mapper = mapper ?? new PropertyMapper<TVirtualView>(SkiaControl.Mapper);
		}

		public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
		{
            canvas.Clear(Color.Transparent.ToSKColor());
            if (TypedVirtualView == null || drawMapper == null)
                return;
            canvas.Save();

            var border = VirtualView?.GetBorder();
            var didDrawBorder = border != null && drawMapper.DrawLayer(canvas, dirtyRect, this, TypedVirtualView, SkiaEnvironmentKeys.Border);

            if (!didDrawBorder)
                drawMapper.DrawLayer(canvas, dirtyRect, this, TypedVirtualView, SkiaEnvironmentKeys.Background);


            drawMapper.DrawLayer(canvas, dirtyRect, this, TypedVirtualView, SkiaEnvironmentKeys.Text);

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

        public void Remove(View view)
        {
            throw new NotImplementedException();
        }

        public void SetFrame(RectangleF frame)
        {
            throw new NotImplementedException();
        }
    }
}
