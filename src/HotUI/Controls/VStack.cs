	using HotUI.Layout;

	namespace HotUI 
{
	public class VStack : AbstractLayout 
	{
        public VStack(
            HorizontalAlignment alignment = HorizontalAlignment.Center,
            float? spacing = null) : base(new VStackLayoutManager2(alignment, spacing))
        {

        }
	}
}
