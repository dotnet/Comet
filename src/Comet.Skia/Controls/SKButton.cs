using System;
namespace Comet.Skia
{
    public class SKButton : Button
    {
        public SKButton(
            Binding<string> text = null,
            Action action = null) : base(text, action)
        {

        }

        public SKButton(
            Func<string> text,
            Action action = null) : base((Binding<string>)text, action)
        {

        }
    }
}
