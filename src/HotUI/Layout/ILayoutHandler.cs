using System.Collections.Generic;
using System.Drawing;

namespace HotUI.Layout
{
    public interface ILayoutHandler<T>
    {
        Size Measure(T view, Size available);
        Size GetSize(T view);
        void SetSize(T view, float width, float height);
        void SetFrame(T view, float x, float y, float width, float height);
        IEnumerable<T> GetSubviews();
    }
}