using System.Drawing;

namespace Comet.Tests.Handlers
{
	public class SecureFieldHandler : GenericViewHandler
	{
		public SecureFieldHandler()
		{
			OnMeasure = HandleOnMeasure;
		}

		public SecureField VirtualView => (SecureField)CurrentView;

		private SizeF HandleOnMeasure(SizeF arg)
		{
			var length = VirtualView.Text?.CurrentValue?.Length ?? 0;
			return new SizeF(10 * length, 12);
		}
	}
}
