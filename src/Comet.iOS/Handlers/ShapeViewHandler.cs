

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.iOS.Handlers
{
	public class ShapeViewHandler : AbstractControlHandler<ShapeView, CUIShapeView>
	{
		public static readonly PropertyMapper<ShapeView> Mapper = new PropertyMapper<ShapeView>(ViewHandler.Mapper)
		{
			[nameof(Comet.ShapeView.Shape)] = MapShapeProperty,
		};


		public ShapeViewHandler() : base(Mapper)
		{

		}

		protected override CUIShapeView CreateView() => new CUIShapeView();

		protected override void DisposeView(CUIShapeView nativeView)
		{

		}

		public static void MapShapeProperty(IViewHandler viewHandler, ShapeView virtualView)
		{
			var nativeView = (CUIShapeView)viewHandler.NativeView;
			nativeView.View = virtualView;
			nativeView.Shape = virtualView.Shape?.CurrentValue;
		}
	}
}
