using System;

namespace System.Numerics
{
	public unsafe class Vector3
	{
		public static readonly Vector3 Zero = new Vector3(0, 0, 0);

		public float X;
		public float Y;
		public float Z;

		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static Vector3 operator -(Vector3 a, Vector3 b)
		{
			return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		public static Vector3 operator +(Vector3 a, Vector3 b)
		{
			return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		public static Vector3 operator *(Vector3 a, float b)
		{
			return new Vector3(a.X * b, a.Y * b, a.Z * b);
		}

		public static Vector3 operator /(Vector3 a, float b)
		{
			return new Vector3(a.X / b, a.Y / b, a.Z / b);
		}

		public static Vector3 operator *(Vector3 a, Vector3 b)
		{
			return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
		}

		public static Vector3 Cross(Vector3 a, Vector3 b)
		{
			return new Vector3(
				a.Y * b.Z - a.Z * b.Y,
				a.Z * b.X - a.X * b.Z,
				a.X * b.Y - a.Y * b.X
			);
		}

		public static float Dot(Vector3 a, Vector3 b)
		{
			return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
		}

		public float Length()
		{
			return MathF.Sqrt(X * X + Y * Y + Z * Z);
		}

		public static Vector3 Normalize(Vector3 v)
		{
			float length = v.Length();
			return new Vector3(v.X / length, v.Y / length, v.Z / length);
		}
	}
}
