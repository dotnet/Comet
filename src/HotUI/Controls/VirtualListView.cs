using System;
using System.Collections.Generic;
using System.Linq;

namespace HotUI
{
    public class VirtualListView : ListView
    {
        readonly FixedSizeDictionary<object, View> CurrentViews = new FixedSizeDictionary<object, View>(30);
        public override void Add(View view) => throw new NotSupportedException();

        public Func<int, int, View> Cell { get; set; }
        public Func<int, View> SectionHeader { get; set; }
        public Func<int, View> SectionFooter { get; set; }
        public View Header { get; set; }
        public View Footer { get; set; }

        protected override View HeaderView() => Header;
        protected override View FooterView() => Footer;
        protected override View ViewFor(int section, int row)
        {
            var tuple = new Tuple<int, int>(section, row);
            if (!CurrentViews.TryGetValue(tuple, out var view) || (view?.IsDisposed ?? true))
            {
                CurrentViews[tuple] = view = Cell(section, row);
                view.Parent = this;
            }
            return view;
        }

        protected override View HeaderFor(int section)
        {
            var tuple = new Tuple<string, int>(nameof(SectionHeader), section);
            if (!CurrentViews.TryGetValue(tuple, out var view) && view != null)
            {
                CurrentViews[tuple] = view = SectionHeader(section);
                view.Parent = this;
            }
            return view;
        }

        protected override View FooterFor(int section)
        {
            var tuple = new Tuple<string, int>(nameof(SectionFooter), section);
            if (!CurrentViews.TryGetValue(tuple, out var view) && view != null)
            {
                CurrentViews[tuple] = view = SectionFooter(section);
                view.Parent = this;
            }
            return view;
        }

        protected override void OnSelected(int section, int index)
        {
            var view = ViewFor(section,index);
            ItemSelected?.Invoke(section, index);
        }
        public Action<int, int> ItemSelected;
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            var currentViews = CurrentViews.ToList();
            CurrentViews.Clear();
            currentViews.ForEach(x => x.Value?.Dispose());
            base.Dispose(disposing);
        }
    }
}
