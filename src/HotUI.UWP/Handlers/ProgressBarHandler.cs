// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

using Windows.UI.Xaml.Controls;

using UWPProgressBar = Windows.UI.Xaml.Controls.ProgressBar;

namespace Comet.UWP.Handlers
{
    public class ProgressBarHandler : AbstractHandler<ProgressBar, UWPProgressBar>
    {
        public static readonly PropertyMapper<ProgressBar> Mapper = new PropertyMapper<ProgressBar>()
        {
            [nameof(ProgressBar.Value)] = MapValueProperty,
            [nameof(ProgressBar.IsIndeterminate)] = MapIsIndeterminateProperty,
        };

        public ProgressBarHandler() : base(Mapper)
        {
        }

        protected override UWPProgressBar CreateView() => new UWPProgressBar();

        public static void MapValueProperty(IViewHandler viewHandler, ProgressBar virtualView)
        {
            var nativeView = (UWPProgressBar)viewHandler.NativeView;
            nativeView.Value = virtualView.Value;
        }

        public static void MapIsIndeterminateProperty(IViewHandler viewHandler, ProgressBar virtualView)
        {
            var nativeView = (UWPProgressBar)viewHandler.NativeView;
            nativeView.IsIndeterminate = virtualView.IsIndeterminate;
        }
    }
}