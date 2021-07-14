using System;
using Microsoft.Maui;

namespace Comet
{
	public static class LineBreakModeExtensions
	{
		public static LineBreakMode GetLineBreakMode<T>(this T view, LineBreakMode defaultMode) where T : View
		{
			var mode = view.GetEnvironment<LineBreakMode?>(EnvironmentKeys.LineBreakMode.Mode);
			return mode ?? defaultMode;
		}

		public static T LineBreakMode<T>(this T view, Binding<LineBreakMode> mode) where T : View =>
			view.SetEnvironment(EnvironmentKeys.LineBreakMode.Mode, mode);
		public static T LineBreakMode<T>(this T view, Func<LineBreakMode> mode) where T : View =>
			view.LineBreakMode((Binding<LineBreakMode>)mode);
	}
}
