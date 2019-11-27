using Android.Content;
using AView = global::Android.Views.View;
using LP = Android.Views.ViewGroup.LayoutParams;

namespace Comet.Android.Handlers
{
	public class ContentViewHandler : AbstractHandler<ContentView, CUIContentView>
	{
		private AView _view;

		protected override CUIContentView CreateView(Context context)
		{
			var contentView = new CUIContentView();
			_view = VirtualView?.Content?.ToView();
			contentView.AddView(_view, new LP(LP.MatchParent, LP.MatchParent));
			return contentView;
		}

		public override void Remove(View view)
		{
			TypedNativeView.RemoveView(_view);
			_view = null;

			base.Remove(view);
		}
	}
}
