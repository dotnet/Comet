using System;
using System.Collections.Generic;

/*

struct ContentView : View {
    @State private var password: String = ""

    var body: some View {
        VStack {
            SecureField("Enter a password", text: $password)
            Text("You entered: \(password)")    
        }
    }
}

*/
namespace HotUI.Samples
{

    class CreditCard : BindingObject
    {
        public string Number
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string Expiration
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string CVV
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string Name
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
    }

    public class DemoCreditCardView : View
    {
        [State]
        readonly CreditCard Card;

        readonly State<bool> remember = false;

        Color titleColor = new Color("#1d1d1d");
        Color ccColor = new Color("#999999");

        public DemoCreditCardView()
        {
            Card = new CreditCard();
        }

        [Body]
        View body() => new Grid(
            rows: new object[] { "250", 20, 160, 20, 44, 20, 1, 20, 44, "*" },
            columns: new object[] { 20, "*", 20 }
            )
        {
            new Grid(
                rows: new object[] { 30,"*",30},
                columns: new object[] { 30, "*", 30 })
            {
                // thecredit card display at the top
                new ShapeView(
                    new RoundedRectangle(8)
                        .Fill(Color.CornflowerBlue)
                        .Style(Graphics.DrawingStyle.Fill)
                ).Cell(row:1, column:1)
            }.Cell(row:0, column:0, colSpan:3).Background(new Color("#f6f6f6")).Frame(height:250),
            new Grid(
                rows: new object[] { 40, 20, 40, 20, 40, 20, 44, 20, 1, 20, 44 },
                columns: new object[] { "2*", 20, "*" })
            {
                EntryContainer(Card.Number, "Enter CC Number").Cell(row:0, column: 0, colSpan: 3),  
                EntryContainer(Card.Expiration, "MM/YYYY").Cell(row:2, column: 0),
                EntryContainer(Card.CVV, "CVV").Cell(row:2, column: 2),
                new HStack
                {
                    new Toggle(remember),
                    new Text("  Remember Me")
                }.Cell(row:4,column:0, colSpan: 3), 
                new Button("Or Pay with PayPal").RoundedBorder(22, Color.SlateGrey).Cell(row:6, column:0, colSpan:3).Color(Color.SlateGrey),
                HRule().Cell(row:8,column:0,colSpan:3),
                new Button("Purchase for $200").RoundedBorder(22, Color.SlateGrey).Background(Color.SlateGrey).Cell(row:10,column:0,colSpan:3).Color(Color.White)
            }.Cell(row:2, column:1),

        };

        View HRule()
        {
            return new ShapeView(
                new Rectangle()
                    .Stroke(Color.Grey, 2)
                )
                .Frame(100, 1);
        }

        Text CCText(Binding<string> val)
        {
            return new Text(val)
                .Frame(height: 24)
                .FontSize(12)
                .Color(ccColor);
        }

        Text TitleText(Binding<string> val)
        {
            return new Text(val)
                .FontSize(24)
                .Color(titleColor);
        }

        HStack EntryContainer(Binding<String> val, string placeholder)
        {
            return new HStack(spacing:10)
            {
                    new Text("").Frame(width:5),
                    new TextField(val, placeholder).Padding(top:9)
                
            }.RoundedBorder(color: Color.Grey).FillHorizontal();
        }

        //class CCText : Text
        //{
        //    public CCText(Binding<string> val) : base(val)
        //    {
        //    }

        //}
    }

    
}