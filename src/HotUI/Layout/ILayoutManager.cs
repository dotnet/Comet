namespace HotUI.Layout
{
    public interface ILayoutManager<T>
    {
        SizeF Measure(ILayoutHandler<T> handler, T parentView, AbstractLayout layout, SizeF available);
        void Layout(ILayoutHandler<T> handler, T parentView, AbstractLayout layout, SizeF measured);
    }
}