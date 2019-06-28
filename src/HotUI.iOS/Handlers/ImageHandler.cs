using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CoreGraphics;
using FFImageLoading;
using UIKit;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
{
    public class ImageHandler : UIImageView, IUIView
    {
        private static readonly PropertyMapper<Image, ImageHandler> Mapper = new PropertyMapper<Image, ImageHandler>(new Dictionary<string, Func<ImageHandler, Image, bool>>
        {
            [nameof(Button.Text)] = MapSourceProperty
        });

        private Image _image;
        private string _source;

        public ImageHandler()
        {
            Frame = new CGRect(0, 0, 56, 56);
        }

        public UIView View => this;

        public void Remove(View view)
        {
            _image = null;
            _source = null;
        }

        public void SetView(View view)
        {
            _image = view as Image;
            Mapper.UpdateProperties(this, _image);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _image, property);
        }

        public static bool MapSourceProperty(ImageHandler nativeView, Image virtualView)
        {
            nativeView.UpdateSource(virtualView.Source);
            return true;
        }
        
        private async void UpdateSource(string source)
        {
            if (source == _source)
                return;
            
            _source = source;
            try
            {
                var image = await source.LoadImage();
                if (source == _source)
                    Image = image;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}