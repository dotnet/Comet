using System;
using HotUI;
using Xamarin.Forms;
using FWebView = Xamarin.Forms.WebView;
using HWebView = HotUI.WebView;
using HView = HotUI.View;
namespace HotUI.Forms
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
