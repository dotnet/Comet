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
namespace System.Maui.Samples
{
	public class SecureFieldSample4 : View
	{
		readonly State<string> password = "";

		[Body]
		View body() => new VStack()
		{
			new SecureField(password, "Enter a password"),
			new Text(password)
		}.FillHorizontal();
	}
}
