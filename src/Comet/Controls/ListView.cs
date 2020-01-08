using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Comet.Internal;

namespace Comet
{
	public interface IListView
	{
		int Sections();
		int Rows(int section);
		View FooterView();
		View HeaderView();
		object ItemAt(int section, int index);
		View ViewFor(int section, int index);
		View HeaderFor(int section);
		View FooterFor(int section);
		bool ShouldDisposeViews { get; }
		void OnSelected(int section, int index);
	}

	public class ListView<T> : ListView
	{
		//TODO Evaluate if 30 is a good number
		protected IDictionary<object, View> CurrentViews { get; }

		readonly Binding<IReadOnlyList<T>> itemsBinding;
		IReadOnlyList<T> items;

		public ListView(Binding<IReadOnlyList<T>> items) : this()
		{

			this.itemsBinding = items;
			this.items = items?.CurrentValue;
			SetupObservable();
		}

		public ListView()
		{
			if (HandlerSupportsVirtualization)
			{
				CurrentViews = new FixedSizeDictionary<object, View>(150)
				{
					OnDequeue = (pair) =>
					{
						var view = pair.Value;
						if (view?.ViewHandler?.NativeView == null)
							view.Dispose();
						else
							CurrentViews[pair.Key] = view;
					}
				};
			}
			else
				CurrentViews = new Dictionary<object, View>();

			ShouldDisposeViews = true;
		}
		public override void ViewPropertyChanged(string property, object value)
		{
			//Update this when things change!
			DisposeObservable();
			items = itemsBinding?.CurrentValue;
			SetupObservable();
			base.ViewPropertyChanged(property, value);
		}

		void SetupObservable()
		{
			if (!(items is ObservableCollection<T> observable))
				return;
			observable.CollectionChanged += Observable_CollectionChanged;
		}

		protected virtual void Observable_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			ReloadData();
		}

		void DisposeObservable()
		{
			if (!(items is ObservableCollection<T> observable))
				return;

			observable.CollectionChanged -= Observable_CollectionChanged;
		}


		public Func<T, View> ViewFor { get; set; }

		public Func<int, T> ItemFor { get; set; }

		public Func<int> Count { get; set; }

		protected override int GetCount(int section) => items?.Count() ?? Count?.Invoke() ?? 0;

		protected override object GetItemAt(int section, int index) => items.SafeGetAtIndex(index, ItemFor);

		protected override View GetViewFor(int section, int index)
		{
			var item = (T)GetItemAt(section, index);
			if (item == null)
				return null;
			if (!CurrentViews.TryGetValue(item, out var view) || (view?.IsDisposed ?? true))
			{
				using (new StateBuilder(this))
				{
					view = ViewFor?.Invoke(item);
					if (item is INotifyPropertyRead read && view != null)
						StateManager.MonitorListViewObject(view, read);
				}
				if (view == null)
					return null;
				CurrentViews[item] = view;
				view.Parent = this;
			}
			return view;
		}

		public override void Add(View view) => throw new NotSupportedException("You cannot add a View directly to a Typed ListView");


		protected override void Dispose(bool disposing)
		{
			if (!disposing)
				return;

			DisposeObservable();

			var currentViews = CurrentViews?.ToList();
			CurrentViews?.Clear();
			currentViews?.ForEach(x => x.Value?.Dispose());
			base.Dispose(disposing);
		}
	}

	public class ListView : View, IEnumerable, IEnumerable<View>, IListView
	{
		public static bool HandlerSupportsVirtualization { get; set; } = true;

		List<View> views;

		public View Header { get; set; }

		public View Footer { get; set; }

		public Action<object> ItemSelected { get; set; }

		protected virtual int GetSections() => 1;

		protected virtual int GetCount(int section) => views?.Count ?? 0;

		protected virtual View GetHeaderFor(int section) => null;

		protected virtual View GetFooterFor(int section) => null;

		protected virtual object GetItemAt(int section, int index) => views?[index];

		protected virtual View GetViewFor(int section, int index) => views?[index];

		public virtual void Add(View view)
		{
			if (views == null)
				views = new List<View>();
			views.Add(view);
		}

		public virtual void ReloadData()
		{
			ViewHandler?.UpdateValue(nameof(ReloadData), null);
		}

		protected virtual void OnSelected(int section, int index)
		{
			var item = GetItemAt(section, index);
			ItemSelected?.Invoke(item);
		}

		protected override void Dispose(bool disposing)
		{
			views?.ForEach(v => v.Dispose());
			//TODO: Verify. I don't think we need to check all active views anymore
			var cells = ActiveViews.Where(x => x.Parent == this).ToList();
			foreach (var cell in cells)
				cell.Dispose();
			base.Dispose(disposing);
		}

		protected override void OnParentChange(View parent)
		{
			base.OnParentChange(parent);
			views?.ForEach(v => v.Parent = parent);
		}
		protected bool ShouldDisposeViews { get; set; }

		bool IListView.ShouldDisposeViews => ShouldDisposeViews;

		IEnumerator<View> IEnumerable<View>.GetEnumerator() => views?.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => views?.GetEnumerator();

		int IListView.Sections() => GetSections();

		int IListView.Rows(int section) => GetCount(section);

		View IListView.FooterView() => Footer;

		View IListView.HeaderView() => Header;

		object IListView.ItemAt(int section, int index) => GetItemAt(section, index);

		View IListView.ViewFor(int section, int index) => GetViewFor(section, index);

		View IListView.HeaderFor(int section) => GetHeaderFor(section);

		View IListView.FooterFor(int section) => GetFooterFor(section);

		void IListView.OnSelected(int section, int index) => OnSelected(section, index);
	}


	public class Section : View, IEnumerable<View>
	{
		public Section(Binding<IList<View>> views, View header = null, View footer = null)
		{

		}
		public Section(View header = null, View footer = null)
		{
			Header = header;
			Footer = footer;
		}
		public View Header { get; set; }
		public View Footer { get; set; }

		List<View> views;
		public virtual void Add(IEnumerable<View> views)
		{
			if (this.views == null)
				this.views = new List<View>();
			this.views.AddRange(views);
		}
		public virtual void Add(View view)
		{
			if (views == null)
				views = new List<View>();
			views.Add(view);
		}
		public virtual object GetItemAt(int index) => views?[index];
		public virtual int GetCount() => views?.Count ?? 0;
		public virtual View GetViewFor(int index) => (View)GetItemAt(index);

		public IEnumerator<View> GetEnumerator() => views?.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => views?.GetEnumerator();
	}

	public class Section<T> : Section
	{
		readonly Binding<IReadOnlyList<T>> itemsBinding;
		IReadOnlyList<T> items;

		public Section() { }

		public Section(Binding<IReadOnlyList<T>> items)
		{
			this.itemsBinding = items;
			this.items = items?.CurrentValue;
		}

		public override void ViewPropertyChanged(string property, object value)
		{
			//Update this when things change!
			items = itemsBinding?.CurrentValue;
			base.ViewPropertyChanged(property, value);
		}

		public Func<int> Count { get; set; }

		public Func<T, View> ViewFor { get; set; }

		public Func<int, T> ItemFor { get; set; }

		public override object GetItemAt(int index) => items.SafeGetAtIndex(index, ItemFor);
		public override View GetViewFor(int index)
		{
			var item = (T)GetItemAt(index);
			using (new StateBuilder(this))
			{
				var view = ViewFor?.Invoke(item);
				//TODO: Make sure we clean this up. This is a memory leak!!!!
				if (item is INotifyPropertyRead read && view != null)
					StateManager.MonitorListViewObject(view, read);
				return view;
			}
		}
		public override int GetCount() => items?.Count ?? Count?.Invoke() ?? 0;

		protected override void OnParentChange(View parent)
		{
			base.OnParentChange(parent);
			Header?.SetParent(parent);
			Footer?.SetParent(parent);
		}

	}

	public class SectionedListView<T> : ListView<T>
	{

		Dictionary<int, Section<T>> sectionCache = new Dictionary<int, Section<T>>();

		public override void Add(View view) => throw new NotSupportedException("You cannot add a View directly to a SectionedListView");

		List<Section<T>> sections;
		public virtual void Add(Section<T> section)
		{
			if (sections == null)
				sections = new List<Section<T>>();
			sections.Add(section);
		}

		public Func<int, Section<T>> SectionFor { get; set; }

		public Func<int> SectionCount { get; set; }


		protected override int GetSections() => sections?.Count() ?? SectionCount?.Invoke() ?? 0;
		protected override View GetHeaderFor(int section) => sections.SafeGetAtIndex(section, GetCachedSection)?.Header?.SetParent(this);
		protected override View GetFooterFor(int section) => sections.SafeGetAtIndex(section, GetCachedSection)?.Footer?.SetParent(this);
		protected override object GetItemAt(int section, int index) => sections.SafeGetAtIndex(section, GetCachedSection)?.GetItemAt(index);
		protected override int GetCount(int section) => sections.SafeGetAtIndex(section, GetCachedSection)?.GetCount() ?? 0;

		protected override View GetViewFor(int section, int index)
		{
			var item = (T)GetItemAt(section, index);
			if (item == null)
				return null;
			var key = (section, item);
			if (!CurrentViews.TryGetValue(key, out var view) || (view?.IsDisposed ?? true))
			{
				view = sections.SafeGetAtIndex(section, GetCachedSection)?.GetViewFor(index)?.SetParent(this);
				if (view == null)
					return null;
				CurrentViews[key] = view;
			}
			return view;

		}
		protected Section<T> GetCachedSection(int index)
		{
			if (!sectionCache.TryGetValue(index, out var section))
			{
				section = SectionFor?.Invoke(index);
				if (section != null)
				{
					section.Parent = this;
					sectionCache[index] = section;
				}
			}
			return section;
		}
		public override void ReloadData()
		{
			sectionCache.Clear();
			base.ReloadData();
		}
		protected override void Dispose(bool disposing)
		{
			var cached = sectionCache.Values.ToList();
			cached.ForEach(s => s.Dispose());
			sectionCache.Clear();
			base.Dispose(disposing);
		}
		protected override void OnParentChange(View parent)
		{
			sections?.ForEach(section => section.Parent = this);

			foreach (var pair in sectionCache)
				pair.Value.Parent = this;
			base.OnParentChange(parent);
		}
	}

	public class SectionedListView : ListView
	{
		public SectionedListView()
		{

		}
		public SectionedListView(IList<Section> sections)
		{
			this.sections = sections;
		}
		public override void Add(View view) => throw new NotSupportedException("You cannot add a View directly to a SectionedListView");

		IList<Section> sections;
		public virtual void Add(Section section)
		{
			if (sections == null)
				sections = new List<Section>();
			sections.Add(section);
		}

		public virtual void Add(IEnumerable<Section> items)
		{
			if (this.sections == null)
				this.sections = new List<Section>();
			((List<Section>)this.sections).AddRange(items);
		}
		protected override int GetSections()
		{
			var s = sections?.Count() ?? 0;
			return s;
		}
		protected override View GetHeaderFor(int section) => sections?[section]?.Header?.SetParent(this);
		protected override View GetFooterFor(int section) => sections?[section]?.Footer?.SetParent(this);
		protected override object GetItemAt(int section, int index) => sections?[section]?.GetItemAt(index);
		protected override int GetCount(int section)
		{
			var count = sections?[section]?.GetCount() ?? 0;
			return count;
		}
		protected override View GetViewFor(int section, int index) => sections?[section]?.GetViewFor(index)?.SetParent(this);

		protected override void OnParentChange(View parent)
		{
			foreach (var section in sections)
				section.Parent = this;
			base.OnParentChange(parent);
		}
	}
}
