namespace HotUI.Layout
{
    public interface ILayoutManager<T>
    {
        Size Measure(ILayoutHandler<T> handler, T parentView, AbstractLayout layout, Size available);
        void Layout(ILayoutHandler<T> handler, T parentView, AbstractLayout layout, Size measured);
    }
}