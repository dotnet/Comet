using System;
using Microsoft.Maui.Graphics;

namespace Comet.Samples
{
	public class DemoCreditCardView3 : View
	{
		[State]
		readonly CreditCard Card;

		readonly State<bool> remember = false;

		public DemoCreditCardView3()
		{
			Card = new CreditCard();
		}


		[Body]
		View body() => new VStack(spacing: 20)
		{
			new VStack()
			{
				new VStack()
				{
					new ShapeView(new RoundedRectangle(4.0f)
						.Style(Graphics.DrawingStyle.Fill)
						.Fill(Colors.Grey))
						.Frame(40,30,alignment: Alignment.Trailing)
						.Margin(top: 30, right: 30)
						.FitHorizontal(),

					new Text("CARD NUMBER")
						.FontSize(10)
						.Color(Colors.Silver)
						.Margin(left: 30),

					new Text(Card.Number)
						.FontSize(14)
						.Color(Colors.Black)
						.Margin(left: 30, bottom:20)
						.Frame(height:20),

					new HStack()
					{
						 new Text("EXPIRATION")
							.FontSize(10)
							.Color(Colors.Silver)
							.Frame(width: 200),

						 new Text("CVV")
							.FontSize(10)
							.Color(Colors.Silver),
					}.Margin(left:30),

					new HStack()
					{
						new Text(Card.Expiration)
							.FontSize(14)
							.Color(Colors.Black)
							.Frame(width: 200),

						new Text(Card.CVV)
							.FontSize(14)
							.Color(Colors.Black)
					}.Margin(left:30, bottom:30).Frame(height: 20),

				}.RoundedBorder(radius: 8, color: Color.FromHex("#3177CB"), filled: true).Margin(30)
			}.Background("#f6f6f6"),

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

			new HStack
			{
				new Toggle(remember),
				new Text("  Remember Me")
			}.Margin(left:20),

			new Button("Purchase for $200")
				.RoundedBorder(22, Colors.SlateGrey)
				.Background(Colors.SlateGrey)
				.Color(Colors.White)
				.Frame(height:44)
				.Margin(left:20, right:20),

			new Separator(),

			new Button("Or Pay with PayPal")
				.RoundedBorder(22, Colors.SlateGrey)
				.Color(Colors.SlateGrey)
				.Frame(height: 44)
				.Margin(left:20, right:20),


		}.FillHorizontal().Frame(alignment: Alignment.Top);

		public class Separator : ShapeView
		{
			public Separator() : base(new Shapes.Rectangle().Stroke(Colors.Grey, 2))
			{
				this.Frame(height: 1);
			}
		}

		public class BorderedEntry : View
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