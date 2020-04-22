using System;

namespace System.Maui.Samples
{
	public class Issue133b : View
	{
		[State]
		readonly CreditCard Card;

		readonly State<bool> remember = false;

		public Issue133b()
		{
			Card = new CreditCard();
		}


		[Body]
		View body() => new VStack(spacing: 20)
		{

			new BorderedEntry(Card.Number,"Enter CC Number", "\uf09d")
				.Margin(left:20, right: 20),

		}.FillHorizontal().Frame(alignment: Alignment.Top);

		public class BorderedEntry : HStack
		{
			public BorderedEntry(Binding<String> val, string placeholder, string icon) : base(spacing: 8)
			{
				Add(new Label (icon)
					.Frame(width: 20)
					.Margin(left: 8)
					.FontFamily("Font Awesome 5 Free"));

				Add(new Entry (val, placeholder));

				this.Frame(height: 40).RoundedBorder(color: Color.Grey);

			}
		}
	}
}
