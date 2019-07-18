namespace HotUI.Layout
{
    public class ZStackLayoutManager : ILayoutManager
    {
        public SizeF Measure(AbstractLayout layout, SizeF available)
        {
            return available;
        }

        public void Layout(AbstractLayout layout, SizeF measured)
        {
            
        }
    }
}