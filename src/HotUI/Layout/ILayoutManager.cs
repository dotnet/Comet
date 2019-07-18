namespace HotUI.Layout
{
    public interface ILayoutManager
    {
        SizeF Measure(AbstractLayout layout, SizeF available);
        void Layout(AbstractLayout layout, SizeF measured);
    }
}