using System;
using System.Graphics;

namespace Comet.Samples
{
	public class Issue133c : View
	{
		[State]
		readonly CreditCard _card;

		public Issue133c()
		{
			_card = new CreditCard();
		}


		[Body]
		View body() => new VStack(spacing: 20)
		{

			new BorderedEntry(_card.Number,"Enter CC Number", "\uf09d")
				.Margin(left:20, right: 20),

		}.FillHorizontal().Frame(alignment: Alignment.Top);

		private class BorderedEntry : View
		{
			private Binding<String> _val;
			private string _placeholder;
			private string _icon;

			public BorderedEntry(Binding<String> val, string placeholder, string icon)
			{
				_val = val;
				_placeholder = placeholder;
				_icon = icon;
			}

			[Body]
			View body() => new HStack(spacing: 8)
				{
					new Text(_icon)
						.Frame(width: 20)
						.Margin(left: 8)
						.FontFamily("Font Awesome 5 Free"),

					new TextField(_val, _placeholder)
				}
				.Frame(height: 40)
				.RoundedBorder(color: Colors.Grey);
		}
	}
}
