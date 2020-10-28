using System;
using Xamarin.Platform;
using Xamarin.Platform.Handlers;

namespace Comet.Handlers
{
	public partial class CometViewHandler : AbstractViewHandler<View,CometViewContainer>
	{
		public static PropertyMapper<View> CometViewMapper = new PropertyMapper<View>(ViewHandler.ViewMapper)
		{
			[nameof(Comet.View.BuiltView)] = MapBuiltView
		};

		public CometViewHandler() : this(CometViewMapper)
		{

		}

		public CometViewHandler(PropertyMapper  mapper) : base(mapper)
		{

		}

#if MONOANDROID
		protected override CometViewContainer CreateView() => new CometViewContainer(Context);
#else
		protected override CometViewContainer CreateView() => new CometViewContainer();
#endif

		public static void MapBuiltView(IViewHandler handler, View view)
		{
			(handler.NativeView as CometViewContainer)?.SetView(view);
		}
	}
}
