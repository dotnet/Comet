using System.Globalization;

namespace Comet
{
	public partial class Color
	{
		public readonly float R;
		public readonly float G;
		public readonly float B;
		public readonly float A = 1;

		public Color(float red, float green, float blue)
		{
			R = red;
			G = green;
			B = blue;
		}

		public Color(float red, float green, float blue, float alpha)
		{
			R = red;
			G = green;
			B = blue;
			A = alpha;
		}

		public Color(string colorAsHex)
		{
			//Remove # if present
			if (colorAsHex.IndexOf('#') != -1)
				colorAsHex = colorAsHex.Replace("#", "");

			int red = 0;
			int green = 0;
			int blue = 0;
			int alpha = 255;

			if (colorAsHex.Length == 6)
			{
				//#RRGGBB
				red = int.Parse(colorAsHex.Substring(0, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
				green = int.Parse(colorAsHex.Substring(2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
				blue = int.Parse(colorAsHex.Substring(4, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
			}
			else if (colorAsHex.Length == 3)
			{
				//#RGB
				red = int.Parse($"{colorAsHex[0]}{colorAsHex[0]}", NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
				green = int.Parse($"{colorAsHex[1]}{colorAsHex[1]}", NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
				blue = int.Parse($"{colorAsHex[2]}{colorAsHex[2]}", NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
			}
			else if (colorAsHex.Length == 4)
			{
				//#RGBA
				red = int.Parse($"{colorAsHex[0]}{colorAsHex[0]}", NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
				green = int.Parse($"{colorAsHex[1]}{colorAsHex[1]}", NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
				blue = int.Parse($"{colorAsHex[2]}{colorAsHex[2]}", NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
				alpha = int.Parse($"{colorAsHex[3]}{colorAsHex[3]}", NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
			}
			else if (colorAsHex.Length == 8)
			{
				//#RRGGBBAA
				red = int.Parse(colorAsHex.Substring(0, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
				green = int.Parse(colorAsHex.Substring(2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
				blue = int.Parse(colorAsHex.Substring(4, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
				alpha = int.Parse(colorAsHex.Substring(6, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
			}

			R = red / 255f;
			G = green / 255f;
			B = blue / 255f;
			A = alpha / 255f;
		}

		public static Color FromBytes(byte red, byte green, byte blue) => Color.FromBytes(red, green, blue, 255);
		public static Color FromBytes(byte red, byte green, byte blue, byte alpha)
			=> new Color(red / 255f, green / 255f, blue / 255f, alpha / 255f);

		public override int GetHashCode()
		{
			return ((int)R ^ (int)B) ^ ((int)G ^ (int)A);
		}

		public string ToHexString()
		{
			return "#" + ToHexString(R) + ToHexString(G) + ToHexString(B);
		}

		public string ToHexStringIncludingAlpha()
		{
			if (A < 1)
				return ToHexString() + ToHexString(A);

			return ToHexString();
		}

		public static string ToHexString(float r, float g, float b)
		{
			return "#" + ToHexString(r) + ToHexString(g) + ToHexString(b);
		}

		private static string ToHexString(float value)
		{
			var intValue = (int)(255f * value);
			var stringValue = intValue.ToString("X");
			if (stringValue.Length == 1)
				return "0" + stringValue;

			return stringValue;
		}
	}
}
