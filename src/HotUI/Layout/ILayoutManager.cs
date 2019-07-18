namespace HotUI.Layout
{
    public interface ILayoutManager2
    {
        SizeF Measure(AbstractLayout layout, SizeF available);
        void Layout(AbstractLayout layout, SizeF measured);
    }
}