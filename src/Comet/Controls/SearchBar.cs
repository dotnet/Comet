using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class SearchBar : TextField, ISearchBar
	{
		public SearchBar(Binding<string> value = null, string placeholder = null, Action<string> onEditingChanged = null, Action<string> onSearch = null) : base(value, placeholder, onEditingChanged, onSearch)
		{
			OnSearch = onSearch;
		}

		public SearchBar(Func<string> value, string placeholder = null, Action<string> onEditingChanged = null, Action<string> onSearch = null) : base(value, placeholder, onEditingChanged, onSearch)
		{
			OnSearch = onSearch;
		}

		public Action<string> OnSearch { get; private set; }
		Color ISearchBar.CancelButtonColor => this.GetEnvironment<Color>(nameof(ISearchBar.CancelButtonColor));

		void ISearchBar.SearchButtonPressed() => OnSearch?.Invoke(Text);
	}
}
