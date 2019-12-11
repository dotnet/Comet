using Comet.Services;
using UIKit;

namespace Comet.iOS.Services
{
	public class iOSFontService : IFontService
	{
		private string _systemFontName;
		private string _monospacedFontName;
		private string _roundedFontName;
		private string _serifFontName;

		public string SystemFontName
		{
			get
			{
				if (_systemFontName == null)
				{
					var font = UIFont.SystemFontOfSize(12);
					_systemFontName = font.Name;
					font.Dispose();
				}

				return _systemFontName;
			}
		}

		public string MonospacedFontName
		{
			get
			{
				if (_monospacedFontName == null)
				{
					var font = UIFont.MonospacedDigitSystemFontOfSize(12, UIFontWeight.Regular);
					_monospacedFontName = font.Name;
					font.Dispose();
				}

				return _monospacedFontName;
			}
		}

		public string RoundedFontName
		{
			get
			{
				// todo: figure out what this should be.  For now, just use the system font.
				if (_roundedFontName == null)
				{
					var font = UIFont.SystemFontOfSize(12);
					_roundedFontName = font.Name;
					font.Dispose();
				}

				return _roundedFontName;
			}
		}

		public string SerifFontName
		{
			get
			{
				if (_serifFontName == null)
				{
					// todo: figure out what this should be.  For now, just use times new roman font.
					var font = UIFont.FromName("TimesNewRoman", 12);
					_serifFontName = font.Name;
					font.Dispose();
				}

				return _serifFontName;
			}
		}
	}
}
