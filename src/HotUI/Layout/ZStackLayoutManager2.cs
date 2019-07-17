namespace HotUI.Layout
{
    public class ZStackLayoutManager2 : ILayoutManager2
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