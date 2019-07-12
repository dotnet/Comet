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
    public class SecureFieldSample1 : View
    {
        /*[State]
        readonly MyBindingObject state;*/

        readonly State<string> password = new State<string>("");
        
        public SecureFieldSample1()
        {
            Body = () => new VStack
            {
                new SecureField("Enter a password", newValue => password.Value = newValue),
                new Text(password)
            };
        }
    }
}