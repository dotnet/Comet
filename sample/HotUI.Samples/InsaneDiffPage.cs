using System;
namespace HotUI.Samples {
	public class InsaneDiffPage : View {
		readonly State<bool> myBoolean = new State<bool> ();
		readonly State<string> myText = new State<string> ();
		public InsaneDiffPage ()
		{
			Body = () => {
				var stack = new VStack {
					new Button (()=> myBoolean.Value ? myText.Value : $"State: {myBoolean.Value}") {
						OnClick = ()=> myBoolean.Value = !myBoolean.Value,
					},
				};
				for (var i = 0; i < 100; i++) {
					stack.Add (new Text (i.ToString ()));
				}
				return new ScrollView { stack };
			};
		}

	}
}
