using Comet.Styles.Material;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Samples
{
    public class MaterialStylePicker : View
    {
        public MaterialStylePicker()
        {
            this.Title("Material Style Picker");

        }

        List<ColorPalette> colorPalettes = new List<ColorPalette>
        {
            ColorPalette.Amber,
            ColorPalette.Blue,
            ColorPalette.Cyan,
            ColorPalette.DeepOrange,
            ColorPalette.DeepPurple,
            ColorPalette.Green,
            ColorPalette.Indigo,
            ColorPalette.LightBlue,
            ColorPalette.LightGreen,
            ColorPalette.Lime,
            ColorPalette.Orange,
            ColorPalette.Pink,
            ColorPalette.Purple,
            ColorPalette.Red,
            ColorPalette.Teal,
            ColorPalette.Yellow,
        };
        [Body]
        View body() => new ListView<ColorPalette>(colorPalettes)
        {
            ViewFor = (colorPalette) => new Text(colorPalette.Name).Background(colorPalette.P900).Color(colorPalette.PD900),
        }.OnSelectedNavigate((colorPallete) 
            => new MaterialSample().ApplyStyle(new MaterialStyle(colorPallete))
        );
    }
}
