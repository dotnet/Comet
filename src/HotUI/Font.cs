using System;

namespace HotUI
{
    /// <summary>
    /// An environment-dependent font.
    /// </summary>
    public partial class Font
    {
        private FontAttributes _attributes;

        private Font(Font template)
        {
            _attributes = template._attributes;
        }
        
        private Font(
            string name, 
            float size, 
            bool italic = false, 
            Weight weight = HotUI.Weight.Regular,
            bool smallcaps = false)
        {
            _attributes.Name = name;
            _attributes.Size = size;
            _attributes.Weight = weight;
            _attributes.Italic = italic;
            _attributes.Smallcaps = smallcaps;
        }

        public FontAttributes Attributes => _attributes;
        
        /// <summary>
        /// Adds bold styling to the font.
        /// </summary>
        /// <returns></returns>
        public Font Bold()
        {
            var font = new Font(this);
            font._attributes.Weight = HotUI.Weight.Bold;
            return font;
        }

        /// <summary>
        /// Adds italics to the font.
        /// </summary>
        /// <returns></returns>
        public Font Italic()
        {
            var font = new Font(this);
            font._attributes.Italic = true;
            return font;
        }

        /// <summary>
        /// Adjusts the font to use monospace digits.
        /// </summary>
        /// <returns></returns>
        public Font MonospacedDigit()
        {
            var font = new Font(this);
            font._attributes.Name = Device.FontService.MonospacedFontName;
            return font;
        }

        /// <summary>
        /// Adjusts the font to use small capitals.
        /// </summary>
        /// <returns></returns>
        public Font SmallCaps()
        {
            var font = new Font(this);
            font._attributes.Smallcaps = true;
            return font;
        }

        /// <summary>
        /// Sets the weight of the font.
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public Font Weight(Weight weight)
        {
            var font = new Font(this);
            font._attributes.Weight = weight;
            return font;
        }

        /// <summary>
        /// Gets a system font with the given style and design.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="design"></param>
        /// <returns></returns>
        public static Font System(TextStyle style, Design design = Design.Default)
        {
            var fontName = design.FontName();
            var size = 12f;
            var italic = false;
            var smallcaps = false;
            var weight = HotUI.Weight.Regular;
            
            // todo: figure out what these values should be for real by playing around
            // with SwiftUI or figure out if iOS 13 allows me to get these values.
            switch (style)
            {
                case TextStyle.Body:
                    break;
                case TextStyle.Callout:
                    break;
                case TextStyle.Caption:
                    break;
                case TextStyle.Footnote:
                    italic = true;
                    break;
                case TextStyle.Headline:
                    break;
                case TextStyle.LargeTitle:
                    weight = HotUI.Weight.Bold;
                    size = 24f;
                    break;
                case TextStyle.Subheadline:
                    break;
                case TextStyle.Title:
                    weight = HotUI.Weight.Bold;
                    size = 17f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
            
            return new Font(fontName, size, italic, weight, smallcaps);
        }

        /// <summary>
        /// Gets a system font with the given size and design.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="design"></param>
        /// <returns></returns>
        public static Font System(float size, Design design = Design.Default)
        {
            return new Font(design.FontName(), size);
        }

        /// <summary>
        /// Gets a custom font with the given name and size.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Font Custom(string name, float size)
        {
            throw new NotImplementedException();
        }
    }
}
