using System;
using Comet.Styles;

namespace Comet.Skia
{
	public class SkiaStyle : Style
	{
		public SkiaStyle()
		{

		}
		public override void Apply(ContextualObject view = null)
		{
			SetDefaultControlSizingForLayouts();
		}
		void SetDefaultControlSizingForLayouts()
		{
			void setSizing(Type control, Type container, string keyType, Sizing sizing)
			{
				var key = $"{container.Name}.{keyType}";
				SetEnvironmentValue(null, control, key, sizing);
			}
			setSizing(typeof(SKText), typeof(VStack), EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fill);
			setSizing(typeof(SKTextField), typeof(VStack), EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fill);
			//setSizing(typeof(SecureField), typeof(VStack), EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fill);
			setSizing(typeof(SKProgressBar), typeof(VStack), EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fill);
			setSizing(typeof(SKSlider), typeof(VStack), EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fill);
			//setSizing(typeof(ScrollView), typeof(HStack), EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fill);
			//setSizing(typeof(ScrollView), typeof(HStack), EnvironmentKeys.Layout.VerticalSizing, Sizing.Fill);
			//setSizing(typeof(ScrollView), typeof(VStack), EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fill);
			//setSizing(typeof(ScrollView), typeof(VStack), EnvironmentKeys.Layout.VerticalSizing, Sizing.Fill);
			//setSizing(typeof(WebView), typeof(HStack), EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fill);
			//setSizing(typeof(WebView), typeof(HStack), EnvironmentKeys.Layout.VerticalSizing, Sizing.Fill);
			//setSizing(typeof(WebView), typeof(VStack), EnvironmentKeys.Layout.VerticalSizing, Sizing.Fill);
			//setSizing(typeof(WebView), typeof(VStack), EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fill);
			//setSizing(typeof(ListView), typeof(HStack), EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fill);
			//setSizing(typeof(ListView), typeof(HStack), EnvironmentKeys.Layout.VerticalSizing, Sizing.Fill);
			//setSizing(typeof(ListView), typeof(VStack), EnvironmentKeys.Layout.VerticalSizing, Sizing.Fill);
			//setSizing(typeof(ListView), typeof(VStack), EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fill);
		}

		void SetEnvironmentValue(ContextualObject view, Type type, string key, object value)
		{
			if (view != null)
				view.SetEnvironment(type, key, value);
			else
				View.SetGlobalEnvironment(type, key, value);
		}
	}
}
