using Comet.Layout;

namespace Comet
{
	public class ZStack : AbstractLayout
	{
		public ZStack() : base(new ZStackLayoutManager())
		{
		}
	}
}
