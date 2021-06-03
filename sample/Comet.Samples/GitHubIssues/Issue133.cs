using System;
using Microsoft.Maui.Graphics;

namespace Comet.Samples
{
	public class Issue133 : View
	{
		[State]
		readonly CreditCard Card;

		readonly State<bool> remember = false;

		public Issue133()
		{
			Card = new CreditCard();
		}


		[Body]
		View body() => new VStack(spacing: 20)
		{

			new BorderedEntry(Card.Number,"Enter CC Number", "\uf09d")
				.Margin(left:20, right: 20),

			new HStack(spacing:20)
			{
				new BorderedEntry(Card.Expiration, "MM/YYYY", "\uf783")
					.Frame(height: 40, width: 200)
					.Margin(left:20),

				new Spacer(),

				new BorderedEntry(Card.CVV, "CVV", "\uf023")
					.Frame( height: 40, width: 100)
					.Margin(right:20),
			},


		}.FillHorizontal().Frame(alignment: Alignment.Top);

		public class BorderedEntry : HStack
		{
			public BorderedEntry(Binding<String> val, string placeholder, string icon) : base(spacing: 8)
			{
				Add(new Text(icon)
					.Frame(width: 20)
					.Margin(left: 8)
					.FontFamily("Font Awesome 5 Free"));

				Add(new TextField(val, placeholder));

				this.Frame(height: 40).RoundedBorder(color: Colors.Grey);

			}
		}
	}
}
