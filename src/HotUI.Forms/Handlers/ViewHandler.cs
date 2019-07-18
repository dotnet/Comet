using System.Diagnostics;
using FView = Xamarin.Forms.View;

namespace HotUI.Forms.Handlers
{
    public class ViewHandler : AbstractHandler<View,FView>
    {
        public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>()
        {

        };

        public ViewHandler() : base(Mapper)
        {
            
        }
        
        protected override FView CreateView()
        {
            var viewHandler = VirtualView?.GetOrCreateViewHandler();
            if (viewHandler?.GetType() == typeof(ViewHandler) && VirtualView.Body == null)
            {
                // this is recursive.
                Debug.WriteLine($"There is no ViewHandler for {VirtualView.GetType()}");
                return null;
            }

            return viewHandler?.View ?? new Xamarin.Forms.ContentView();
        }
    }
}
