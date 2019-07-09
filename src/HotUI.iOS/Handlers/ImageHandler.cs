using CoreGraphics;
using HotUI.iOS.Controls;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
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

        protected override HUIImageView CreateView()
        {
            return new HUIImageView(new CGRect(0, 0, 44, 44));
        }

        public static bool MapSourceProperty(IViewHandler viewHandler, Image virtualView)
        {
            var nativeView = (HUIImageView) viewHandler.NativeView;
            nativeView.Source = virtualView.Source;
            return true;
        }
    }
}