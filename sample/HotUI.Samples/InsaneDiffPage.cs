using System;
namespace HotUI.Samples {
	public class InsaneDiffPage : HotPage {
		public InsaneDiffPage ()
		{
			Title = "Hello Insane ScrollView";
		}

		readonly State<bool> myBoolean = new State<bool> ();
		readonly State<string> myText = new State<string> ();

		protected override View Build () {
			var stack = new Stack {
				new Button {
					TextBinding = ()=> myBoolean.Value ? myText.Value : $"State: {myBoolean.Value}",
					//Text = $"State: {myBoolean.Value}",
					OnClick = ()=> myBoolean.Value = !myBoolean.Value,
				},
			};
			for(var i = 0; i < 100; i++) {
				stack.Add (new Label { Text = i.ToString () });
			}
			return new ScrollView { stack };
		}

	}
}
