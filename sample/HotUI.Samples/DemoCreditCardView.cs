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
        CreditCard Card;

        readonly State<bool> remember = false;

        Color titleColor = new Color("#1d1d1d");
        Color ccColor = new Color("#999999");

        public DemoCreditCardView()
        {
            Card = new CreditCard();
        }

        [Body]
        View body() => new Grid(
            rows: new object[] { "300", "*", "*", "*", "*", "*", "*" },
            columns: new object[] { "*", "5*", "5*" }
            )
        {
            new Grid(){
                new ShapeView(
                    new RoundedRectangle(8)
                        .Stroke(Color.Black, 2.0f)
                        .Fill(Color.Red)
                )
                .Frame(300,100)
                .FillHorizontal()
                .FillHorizontal()
            }
            .Cell(row:0, column:0, colSpan:3),
            EntryContainer(Card.Number, "Enter CC Number")
                .Cell(row:1,column:0,colSpan:3),
            EntryContainer(Card.Expiration, "MM/YYYY").Cell(row:2,column:0,colSpan:2),
            EntryContainer(Card.CVV, "CVV").Cell(row:2,column:2),
            new HStack{
                new Toggle(remember),
                new Text("Remember Me")
            }.Cell(row:3,column:0),
            new Button("Or Pay with PayPal").Cell(row:4, column:0, colSpan:3),
            HRule().Cell(row:5,column:0,colSpan:3),
            new Button("Purchase for $200").Cell(row:6,column:0,colSpan:3)



        }.FillVertical();

        //View body() => new VStack(spacing:10)
        //{
        //    //new Spacer(),
        //    TitleText("Card Number"),
        //    CCText(Card. Number),
        //    TitleText("Expiration"),
        //    CCText(Card.Expiration),
        //    TitleText("CVV"),
        //    CCText(Card.CVV),
        //    HRule(),
        //    //new Spacer(),
        //    new TextField(Card.Number, "Enter a CC Number"),
        //    new TextField(Card.Expiration, "MM/YY"),
        //    new TextField(Card.CVV, "CVV"),
        //    new HStack{
        //        new Toggle(remember),
        //        new Text("Remember Me")
        //    },
        //    new Button("Or Pay with PayPal"),
        //    //new Spacer(),
        //    HRule(),
        //    new Button("Purchase for $200"),
        //    //new Spacer(),
        //}
        //.FillHorizontal()
        //.FillVertical()

        //    ;

        View HRule()
        {
            return new ShapeView(
                new Rectangle()
                    .Stroke(Color.Black, 2)
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
            return new HStack(spacing:20)
                {
                    new Image(""),
                    new TextField(val, placeholder)
                
            };
        }

        //class CCText : Text
        //{
        //    public CCText(Binding<string> val) : base(val)
        //    {
        //    }

        //}
    }

    
}