	using HotUI.Layout;

	namespace HotUI 
{
	public class VStack : AbstractLayout 
	{
        public VStack(
            HorizontalAlignment alignment = HorizontalAlignment.Center,
            float? spacing = null,
            Sizing sizing = Sizing.Fit) : base(new VStackLayoutManager(alignment, spacing, sizing))
        {

        }
	}
}
