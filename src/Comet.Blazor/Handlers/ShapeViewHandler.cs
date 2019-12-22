using Comet.Blazor.Components;

namespace Comet.Blazor.Handlers
{
    internal class ShapeViewHandler : BlazorHandler<ShapeView, BShape>
    {
        public static readonly PropertyMapper<ShapeView> Mapper = new PropertyMapper<ShapeView>
        {
            { nameof(ShapeView.Shape), MapShapeProperty },
        };

        public ShapeViewHandler()
            : base(Mapper)
        {
        }

        public static void MapShapeProperty(IViewHandler viewHandler, ShapeView virtualView)
        {
            var nativeView = (BShape)viewHandler.NativeView;

            nativeView.Shape = virtualView.Shape;
        }
    }
}
