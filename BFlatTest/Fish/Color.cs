using System;
using System.Runtime.InteropServices;

namespace Fish
{
	public static class Colors
	{
		public readonly static Color Red;
		public readonly static Color Green;
		public readonly static Color Blue;
		public readonly static Color White;
		public readonly static Color Black;
		public readonly static Color Yellow;
		public readonly static Color Cyan;
		public readonly static Color Magenta;

		static Colors()
		{
			Red = new Color(255, 0, 0);
			Green = new Color(0, 255, 0);
			Blue = new Color(0, 0, 255);
			White = new Color(255, 255, 255);
			Black = new Color(0, 0, 0);
			Yellow = new Color(255, 255, 0);
			Cyan = new Color(0, 255, 255);
			Magenta = new Color(255, 0, 255);
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
	public unsafe class Color
	{
		public byte R;
		public byte G;
		public byte B;
		public byte A;

		public Color(byte R, byte G, byte B, byte A = 255)
		{
			this.R = R;
			this.G = G;
			this.B = B;
			this.A = 255;
		}
	}
}
