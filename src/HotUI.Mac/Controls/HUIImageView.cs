using System;
using System.Diagnostics;
using CoreGraphics;
using AppKit;

namespace HotUI.Mac.Controls
{
    public class HUIImageView : NSImageView
    {
        private string _source;
        
        public HUIImageView(CGRect frame) : base(frame)
        {
            
        }

        public string Source
        {
            get => _source;
            set => UpdateSource(value);
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
                {
                    Console.WriteLine(Bounds);
                    Image = image;
                    SizeToFit();
                    Console.WriteLine(Bounds);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            if (Image != null)
            {
                var maxX = Math.Min(Image.Size.Width, size.Width);
                var maxY = Math.Max(Image.Size.Height, size.Height);

                var fx = size.Width / maxX;
                var fy = size.Height / maxY;

                var factor = Math.Min(fx, fy);
                factor = Math.Min(factor, 1);
                
                return new CGSize(Image.Size.Width * factor, Image.Size.Height * factor);
            }
            
            return base.SizeThatFits(size);
        }

        public override void SizeToFit()
        {
            if (Image != null)
            {
                Frame = new CGRect(new CGPoint(), Image.Size );
            }
            else
            {
                base.SizeToFit();
            }
            
        }
    }
}