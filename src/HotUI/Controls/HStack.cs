using HotUI.Layout;

namespace HotUI 
{
	public class HStack : AbstractLayout 
	{
        public HStack(
            VerticalAlignment alignment = VerticalAlignment.Center,
            float? spacing = null) : base(new HStackLayoutManager2(alignment, spacing))
        {
            
        }
    }
}
