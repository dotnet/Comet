using Microsoft.Maui.Graphics;

namespace Comet.Tests.Handlers
{
	public class SecureFieldHandler : GenericViewHandler
	{
		public SecureFieldHandler()
		{
			OnGetIntrinsicSize = HandleOnGetIntrinsicSize;
		}

		public SecureField VirtualView => (SecureField)CurrentView;

		private Size HandleOnGetIntrinsicSize(double widthConstraint, double heightConstraint)
		{
			var length = VirtualView.Text?.CurrentValue?.Length ?? 0;
			return new SizeF(10 * length, 12);
		}
	}
}
