using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HotUI {

	public class ListView<T> : ListView
	{
		public ListView (IList<T> list) : base((IList)list)
		{

		}
		public void Add (Func<T, View> viewCreator)
		{
			if (CellCreator != null) {
				throw new Exception ("You can only have one View Creator");
			}
			CellCreator =  (o) => viewCreator?.Invoke((T)o);
		}
		public void Add (Action<T> onTap)
		{
			this.ItemSelected = (o) => onTap?.Invoke ((T)o);
		}

		public Func<T, View> Cell
		{
			get => o => CellCreator?.Invoke(o);
			set => CellCreator = o => value.Invoke((T)o);
		}
	
		// todo: this doesn't do anything, just added this for prototyping purposes.
		public Func<object, View> Header { get; set; }
		
	}

	public class ListView : View, IEnumerable, IEnumerable<Func<object,View>> {

        //TODO Evaluate if 30 is a good number
        FixedSizeDictionary<object, View> CurrentViews = new FixedSizeDictionary<object, View>(30);

		public ListView(IList list)
		{
			List = list;
            //Dispose that View!
            CurrentViews.OnDequeue = (pair) => pair.Value?.Dispose();
		}

		public IList List { get; }
		public IEnumerator GetEnumerator () => List.GetEnumerator ();
		public Func<object, View> CellCreator { get; protected set; }
		public void Add(Func<object,View> viewCreator)
		{
			if(CellCreator != null) {
				throw new NotImplementedException ("You can only have one View Creator");
			}
			CellCreator = viewCreator;
		}

		IEnumerator<Func<object, View>> IEnumerable<Func<object, View>>.GetEnumerator ()
		{
			throw new NotImplementedException ();
		}

		public Action<object> ItemSelected { get; set; }

        public View ViewFor(int index)
        {
            var item = List[index];
            if (!CurrentViews.TryGetValue(item, out var view))
            {
                CurrentViews[item] = view = CellCreator(item);
                view.Parent = this;
            }
            return view;            
        }

		public void OnSelected (int index)
		{
			var item = List [index];
			if (CellCreator?.Invoke (item) is NavigationButton navigation) {
				navigation.Parent = this;
				navigation.Navigate ();
				return;
			}
			ItemSelected?.Invoke (item);
		}

		public void OnSelected (object item) => ItemSelected?.Invoke (item);

        protected override void Dispose(bool disposing)
        {
            var currentViews = CurrentViews.ToList();
            CurrentViews.Clear();
            currentViews.ForEach(x => x.Value?.Dispose());
            //TODO: Verify. I don't think we need to check all active views anymore
            var cells = ActiveViews.Where(x => x.Parent == this).ToList();
            foreach (var cell in cells)
                cell.Dispose();
            base.Dispose(disposing);
        }
        protected override void OnParentChange(View parent)
        {
            base.OnParentChange(parent);
            foreach (var pair in CurrentViews)
                pair.Value.Parent = parent;
        }
    }
}
