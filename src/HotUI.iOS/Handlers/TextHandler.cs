using System;
using System.Collections.Generic;
using UIKit;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
{
    public class TextHandler : UILabel, IUIView
    {
        public static readonly PropertyMapper<Text, UIView, TextHandler> Mapper = new PropertyMapper<Text, UIView, TextHandler>(ViewHandler.Mapper)
        {
            [nameof(HotUI.Text.Value)] = MapValueProperty,
            [EnvironmentKeys.Fonts.Font] = MapFontProperty,
            [EnvironmentKeys.Colors.Color] = MapColorProperty,
        };

        private static Font DefaultFont;
        private static Color DefaultColor;
        private static Color DefaultBackgroundColor;

        private Text _text;

		public TextHandler ()
		{
			// todo: answer the question of whether or not these should be default or not.
			if (DefaultColor == null)
			{
				DefaultFont = Font.ToFont();
                DefaultColor = TextColor.ToColor();
                DefaultBackgroundColor = BackgroundColor.ToColor();
            }
        }

        public UIView View => this;

        public void Remove(View view)
        {
            _text = null;
        }

        public void SetView(View view)
        {
            _text= view as Text;
            Mapper.UpdateProperties(this, _text);
        }

        public void UpdateValue(string property, object value)
        {
			if (Mapper.UpdateProperty (this, _text, property))
				return;
        }
        
        public static bool MapValueProperty(TextHandler nativeView, Text virtualView)
        {
            nativeView.Text = virtualView.Value;
            nativeView.SizeToFit();
            return true;
        }

		public static bool MapFontProperty (TextHandler nativeView, Text virtualView)
		{
			var font = virtualView.GetFont(DefaultFont);
			nativeView.Font = font.ToUIFont();
			nativeView.SizeToFit ();
			return true;
		}

        public static bool MapColorProperty(TextHandler nativeView, Text virtualView)
        {
            var color = virtualView.GetColor(DefaultColor);
            var nativeColor = nativeView.TextColor.ToColor();
            if (!color.Equals(nativeColor))
                nativeView.TextColor = color.ToUIColor();

            return true;
        }
    }
}