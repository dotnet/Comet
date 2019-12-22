using Windows.UI.Xaml.Controls;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.UWP.Handlers
{
	public class SpacerHandler : AbstractHandler<Spacer, Canvas>
	{
		protected override Canvas CreateView()
		{
			return new Canvas();
		}
	}
}
