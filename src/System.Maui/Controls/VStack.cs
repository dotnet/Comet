using System.Maui.Layout;

namespace System.Maui
{
	public class VStack : AbstractLayout
	{
		public VStack(
			HorizontalAlignment alignment = HorizontalAlignment.Center,
			float? spacing = null) : base(new VStackLayoutManager(alignment, spacing))
		{

		}
	}
}
