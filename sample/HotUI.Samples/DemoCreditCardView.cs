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

    //class MyBindingObject : BindingObject
    //{
    //    public bool CanEdit
    //    {
    //        get => GetProperty<bool>();
    //        set => SetProperty(value);
    //    }
    //    public string Text
    //    {
    //        get => GetProperty<string>();
    //        set => SetProperty(value);
    //    }
    //}

    public class DemoCreditCardView : View
    {
        readonly State<string> password = "";
        readonly State<string> ccnumber = "";
        readonly State<string> ccexpiration = "";
        readonly State<string> cccvv = "";
        readonly State<bool> remember = false;

        Color titleColor = new Color("#1d1d1d");
        Color ccColor = new Color("#999999");



        [Body]
        View body() => new VStack(sizing: Sizing.Fill)
        {
            new Spacer(),
            TitleText("Card Number"),
            CCText(ccnumber),
            TitleText("Expiration"),
            CCText(ccexpiration),
            TitleText("CVV"),
            CCText(cccvv),
            HRule(),
            new Spacer(),
            new TextField(ccnumber, "Enter a CC Number"),
            new TextField(ccexpiration, "MM/YY"),
            new TextField(cccvv, "CVV"),
            new HStack{
                new Toggle(remember),
                new Text("Remember Me")
            },
            new Button("Or Pay with PayPal"),
            new Spacer(),
            HRule(),
            new Button("Purchase for $200"),
            new Spacer(),
        };

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

        //class CCText : Text
        //{
        //    public CCText(Binding<string> val) : base(val)
        //    {
        //    }

        //}
    }
}