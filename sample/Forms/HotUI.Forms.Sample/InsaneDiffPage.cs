using System;
namespace HotUI.Forms.Sample {
	public class InsaneDiffPage : HotPage {
		public InsaneDiffPage ()
		{
			Title = "Hello Insane ScrollView";
		}

		readonly State<bool> myBoolean = new State<bool> ();

		protected override View Build () {
			var stack = new Stack {
				new Button {
					//TextBinding = ()=> $"State: {myBoolean.Value}",
					Text = $"State: {myBoolean.Value}",
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
