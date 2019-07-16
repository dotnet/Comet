using Android.App;
using Android.Support.V7.App;

namespace HotUI.Android
{
    public delegate void ContextChanged(Activity activity);

    public static class AndroidContext
    {
        public static event ContextChanged ContextChanged;

        private static Activity _context;

        public static Activity CurrentContext
        {
            get => _context;
            set
            {
                _context = value;
                ContextChanged?.Invoke(_context);
            }
        }

        public static AppCompatActivity AppCompatActivity => CurrentContext as AppCompatActivity;
    }
}