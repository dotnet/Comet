using System;
using HotUI.Blazor.Components;

namespace HotUI.Blazor.Handlers
{
    internal class ProgressBarHandler : BlazorHandler<ProgressBar, BProgressBar>
    {
        public static readonly PropertyMapper<ProgressBar> Mapper = new PropertyMapper<ProgressBar>
        {
            { nameof(ProgressBar.Value), MapValueProperty },
            { nameof(ProgressBar.IsIndeterminate), MapIsIndeterminateProperty }
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


        private static void MapIsIndeterminateProperty(IViewHandler viewHandler, ProgressBar virtualView)
        {
            var nativeView = (BProgressBar)viewHandler.NativeView;

            nativeView.IsIndeterminate = virtualView.IsIndeterminate;
        }
    }
}
