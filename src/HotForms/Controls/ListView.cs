using System;
using System.Collections;
using FControlType = Xamarin.Forms.ListView;
using Template = Xamarin.Forms.DataTemplateSelector;
using FView = Xamarin.Forms.View;
using FCell = Xamarin.Forms.Cell;
using VCell = Xamarin.Forms.ViewCell;
using Xamarin.Forms;
using System.Collections.Generic;

namespace HotForms {

	public class ListView : ListView<object>, IEnumerable {
		public IEnumerator GetEnumerator () => FormsControl.ItemsSource.GetEnumerator ();

		List<FCell> views = new List<FCell> ();

		public void Add (FView view)
		{
			if (view == null)
				return;
			views.Add (new VCell {
				View = view,
			});
		}

		public void Add (FCell view)
		{
			if (view == null)
				return;

			views.Add (view);
		}

		void CheckCanAddView ()
		{
			if (ViewFor != null && views.Count > 0)
				throw new Exception ("You cannot add Views and a ViewFor");
			if (views.Count > 0 && ItemsSource != null)
				throw new Exception ("You cannot add Views and use an ItemSource");

		}

		public void Add (View view)
		{
			if (view == null)
				return;
			views.Add (new VCell { View = view });
		}


		protected override object CreateFormsView ()
		{
			CheckCanAddView ();

			var list = (FControlType)base.CreateFormsView ();
			if(views.Count > 0) {
				list.ItemTemplate = new CellTemplate ();
			}
			return list;
		}

		class CellTemplate : Template {
			protected override DataTemplate OnSelectTemplate (object item, BindableObject container) =>
				new DataTemplate (() => (Cell)item);
		}
	}


	public class ListView<T> : View<FControlType>  {

		public IEnumerable ItemsSource { get; set; }
		public Func<T,FView> ViewFor { get; set; }
		public Action<T> ItemSelected { get; set; }

		protected override object CreateFormsView ()
		{
			var control = (FControlType)base.CreateFormsView ();
			control.HasUnevenRows = true;
			if(ItemsSource != null) {
				control.ItemsSource = ItemsSource;
			}
			if (ViewFor != null)
				control.ItemTemplate = new ListViewCellTemplate<T> { CellFor = ViewFor };
			if(ItemSelected != null)
				control.ItemSelected += Control_ItemSelected;
			return control;
		}

		private void Control_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			ItemSelected?.Invoke ((T)e.SelectedItem);
		}

		class ListViewCellTemplate<T> : Template {
			public Func<T, FView> CellFor { get; set; }
			protected override DataTemplate OnSelectTemplate (object item, BindableObject container)
			{
				return new DataTemplate (() => {
					return new ViewCell {
						View = CellFor.Invoke ((T)item)
					};
				});
			}
		}
	}

	//Going to add a custom renderer, so it doesnt use forms horrible abstraction over tables views
	public class GroupedListView<T> : View<FControlType> {

		public Func<int> NumberOfSections { get; set; }
		public Func<int,int> RowsInSections { get; set; }
		public Func<(int section, int row), T> ObjectForRow { get; set; }
		public Func<T, FView> ViewFor { get; set; }

		public Action<T> ItemSelected { get; set; }

		protected override object CreateFormsView ()
		{
			var control = (FControlType)base.CreateFormsView ();
			control.HasUnevenRows = true;
			if (ViewFor != null)
				control.ItemTemplate = new ListViewCellTemplate<T> { CellFor = ViewFor };
			if (ItemSelected != null)
				control.ItemSelected += Control_ItemSelected;
			return control;
		}

		private void Control_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			ItemSelected?.Invoke ((T)e.SelectedItem);
		}

		class ListViewCellTemplate<T> : Template {
			public Func<T, FView> CellFor { get; set; }
			protected override DataTemplate OnSelectTemplate (object item, BindableObject container)
			{
				return new DataTemplate (() => {
					return new ViewCell {
						View = CellFor.Invoke ((T)item)
					};
				});
			}
		}
	}

}
