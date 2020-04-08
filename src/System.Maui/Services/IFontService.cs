namespace System.Maui.Services
{
	public interface IFontService
	{
		string SystemFontName { get; }
		string MonospacedFontName { get; }
		string RoundedFontName { get; }
		string SerifFontName { get; }
	}
}
