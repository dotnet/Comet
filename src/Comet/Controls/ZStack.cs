using Comet.Layout;
using Microsoft.Maui.Layouts;

namespace Comet
{
	public class ZStack : AbstractLayout
	{
		protected override ILayoutManager CreateLayoutManager() => new ZStackLayoutManager(this);
	}
}
