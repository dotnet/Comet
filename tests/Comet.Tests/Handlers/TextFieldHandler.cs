using Microsoft.Maui.Graphics;

namespace Comet.Tests.Handlers
{
	public class TextFieldHandler : GenericViewHandler
	{
		public TextFieldHandler()
		{
			OnGetIntrinsicSize = HandleOnGetIntrinsicSize;
		}

		public TextField VirtualView => (TextField)CurrentView;

		private Size HandleOnGetIntrinsicSize(double widthConstraint, double heightConstraint)
		{
			var length = VirtualView.Text?.CurrentValue?.Length ?? 0;
			return new SizeF(10 * length, 12);
		}
	}
}
