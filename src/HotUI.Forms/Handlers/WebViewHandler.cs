using FWebView = Xamarin.Forms.WebView;
using HWebView = HotUI.WebView;

// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.Forms.Handlers
{
    public class WebViewHandler : AbstractHandler<HWebView, FWebView>
    {
        public static readonly PropertyMapper<HWebView> Mapper = new PropertyMapper<HWebView>(ViewHandler.Mapper)
        {
            [nameof(WebView.Source)] = MapSourceProperty,
            [nameof(WebView.Html)] = MapHtmlProperty
        };

        public WebViewHandler() : base(Mapper)
        {
        }

        protected override FWebView CreateView() => new FWebView();
        
        protected override void DisposeView(FWebView nativeView)
        {
            
        }

        public static void MapSourceProperty(IViewHandler viewHandler, WebView virtualView)
        {
            var webView = viewHandler.NativeView as FWebView;
            webView.Source = virtualView.Source?.ToWebViewSource();
        }
        public static void MapHtmlProperty(IViewHandler viewHandler, WebView virtualView)
        {
            var webView = viewHandler.NativeView as FWebView;
            webView.Source = virtualView.Html == null ? null : new Xamarin.Forms.HtmlWebViewSource { Html = virtualView.Html };
        }

    }
}
