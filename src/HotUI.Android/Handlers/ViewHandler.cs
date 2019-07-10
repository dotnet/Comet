using System;
using AView = Android.Views.View;
namespace HotUI.Android 
{
	public class ViewHandler : AndroidViewHandler
	{
		public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>()
		{
			[nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty,
			[nameof(EnvironmentKeys.View.Shadow)] = MapShadowProperty,
			[nameof(EnvironmentKeys.View.ClipShape)] = MapClipShapeProperty
		};

		private View _view;
		private AView _body;

		public Action ViewChanged { get; set; }

		public AView View => _body;
		
		public object NativeView => View;

		public bool HasContainer
		{
			get => false;
			set { }
		}

		public void Remove (View view)
		{
			// todo: implement this
		}

		public void SetView (View view)
		{
			_view = view;
			SetBody();
			Mapper.UpdateProperties(this, _view);
			ViewChanged?.Invoke();
		}

		private void SetBody()
		{
			_body = _view.ToView();
		}

		public void UpdateValue (string property, object value)
		{
			Mapper.UpdateProperty(this, _view, property);
		}
		
		public static void MapBackgroundColorProperty(IViewHandler handler, View virtualView)
        {
            var nativeView = (AView) handler.NativeView;
            var color = virtualView.GetBackgroundColor();
            if (color != null)
                nativeView.SetBackgroundColor(color.ToColor());
        }
        
        public static void MapShadowProperty(IViewHandler handler, View virtualView)
        {
            Console.WriteLine("Shadows not yet supported on Android");
        }

        public static void MapClipShapeProperty(IViewHandler handler, View virtualView)
        {
	        Console.WriteLine("ClipShape not yet supported on Android");
        }
	}
}
