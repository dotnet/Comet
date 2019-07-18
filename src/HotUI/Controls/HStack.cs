using HotUI.Layout;

namespace HotUI 
{
	public class HStack : AbstractLayout 
	{
        public HStack(
            VerticalAlignment alignment = VerticalAlignment.Center,
            float? spacing = null) : base(new HStackLayoutManager(alignment, spacing))
        {
            
        }
    }
}
