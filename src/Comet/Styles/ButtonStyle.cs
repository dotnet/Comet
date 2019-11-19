using Comet.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Styles
{
    public class ButtonStyle
    {
        public Color TextColor { get; set; }

        public FontAttributes TextFont { get; set; }

        public Color BackgroundColor { get; set; }

        public Shape Border { get; set; }

        public Shadow Shadow { get; set; }

        public Thickness Padding { get; set; }

    }
}
