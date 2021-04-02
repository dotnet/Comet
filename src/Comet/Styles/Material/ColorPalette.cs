﻿using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Maui.Graphics;

namespace Comet.Styles.Material
{
	public partial class ColorPalette
	{
		public string Name { get; set; }
		public Color P50 { get; set; }
		public Color PD50 { get; set; }
		public Color P100 { get; set; }
		public Color PD100 { get; set; }
		public Color P200 { get; set; }
		public Color PD200 { get; set; }
		public Color P300 { get; set; }
		public Color PD300 { get; set; }
		public Color P400 { get; set; }
		public Color PD400 { get; set; }
		public Color P500 { get; set; }
		public Color PD500 { get; set; }
		public Color P600 { get; set; }
		public Color PD600 { get; set; }
		public Color P700 { get; set; }
		public Color PD700 { get; set; }
		public Color P800 { get; set; }
		public Color PD800 { get; set; }
		public Color P900 { get; set; }
		public Color PD900 { get; set; }
		public Color A100 { get; set; }
		public Color AD100 { get; set; }
		public Color A200 { get; set; }
		public Color AD200 { get; set; }
		public Color A400 { get; set; }
		public Color AD400 { get; set; }
		public Color A700 { get; set; }
		public Color AD700 { get; set; }

		public static ColorPalette Red { get; } = new ColorPalette
		{
			Name = "Red",
			P50 = new Color("#FFEBEE"),
			P100 = new Color("#FFCDD2"),
			P200 = new Color("#EF9A9A"),
			P300 = new Color("#E57373"),
			P400 = new Color("#EF5350"),
			P500 = new Color("#F44336"),
			P600 = new Color("#E53935"),
			P700 = new Color("#D32F2F"),
			P800 = new Color("#C62828"),
			P900 = new Color("#B71C1C"),
			A100 = new Color("#FF8A80"),
			A200 = new Color("#FF5252"),
			A400 = new Color("#FF1744"),
			A700 = new Color("#D50000"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.White,
			PD500 = Colors.White,
			PD600 = Colors.White,
			PD700 = Colors.White,
			PD800 = Colors.White,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.White,
			AD400 = Colors.White,
			AD700 = Colors.White,
		};

		public static ColorPalette Pink { get; } = new ColorPalette
		{
			Name = "Pink",
			P50 = new Color("#FCE4EC"),
			P100 = new Color("#F8BBD0"),
			P200 = new Color("#F48FB1"),
			P300 = new Color("#F06292"),
			P400 = new Color("#EC407A"),
			P500 = new Color("#E91E63"),
			P600 = new Color("#D81B60"),
			P700 = new Color("#C2185B"),
			P800 = new Color("#AD1457"),
			P900 = new Color("#880E4F"),
			A100 = new Color("#FF80AB"),
			A200 = new Color("#FF4081"),
			A400 = new Color("#F50057"),
			A700 = new Color("#C51162"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.White,
			PD500 = Colors.White,
			PD600 = Colors.White,
			PD700 = Colors.White,
			PD800 = Colors.White,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.White,
			AD400 = Colors.White,
			AD700 = Colors.White,
		};

		public static ColorPalette Purple { get; } = new ColorPalette
		{
			Name = "Purple",
			P50 = new Color("#F3E5F5"),
			P100 = new Color("#E1BEE7"),
			P200 = new Color("#CE93D8"),
			P300 = new Color("#BA68C8"),
			P400 = new Color("#AB47BC"),
			P500 = new Color("#9C27B0"),
			P600 = new Color("#8E24AA"),
			P700 = new Color("#7B1FA2"),
			P800 = new Color("#6A1B9A"),
			P900 = new Color("#4A148C"),
			A100 = new Color("#EA80FC"),
			A200 = new Color("#E040FB"),
			A400 = new Color("#D500F9"),
			A700 = new Color("#AA00FF"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.White,
			PD400 = Colors.White,
			PD500 = Colors.White,
			PD600 = Colors.White,
			PD700 = Colors.White,
			PD800 = Colors.White,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.White,
			AD400 = Colors.White,
			AD700 = Colors.White,
		};


		public static ColorPalette DeepPurple { get; } = new ColorPalette
		{
			Name = "Deep Purple",
			P50 = new Color("#EDE7F6"),
			P100 = new Color("#D1C4E9"),
			P200 = new Color("#B39DDB"),
			P300 = new Color("#9575CD"),
			P400 = new Color("#7E57C2"),
			P500 = new Color("#673AB7"),
			P600 = new Color("#5E35B1"),
			P700 = new Color("#512DA8"),
			P800 = new Color("#4527A0"),
			P900 = new Color("#311B92"),
			A100 = new Color("#B388FF"),
			A200 = new Color("#7C4DFF"),
			A400 = new Color("#651FFF"),
			A700 = new Color("#6200EA"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.White,
			PD400 = Colors.White,
			PD500 = Colors.White,
			PD600 = Colors.White,
			PD700 = Colors.White,
			PD800 = Colors.White,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.White,
			AD400 = Colors.White,
			AD700 = Colors.White,
		};
		public static ColorPalette Indigo { get; } = new ColorPalette
		{
			Name = "Indigo",
			P50 = new Color("#E8EAF6"),
			P100 = new Color("#C5CAE9"),
			P200 = new Color("#9FA8DA"),
			P300 = new Color("#7986CB"),
			P400 = new Color("#5C6BC0"),
			P500 = new Color("#3F51B5"),
			P600 = new Color("#3949AB"),
			P700 = new Color("#303F9F"),
			P800 = new Color("#283593"),
			P900 = new Color("#1A237E"),
			A100 = new Color("#8C9EFF"),
			A200 = new Color("#536DFE"),
			A400 = new Color("#3D5AFE"),
			A700 = new Color("#304FFE"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.White,
			PD400 = Colors.White,
			PD500 = Colors.White,
			PD600 = Colors.White,
			PD700 = Colors.White,
			PD800 = Colors.White,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.White,
			AD400 = Colors.White,
			AD700 = Colors.White,
		};
		public static ColorPalette Blue { get; } = new ColorPalette
		{
			Name = "Blue",
			P50 = new Color("#E3F2FD"),
			P100 = new Color("#BBDEFB"),
			P200 = new Color("#90CAF9"),
			P300 = new Color("#64B5F6"),
			P400 = new Color("#42A5F5"),
			P500 = new Color("#2196F3"),
			P600 = new Color("#1E88E5"),
			P700 = new Color("#1976D2"),
			P800 = new Color("#1565C0"),
			P900 = new Color("#0D47A1"),
			A100 = new Color("#82B1FF"),
			A200 = new Color("#448AFF"),
			A400 = new Color("#2979FF"),
			A700 = new Color("#2962FF"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.Black,
			PD500 = Colors.Black,
			PD600 = Colors.White,
			PD700 = Colors.White,
			PD800 = Colors.White,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.White,
			AD400 = Colors.White,
			AD700 = Colors.White,
		};
		public static ColorPalette LightBlue { get; } = new ColorPalette
		{
			Name = "Light Blue",
			P50 = new Color("#E1F5FE"),
			P100 = new Color("#B3E5FC"),
			P200 = new Color("#81D4FA"),
			P300 = new Color("#4FC3F7"),
			P400 = new Color("#29B6F6"),
			P500 = new Color("#03A9F4"),
			P600 = new Color("#039BE5"),
			P700 = new Color("#0288D1"),
			P800 = new Color("#0277BD"),
			P900 = new Color("#01579B"),
			A100 = new Color("#80D8FF"),
			A200 = new Color("#40C4FF"),
			A400 = new Color("#00B0FF"),
			A700 = new Color("#0091EA"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.Black,
			PD500 = Colors.Black,
			PD600 = Colors.Black,
			PD700 = Colors.White,
			PD800 = Colors.White,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.Black,
			AD400 = Colors.Black,
			AD700 = Colors.White,
		};
		public static ColorPalette Cyan { get; } = new ColorPalette
		{
			Name = "Cyan",
			P50 = new Color("#E0F7FA"),
			P100 = new Color("#B2EBF2"),
			P200 = new Color("#80DEEA"),
			P300 = new Color("#4DD0E1"),
			P400 = new Color("#26C6DA"),
			P500 = new Color("#00BCD4"),
			P600 = new Color("#00ACC1"),
			P700 = new Color("#0097A7"),
			P800 = new Color("#00838F"),
			P900 = new Color("#006064"),
			A100 = new Color("#84FFFF"),
			A200 = new Color("#18FFFF"),
			A400 = new Color("#00E5FF"),
			A700 = new Color("#00B8D4"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.Black,
			PD500 = Colors.Black,
			PD600 = Colors.Black,
			PD700 = Colors.White,
			PD800 = Colors.White,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.Black,
			AD400 = Colors.Black,
			AD700 = Colors.Black,
		};
		public static ColorPalette Teal { get; } = new ColorPalette
		{
			Name = "Teal",
			P50 = new Color("#E0F2F1"),
			P100 = new Color("#B2DFDB"),
			P200 = new Color("#80CBC4"),
			P300 = new Color("#4DB6AC"),
			P400 = new Color("#26A69A"),
			P500 = new Color("#009688"),
			P600 = new Color("#00897B"),
			P700 = new Color("#00796B"),
			P800 = new Color("#00695C"),
			P900 = new Color("#004D40"),
			A100 = new Color("#A7FFEB"),
			A200 = new Color("#64FFDA"),
			A400 = new Color("#1DE9B6"),
			A700 = new Color("#00BFA5"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.Black,
			PD500 = Colors.White,
			PD600 = Colors.White,
			PD700 = Colors.White,
			PD800 = Colors.White,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.Black,
			AD400 = Colors.Black,
			AD700 = Colors.Black,
		};
		public static ColorPalette Green { get; } = new ColorPalette
		{
			Name = "Green",
			P50 = new Color("#E8F5E9"),
			P100 = new Color("#C8E6C9"),
			P200 = new Color("#A5D6A7"),
			P300 = new Color("#81C784"),
			P400 = new Color("#66BB6A"),
			P500 = new Color("#4CAF50"),
			P600 = new Color("#43A047"),
			P700 = new Color("#388E3C"),
			P800 = new Color("#2E7D32"),
			P900 = new Color("#1B5E20"),
			A100 = new Color("#B9F6CA"),
			A200 = new Color("#69F0AE"),
			A400 = new Color("#00E676"),
			A700 = new Color("#00C853"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.Black,
			PD500 = Colors.Black,
			PD600 = Colors.White,
			PD700 = Colors.White,
			PD800 = Colors.White,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.Black,
			AD400 = Colors.Black,
			AD700 = Colors.Black,
		};
		public static ColorPalette LightGreen { get; } = new ColorPalette
		{
			Name = "Light Green",
			P50 = new Color("#F1F8E9"),
			P100 = new Color("#DCEDC8"),
			P200 = new Color("#C5E1A5"),
			P300 = new Color("#C5E1A5"),
			P400 = new Color("#9CCC65"),
			P500 = new Color("#8BC34A"),
			P600 = new Color("#7CB342"),
			P700 = new Color("#689F38"),
			P800 = new Color("#558B2F"),
			P900 = new Color("#33691E"),
			A100 = new Color("#CCFF90"),
			A200 = new Color("#B2FF59"),
			A400 = new Color("#76FF03"),
			A700 = new Color("#64DD17"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.Black,
			PD500 = Colors.Black,
			PD600 = Colors.Black,
			PD700 = Colors.Black,
			PD800 = Colors.White,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.Black,
			AD400 = Colors.Black,
			AD700 = Colors.Black,
		};

		public static ColorPalette Lime { get; } = new ColorPalette
		{
			Name = "Lime",
			P50 = new Color("#F9FBE7"),
			P100 = new Color("#F0F4C3"),
			P200 = new Color("#E6EE9C"),
			P300 = new Color("#DCE775"),
			P400 = new Color("#D4E157"),
			P500 = new Color("#CDDC39"),
			P600 = new Color("#C0CA33"),
			P700 = new Color("#AFB42B"),
			P800 = new Color("#9E9D24"),
			P900 = new Color("#827717"),
			A100 = new Color("#F4FF81"),
			A200 = new Color("#EEFF41"),
			A400 = new Color("#C6FF00"),
			A700 = new Color("#AEEA00"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.Black,
			PD500 = Colors.Black,
			PD600 = Colors.Black,
			PD700 = Colors.Black,
			PD800 = Colors.Black,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.Black,
			AD400 = Colors.Black,
			AD700 = Colors.Black,
		};

		public static ColorPalette Yellow { get; } = new ColorPalette
		{
			Name = "Yellow",
			P50 = new Color("#FFFDE7"),
			P100 = new Color("#FFF9C4"),
			P200 = new Color("#FFF59D"),
			P300 = new Color("#FFF176"),
			P400 = new Color("#FFEE58"),
			P500 = new Color("#FFEB3B"),
			P600 = new Color("#FDD835"),
			P700 = new Color("#FBC02D"),
			P800 = new Color("#F9A825"),
			P900 = new Color("#F57F17"),
			A100 = new Color("#FFFF8D"),
			A200 = new Color("#FFFF00"),
			A400 = new Color("#FFEA00"),
			A700 = new Color("#FFD600"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.Black,
			PD500 = Colors.Black,
			PD600 = Colors.Black,
			PD700 = Colors.Black,
			PD800 = Colors.Black,
			PD900 = Colors.Black,
			AD100 = Colors.Black,
			AD200 = Colors.Black,
			AD400 = Colors.Black,
			AD700 = Colors.Black,
		};

		public static ColorPalette Amber { get; } = new ColorPalette
		{
			Name = "Amber",
			P50 = new Color("#FFF8E1"),
			P100 = new Color("#FFECB3"),
			P200 = new Color("#FFE082"),
			P300 = new Color("#FFD54F"),
			P400 = new Color("#FFCA28"),
			P500 = new Color("#FFC107"),
			P600 = new Color("#FFB300"),
			P700 = new Color("#FFA000"),
			P800 = new Color("#FF8F00"),
			P900 = new Color("#FF6F00"),
			A100 = new Color("#FFE57F"),
			A200 = new Color("#FFD740"),
			A400 = new Color("#FFC400"),
			A700 = new Color("#FFAB00"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.Black,
			PD500 = Colors.Black,
			PD600 = Colors.Black,
			PD700 = Colors.Black,
			PD800 = Colors.Black,
			PD900 = Colors.Black,
			AD100 = Colors.Black,
			AD200 = Colors.Black,
			AD400 = Colors.Black,
			AD700 = Colors.Black,
		};

		public static ColorPalette Orange { get; } = new ColorPalette
		{
			Name = "Orange",
			P50 = new Color("#FFF3E0"),
			P100 = new Color("#FFE0B2"),
			P200 = new Color("#FFCC80"),
			P300 = new Color("#FFB74D"),
			P400 = new Color("#FFA726"),
			P500 = new Color("#FF9800"),
			P600 = new Color("#FB8C00"),
			P700 = new Color("#F57C00"),
			P800 = new Color("#EF6C00"),
			P900 = new Color("#E65100"),
			A100 = new Color("#FFD180"),
			A200 = new Color("#FFAB40"),
			A400 = new Color("#FF9100"),
			A700 = new Color("#FF6D00"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.Black,
			PD500 = Colors.Black,
			PD600 = Colors.Black,
			PD700 = Colors.Black,
			PD800 = Colors.Black,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.Black,
			AD400 = Colors.Black,
			AD700 = Colors.Black,
		};

		public static ColorPalette DeepOrange { get; } = new ColorPalette
		{
			Name = "Deep Orange",
			P50 = new Color("#FBE9E7"),
			P100 = new Color("#FFCCBC"),
			P200 = new Color("#FFAB91"),
			P300 = new Color("#FF8A65"),
			P400 = new Color("#FF7043"),
			P500 = new Color("#FF5722"),
			P600 = new Color("#F4511E"),
			P700 = new Color("#E64A19"),
			P800 = new Color("#D84315"),
			P900 = new Color("#BF360C"),
			A100 = new Color("#FF9E80"),
			A200 = new Color("#FF6E40"),
			A400 = new Color("#FF3D00"),
			A700 = new Color("#DD2C00"),
			PD50 = Colors.Black,
			PD100 = Colors.Black,
			PD200 = Colors.Black,
			PD300 = Colors.Black,
			PD400 = Colors.Black,
			PD500 = Colors.Black,
			PD600 = Colors.White,
			PD700 = Colors.White,
			PD800 = Colors.White,
			PD900 = Colors.White,
			AD100 = Colors.Black,
			AD200 = Colors.Black,
			AD400 = Colors.White,
			AD700 = Colors.White,
		};

	}
}
