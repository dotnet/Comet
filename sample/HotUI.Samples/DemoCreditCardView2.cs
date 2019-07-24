using System;

namespace HotUI.Samples
{
    public class DemoCreditCardView2 : View
    {
        [State]
        readonly CreditCard Card;

        readonly State<bool> remember = false;

        public DemoCreditCardView2()
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
                        .Fill(Color.Black))
                        .Frame(40,30,alignment: Alignment.Trailing)
                        .Padding(top: 30, right: 30)
                        .FitHorizontal(),

                    new Text("CARD NUMBER")
                        .FontSize(10)
                        .Color(Color.Silver)
                        .Padding(left: 30),

                    new Text(Card.Number)
                        .FontSize(14)
                        .Color(Color.Black)
                        .Padding(left: 30, bottom:20)
                        .Frame(height:20),

                    new HStack()
                    {
                         new Text("EXPIRATION")
                            .FontSize(10)
                            .Color(Color.Silver)
                            .Frame(width: 200),

                         new Text("CVV")
                            .FontSize(10)
                            .Color(Color.Silver),
                    }.Padding(left:30),

                    new HStack()
                    {
                        new Text(Card.Expiration)
                            .FontSize(14)
                            .Color(Color.Black)
                            .Frame(width: 200),

                        new Text(Card.CVV)
                            .FontSize(14)
                            .Color(Color.Black)
                    }.Padding(left:30, bottom:30).Frame(height: 20),

                }.RoundedBorder(radius: 8, color: "#3177CB", filled: true).Padding(30)
            }.Background("#f6f6f6"),

            new BorderedEntry(Card.TwoWayBinding(x => x.Number),"Enter CC Number", "\uf09d")
                .Padding(left:20, right: 20),

            new HStack(spacing:20)
            {
                new BorderedEntry(Card.TwoWayBinding(x => x.Expiration), "MM/YYYY", "\uf783")
                    .Frame(height: 40, width: 200)
                    .Padding(left:20),

                new Spacer(),

                new BorderedEntry(Card.TwoWayBinding(x => x.CVV), "CVV", "\uf023")
                    .Frame( height: 40, width: 100)
                    .Padding(right:20),
            },

            new HStack
            {
                new Toggle(remember),
                new Text("  Remember Me")
            }.Padding(left:20),

            new Button("Purchase for $200")
                .RoundedBorder(22, Color.SlateGrey)
                .Background(Color.SlateGrey)
                .Color(Color.White)
                .Frame(height:44)
                .Padding(left:20, right:20),

            new Separator(),

            new Button("Or Pay with PayPal")
                .RoundedBorder(22, Color.SlateGrey)
                .Color(Color.SlateGrey)
                .Frame(height: 44)
                .Padding(left:20, right:20),

        }.FillHorizontal().Frame(alignment: Alignment.Top);

        public class Separator : ShapeView
        {
            public Separator() : base(new Rectangle().Stroke(Color.Grey, 2))
            {
                this.Frame(height: 1);
            }
        }

        public class BorderedEntry : HStack
        {
            public BorderedEntry(Binding<String> val, string placeholder, string icon) : base(spacing: 8)
            {
                Add(new Text(icon)
                    .Frame(width: 20)
                    .Padding(left: 8)
                    .FontFamily("Font Awesome 5 Free"));

                Add(new TextField(val, placeholder));

                this.Frame(height: 40).RoundedBorder(color: Color.Grey);
              
            }
        }
    }
}