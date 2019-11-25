using System;
namespace Comet.Skia
{
    public class SKText : Text
    {
        public SKText(
           Binding<string> value = null) : base(value)
        {
            
        }
        public SKText(
            Func<string> value) : this((Binding<string>)value)
        {

        }

    }
}
