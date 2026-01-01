using System;
using System.Runtime.InteropServices;

namespace System.Numerics
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe class Vector2
	{
		public float X;
		public float Y;

		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe class Vector3
	{
		public float X;
		public float Y;
		public float Z;

		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}
}
