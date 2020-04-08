using System.Maui.Blazor.Components;

namespace System.Maui.Blazor.Handlers
{
	internal class ButtonHandler : BlazorHandler<Button, BButton>
	{
		public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>
		{
			{ nameof(Button.Text), MapTextProperty },
			{ nameof(Button.OnClick), MapOnClickProperty }
		};

		public ButtonHandler()
			: base(Mapper)
		{
		}

		public static void MapOnClickProperty(IViewHandler viewHandler, Button virtualView)
		{
			var nativeView = (BButton)viewHandler.NativeView;

			nativeView.OnClick = virtualView.OnClick;
		}

		public static void MapTextProperty(IViewHandler viewHandler, Button virtualView)
		{
			var nativeView = (BButton)viewHandler.NativeView;

			nativeView.Text = virtualView.Text;
		}
	}
}
