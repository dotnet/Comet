using System;
namespace HotUI.Forms.Sample {
	public class InsaneDiffPage : HotPage {
		public InsaneDiffPage ()
		{
		}

		readonly State<bool> myBoolean = new State<bool> ();

		protected override View Build () {
			var stack = new Stack {
				new Button {
					Text = $"State: {myBoolean.Value}",
					OnClick = ()=> myBoolean.Value = !myBoolean.Value,
				},
			};
			for(var i = 0; i < 1000; i++) {
				stack.Add (new Label { Text = i.ToString () });
			}
			return new ScrollView { stack };
		}

	}
}
