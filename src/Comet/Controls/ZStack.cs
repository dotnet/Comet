using Comet.Layout;
using Xamarin.Platform.Layouts;

namespace Comet
{
	public class ZStack : AbstractLayout
	{
		protected override ILayoutManager CreateLayoutManager() => new ZStackLayoutManager(this);
	}
}
