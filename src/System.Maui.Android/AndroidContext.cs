using Android.App;
using Android.Support.V7.App;

namespace System.Maui.Android
{
	public delegate void ContextChanged(Activity activity);

	public static class AndroidContext
	{
		public static event ContextChanged ContextChanged;

		private static Activity _context;
		private static float _displayScale;
		
		public static Activity CurrentContext
		{
			get => _context;
			set
			{
				_context = value;
				_displayScale = _context?.Resources.DisplayMetrics.Density ?? 1;
				ContextChanged?.Invoke(_context);
			}
		}

		public static float DisplayScale => _displayScale;
		
		public static AppCompatActivity AppCompatActivity => CurrentContext as AppCompatActivity;
	}
}
