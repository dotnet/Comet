using System;
using System.Maui.Blazor.Components;

namespace System.Maui.Blazor.Handlers
{
	internal class ProgressBarHandler : BlazorHandler<ProgressBar, BProgressBar>
	{
		public static readonly PropertyMapper<ProgressBar> Mapper = new PropertyMapper<ProgressBar>
		{
			{ nameof(ProgressBar.Value), MapValueProperty },
		};

		public ProgressBarHandler()
			: base(Mapper)
		{
		}

		public static void MapValueProperty(IViewHandler viewHandler, ProgressBar virtualView)
		{
			var nativeView = (BProgressBar)viewHandler.NativeView;

			nativeView.Value = virtualView.Value;
		}
	}
}
