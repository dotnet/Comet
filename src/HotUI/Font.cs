using System;
namespace HotUI
{
    /// <summary>
    /// An environment-dependent font.
    /// </summary>
    public partial class Font
    {
        private Font()
        {
        }

        /// <summary>
        /// Adds bold styling to the font.
        /// </summary>
        /// <returns></returns>
        public Font Bold()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds italics to the font.
        /// </summary>
        /// <returns></returns>
        public Font Italic()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adjusts the font to use monospace digits.
        /// </summary>
        /// <returns></returns>
        public Font MonospacedDigit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adjusts the font to use small capitals.
        /// </summary>
        /// <returns></returns>
        public Font SmallCaps()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the weight of the font.
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public Font Weight(Weight weight)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a system font with the given style and design.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="design"></param>
        /// <returns></returns>
        public static Font System(TextStyle style, Design design = Design.Default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a system font with the given size and design.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="design"></param>
        /// <returns></returns>
        public static Font System(float size, Design design = Design.Default)
        {
            throw new NotImplementedException();
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
