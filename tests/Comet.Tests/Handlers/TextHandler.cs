using Microsoft.Maui.Graphics;

namespace Comet.Tests.Handlers
{
	public class TextHandler : GenericViewHandler
	{
		public TextHandler()
		{
			OnGetIntrinsicSize = HandleOnGetIntrinsicSize;
		}

		public Text VirtualView => (Text)CurrentView;

		private Size HandleOnGetIntrinsicSize(double widthConstraint, double heightConstraint)
		{
			var length = VirtualView.Value?.CurrentValue?.Length ?? 0;
			return new SizeF(10 * length, 12);
		}
	}
}
