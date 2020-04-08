namespace System.Maui.Services
{
	public class FallbackFontService : IFontService
	{
		public string SystemFontName => "Arial";
		public string MonospacedFontName => "Courier";
		public string RoundedFontName => "Arial";
		public string SerifFontName => "Times";
	}
}
