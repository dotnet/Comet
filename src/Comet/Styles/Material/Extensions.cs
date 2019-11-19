using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Styles.Material
{
    public static class Extensions
    {
        public static T StyleAsOutlined<T>(this T button) where T : Button
        {
            button.StyleId = MaterialStyle.OutlinedButtonStyleId;
            return button;
        }
        public static T StyleAsContained<T>(this T button) where T : Button
        {
            button.StyleId = MaterialStyle.ContainedButtonStyleId;
            return button;
        }
        public static T StyleAsText<T>(this T button) where T : Button
        {
            button.StyleId = MaterialStyle.TextButtonStyleId;
            return button;
        }
    }
}
