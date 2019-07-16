using HotUI;
using Xamarin.Forms;
using FImage = Xamarin.Forms.Image;
using HImage = HotUI.Image;
using HView = HotUI.View;
namespace HotUI.Forms
{
    public class ImageHandler : AbstractHandler<HImage, FImage>
    {
        public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>(ViewHandler.Mapper)
        {
            [nameof(Image.Source)] = MapSourceProperty
        };

        public ImageHandler() : base(Mapper)
        {
        }
        protected override FImage CreateView()
        {
            throw new System.NotImplementedException();
        }
        public static void MapSourceProperty(IViewHandler viewHandler, Image virtualView)
        {
            var nativeView = (FImage)viewHandler.NativeView;
            nativeView.Source = virtualView.Source?.ToImageSource();
        }
    }
}
