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
		public ListView(IList list)
		{
			List = list;
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
            var cells = ActiveViews.Where(x => x.Parent == this).ToList();
            foreach (var cell in cells)
                cell.Dispose();
            base.Dispose(disposing);
        }
    }
}
