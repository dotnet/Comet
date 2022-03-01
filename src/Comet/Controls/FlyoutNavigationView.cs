using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet
{
	public class FlyoutNavigationView<T> : FlyoutView, IFlyoutView
	{
		ListView<T> listView;
		Binding<IReadOnlyList<T>> _items;
		View detailView;
		Binding<IReadOnlyList<T>> Items
		{
			get => _items;
			set => this.SetBindingValue(ref _items, value);
		}

		Binding<int> _currentIndex;
		public Binding<int> CurrentIndex
		{
			get => _currentIndex;
			private set => this.SetBindingValue(ref _currentIndex, value);
		}
		public FlyoutNavigationView(Binding<IReadOnlyList<T>> items, Binding<int> currentIndex = null)
		{
			CurrentIndex = currentIndex;
			Items = items;
			Setup();
		}

		public FlyoutNavigationView(Func<IReadOnlyList<T>> items, Func<int> currentIndex = null, Func<double> flyoutWidth = null) : this((Binding<IReadOnlyList<T>>)items, (Binding<int>)currentIndex)
		{

		}

		void Setup()
		{
			listView = new ListView<T>(Items)
			{
				ViewFor = (t) => MenuViewFor(t),
				ItemSelected = (t) => {
					var v = DetailViewFor?.Invoke((T)t.item);
					SetDetail(v);
					CurrentIndex.Set?.Invoke(t.row);
				}
			};


		}

		IView IFlyoutView.Flyout => listView;
		IView IFlyoutView.Detail => detailView ??= getDetailView();
		public Func<T, View> MenuViewFor { get; set; } = (t) => {
			var title = t.ToString();
			if (t is View v)
				title = v.Title;
			return new Text(title);
		};
		View getDetailView()
		{
			var t = Items.CurrentValue[CurrentIndex.CurrentValue];
			var v = DetailViewFor?.Invoke(t);
			return v;
		}
		public Func<T, View> DetailViewFor { get; set; }
		protected void SetDetail(View view)
		{
			detailView = view;
			ViewHandler?.UpdateValue(nameof(IFlyoutView.Detail));
		}

	}
}
