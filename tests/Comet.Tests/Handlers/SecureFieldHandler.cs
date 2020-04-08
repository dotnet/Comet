using System.Drawing;

namespace System.Maui.Tests.Handlers
{
	public class SecureFieldHandler : GenericViewHandler
	{
		public SecureFieldHandler()
		{
			OnGetIntrinsicSize = HandleOnGetIntrinsicSize;
		}

		public SecureField VirtualView => (SecureField)CurrentView;

		private SizeF HandleOnGetIntrinsicSize(SizeF arg)
		{
			var length = VirtualView.Text?.CurrentValue?.Length ?? 0;
			return new SizeF(10 * length, 12);
		}
	}
}
