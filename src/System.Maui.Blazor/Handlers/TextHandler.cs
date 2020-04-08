using System.Maui.Blazor.Components;

namespace System.Maui.Blazor.Handlers
{
	internal class TextHandler : BlazorHandler<Text, BText>
	{
		public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>
		{
			[nameof(Text.Value)] = MapValueProperty
		};

		public TextHandler()
			: base(Mapper)
		{
		}

		public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
		{
			var nativeView = (BText)viewHandler.NativeView;
			nativeView.Value = virtualView.Value;
		}
	}
}
