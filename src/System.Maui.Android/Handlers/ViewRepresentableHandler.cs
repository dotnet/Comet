using Android.Content;
using AView = Android.Views.View;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace System.Maui.Android.Handlers
{
	public class ViewRepresentableHandler : AbstractControlHandler<ViewRepresentable, AView>
	{
		public static readonly PropertyMapper<ViewRepresentable> Mapper = new PropertyMapper<ViewRepresentable>(ViewHandler.Mapper)
		{
			[nameof(ViewRepresentable.Data)] = MapDataProperty
		};

		public ViewRepresentableHandler() : base(Mapper)
		{
		}

		protected override AView CreateView(Context context)
		{
			return VirtualView?.MakeView() as AView;
		}

		protected override void DisposeView(AView nativeView)
		{
		}

		public static void MapDataProperty(IViewHandler viewHandler, ViewRepresentable virtualView)
		{
			var data = virtualView.Data;
			virtualView.UpdateView?.Invoke(viewHandler.NativeView, data);
		}
	}
}
