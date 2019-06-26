namespace HotUI.Layout
{
    public interface ILayoutManager<T>
    {
        void Layout(ILayoutHandler<T> handler, T parentView, AbstractLayout layout);
    }
}