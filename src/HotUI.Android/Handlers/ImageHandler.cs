using Android.Content;
using HotUI.Android.Controls;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class ImageHandler : AbstractHandler<Image, HUIImageView>
    {
        public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>(ViewHandler.Mapper)
        {
            [nameof(Image.Source)] = MapSourceProperty
        };
        
        public ImageHandler() : base(Mapper)
        {
        }

        protected override HUIImageView CreateView(Context context)
        {
            return new HUIImageView(context);
        }

        public static void MapSourceProperty(IViewHandler viewHandler, Image virtualView)
        {
            var nativeView = (HUIImageView) viewHandler.NativeView;
            nativeView.Source = virtualView.Source;
        }
    }
}