using HotUI.Services;
using AppKit;

namespace HotUI.Mac.Services
{
    public class MacFontService : IFontService
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
                    var font = NSFont.SystemFontOfSize(12);
                    _systemFontName = font.FamilyName;
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
                    var font = NSFont.MonospacedDigitSystemFontOfSize(12, (int)Weight.Regular);
                    _monospacedFontName = font.FamilyName;
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
                    var font = NSFont.SystemFontOfSize(12);
                    _roundedFontName = font.FamilyName;
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
                    var font = NSFont.FromFontName("TimesNewRoman", 12);
                    _serifFontName = font.FamilyName;
                    font.Dispose();
                }

                return _serifFontName;
            }
        }
    }
}