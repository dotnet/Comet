using System;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Comet.Handlers
{
	public partial class ShapeViewHandler
	{
		public static readonly PropertyMapper<ShapeView> Mapper = new PropertyMapper<ShapeView>(ViewHandler.ViewMapper)
		{
			 [nameof(ShapeView.Shape)] = MapShapeProperty
		};



		public ShapeViewHandler() : base(Mapper)
		{
		}

		public ShapeViewHandler(PropertyMapper<ShapeView> mapper) : base(mapper)
		{
		}
	}
}
