using HotUI.Layout;

namespace HotUI 
{
	public class HStack : AbstractLayout 
	{    
        public HStack(
            VerticalAlignment alignment = VerticalAlignment.Center,
            float? spacing = null,
            Sizing sizing = Sizing.Fit) : base(new HStackLayoutManager(alignment, spacing, sizing))
        {
            
        }
    }
}
