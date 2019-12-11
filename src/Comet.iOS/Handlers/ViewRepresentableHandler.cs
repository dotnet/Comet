using UIKit;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.iOS.Handlers
{
	public class ViewRepresentableHandler : AbstractControlHandler<ViewRepresentable, UIView>
	{
		public static readonly PropertyMapper<ViewRepresentable> Mapper = new PropertyMapper<ViewRepresentable>(ViewHandler.Mapper)
		{
			[nameof(ViewRepresentable.Data)] = MapDataProperty
		};

		public ViewRepresentableHandler() : base(Mapper)
		{

		}

		protected override UIView CreateView()
		{
			return VirtualView?.MakeView() as UIView;
		}

		protected override void DisposeView(UIView nativeView)
		{

		}

		public static void MapDataProperty(IViewHandler viewHandler, ViewRepresentable virtualView)
		{
			var data = virtualView.Data;
			virtualView.UpdateView?.Invoke(viewHandler.NativeView, data);
		}
	}
}
