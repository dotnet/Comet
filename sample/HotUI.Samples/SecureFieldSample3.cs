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
    public class SecureFieldSample3 : View
    {
        readonly State<string> password = new State<string>("");
        
        public SecureFieldSample3()
        {
            Body = () => new VStack
            {
                new SecureField("Enter a password", password),
                new Text(password)
            };
        }
    }
}