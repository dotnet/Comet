namespace HotUI.Tests.Handlers 
{
	public class TextHandler: GenericViewHandler 
	{
		public TextHandler ()
		{
			OnMeasure = HandleOnMeasure;
		}

		public Text VirtualView => (Text) CurrentView;
		
		private SizeF HandleOnMeasure(SizeF arg)
		{
			var length = VirtualView.Value?.Get()?.Length ?? 0;
			return new SizeF(10 * length, 12);
		}
	}
}
