using System.Drawing;

namespace Comet.Tests.Handlers
{
	public class TextFieldHandler : GenericViewHandler
	{
		public TextFieldHandler()
		{
			OnMeasure = HandleOnMeasure;
		}

		public TextField VirtualView => (TextField)CurrentView;

		private SizeF HandleOnMeasure(SizeF arg)
		{
			var length = VirtualView.Text?.CurrentValue?.Length ?? 0;
			return new SizeF(10 * length, 12);
		}
	}
}
