using System;

namespace System.Maui.Samples
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
						.Fill(Color.Grey))
						.Frame(40,30,alignment: Alignment.Trailing)
						.Margin(top: 30, right: 30)
						.FitHorizontal(),

					new Label("CARD NUMBER")
						.FontSize(10)
						.Color(Color.Silver)
						.Margin(left: 30),

					new Label(Card.Number)
						.FontSize(14)
						.Color(Color.Black)
						.Margin(left: 30, bottom:20)
						.Frame(height:20),

					new HStack()
					{
						 new Label("EXPIRATION")
							.FontSize(10)
							.Color(Color.Silver)
							.Frame(width: 200),

						 new Label("CVV")
							.FontSize(10)
							.Color(Color.Silver),
					}.Margin(left:30),

					new HStack()
					{
						new Label(Card.Expiration)
							.FontSize(14)
							.Color(Color.Black)
							.Frame(width: 200),

						new Label(Card.CVV)
							.FontSize(14)
							.Color(Color.Black)
					}.Margin(left:30, bottom:30).Frame(height: 20),

				}.RoundedBorder(radius: 8, color: "#3177CB", filled: true).Margin(30)
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
				new Switch(remember),
				new Label("  Remember Me")
			}.Margin(left:20),

			new Button("Purchase for $200")
				.RoundedBorder(22, Color.SlateGrey)
				.Background(Color.SlateGrey)
				.Color(Color.White)
				.Frame(height:44)
				.Marginn((left:20, right:20),

			new Separator(),

			new Button("Or Pay with PayPal")
				.RoundedBorder(22, Color.SlateGrey)
				.Color(Color.SlateGrey)
				.Frame(height: 44)
				.Margin(left:20, right:20),


		}.FillHorizontal().Frame(alignment: Alignment.Top);

		public class Separator : ShapeView
		{
			public Separator() : base(new Rectangle().Stroke(Color.Grey, 2))
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
				new Label(_icon)
					.Frame(width: 20)
					.Margin(left: 8)
					.FontFamily("Font Awesome 5 Free"),

				new Entry(_val, _placeholder)
			}
			.Frame(height: 40)
			.RoundedBorder(color: Color.Grey);
		}
	}
}