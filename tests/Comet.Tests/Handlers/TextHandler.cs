using System.Drawing;

namespace Comet.Tests.Handlers
{
	public class TextHandler : GenericViewHandler
	{
		public TextHandler()
		{
			OnGetIntrinsicSize = HandleOnGetIntrinsicSize;
		}

		public Text VirtualView => (Text)CurrentView;

		private SizeF HandleOnGetIntrinsicSize(SizeF arg)
		{
			var length = VirtualView.Value?.CurrentValue?.Length ?? 0;
			return new SizeF(10 * length, 12);
		}
	}
}
