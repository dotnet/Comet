using System;
using System.Collections.Generic;
using UIKit;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
{
    public class TextHandler : UILabel, IUIView
    {
        private static readonly PropertyMapper<Text, TextHandler> Mapper = new PropertyMapper<Text, TextHandler>()
        {
            [nameof(HotUI.Text.Value)] = MapValueProperty,
			[EnvironmentKeys.Fonts.FontSize] = MapFontSizeProperty
		};

		int DefaultFontSize;
		private Text _text;

		public TextHandler ()
		{
			DefaultFontSize = (int)this.Font.PointSize;
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

		public static bool MapFontSizeProperty (TextHandler nativeView, Text virtualView)
		{
			var fontSize = (nfloat)virtualView.GetFontSize (nativeView.DefaultFontSize);
			if (fontSize != nativeView.Font.PointSize) {
				nativeView.Font = nativeView.Font.WithSize (fontSize);
				nativeView.SizeToFit ();
			}
			return true;
		}

	}
}