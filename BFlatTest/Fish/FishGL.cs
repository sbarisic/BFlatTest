using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fish
{
	public struct Tri
	{
		public Vector3 A, B, C;
		public Vector2 A_UV, B_UV, C_UV;

		public static Tri operator +(Tri A, Vector3 B)
		{
			A.A += B;
			A.B += B;
			A.C += B;
			return A;
		}

		public static Tri operator *(Tri A, Vector3 B)
		{
			A.A *= B;
			A.B *= B;
			A.C *= B;
			return A;
		}
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	public unsafe struct FGLColor
	{
		public static readonly FGLColor White = new FGLColor(255, 255, 255);
		public static readonly FGLColor Red = new FGLColor(255, 0, 0);
		public static readonly FGLColor Black = new FGLColor(0, 0, 0);
		public static readonly FGLColor DepthZero = new FGLColor(0.0f);

		[FieldOffset(0)]
		public byte B;

		[FieldOffset(1)]
		public byte G;

		[FieldOffset(2)]
		public byte R;

		[FieldOffset(3)]
		public byte A;

		[FieldOffset(0)]
		public int Int;

		[FieldOffset(0)]
		public float Float;

		public FGLColor(byte R, byte G, byte B, byte A)
		{
			Float = 0;
			Int = 0;

			this.R = R;
			this.G = G;
			this.B = B;
			this.A = A;
		}

		public FGLColor(byte R, byte G, byte B) : this(R, G, B, 255)
		{
		}

		public FGLColor(float Float) : this(0, 0, 0, 0)
		{
			this.Float = Float;
		}

		public static void ScaleColor(ref FGLColor Clr, float Scale)
		{
			Clr.R = (byte)(Clr.R * Scale);
			Clr.G = (byte)(Clr.G * Scale);
			Clr.B = (byte)(Clr.B * Scale);
		}

		public static void ScaleColor(ref FGLColor Clr, ref FGLColor Scale)
		{
			Clr.R = (byte)(Clr.R * (Scale.R / 255.0f));
			Clr.G = (byte)(Clr.G * (Scale.G / 255.0f));
			Clr.B = (byte)(Clr.B * (Scale.B / 255.0f));
		}

		public static void Blend(ref FGLColor Dest, ref FGLColor Src)
		{
			Dest.R = (byte)(((Dest.R * (255 - Src.A)) + (Src.R * Src.A)) / 255);
			Dest.G = (byte)(((Dest.G * (255 - Src.A)) + (Src.G * Src.A)) / 255);
			Dest.B = (byte)(((Dest.B * (255 - Src.A)) + (Src.B * Src.A)) / 255);
			Dest.A = (byte)(((Dest.A * (255 - Src.A)) + (Src.A * Src.A)) / 255);
		}

		public static implicit operator FGLColor(Color Clr)
		{
			return new FGLColor(Clr.R, Clr.G, Clr.B, Clr.A);
		}
	}

	public unsafe struct FGLFramebuffer
	{
		public int Width, Height;
		public int ColorLen, Len;

		public FGLColor* DataPtr;

		//GCHandle DataHandle;

		public FGLFramebuffer(int W, int H)
		{
			Width = W;
			Height = H;
			ColorLen = W * H;
			Len = ColorLen * sizeof(FGLColor);

			//Data = new byte[Len];
			//DataHandle = new GCHandle();

			byte[] barr = new byte[Len];
			byte* bb = (byte*)Unsafe.AsPointer(ref barr[0]);
			DataPtr = (FGLColor*)bb;

			//Pin();
		}

		/*public void Pin()
		{
			//DataHandle = GCHandle.Alloc(Data, GCHandleType.Pinned);
			//DataPtr = (FGLColor*)DataHandle.AddrOfPinnedObject();

			Console.WriteLine("Pinning!");
			DataPtr = (FGLColor*)Unsafe.AsPointer<byte[]>(ref Data);
		}

		public void UnPin()
		{
			DataPtr = null;
			//DataHandle.Free();
		}*/

		public void Get(float U, float V, out FGLColor Clr)
		{
			/*U = Helpers.Clamp(U, 0, 1);
			V = Helpers.Clamp(V, 0, 1);*/

			Clr = DataPtr[(int)(V * Height) * Width + (int)(U * Width)];
		}

		/*public static FGLFramebuffer FromFile(string Pth)
		{
			FGLFramebuffer FB;

			using (Bitmap Bmp = new Bitmap(Image.FromFile(Pth)))
			{
				Bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
				FB = new FGLFramebuffer(Bmp.Width, Bmp.Height);

				for (int Y = 0; Y < Bmp.Height; Y++)
					for (int X = 0; X < Bmp.Width; X++)
						FB.DataPtr[Y * FB.Width + X] = Bmp.GetPixel(X, Y);
			}

			return FB;
		}*/
	}

	static class Helpers
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Min(float A, float B, float C)
		{
			return Math.Min(A, Math.Min(B, C));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Max(float A, float B, float C)
		{
			return Math.Max(A, Math.Max(B, C));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Clamp(float V, float Min, float Max)
		{
			if (V < Min)
				return Min;
			if (V > Max)
				return Max;
			return V;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BoundingBox(ref Tri T, out Vector3 Minimum, out Vector3 Maximum)
		{
			Minimum = new Vector3(Min(T.A.X, T.B.X, T.C.X), Min(T.A.Y, T.B.Y, T.C.Y), Min(T.A.Z, T.B.Z, T.C.Z));
			Maximum = new Vector3(Max(T.A.X, T.B.X, T.C.X), Max(T.A.Y, T.B.Y, T.C.Y), Max(T.A.Z, T.B.Z, T.C.Z));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Barycentric(ref Tri T, int PX, int PY, ref Vector3 Val)
		{
			Vector3 U = Vector3.Cross(new Vector3(T.C.X - T.A.X, T.B.X - T.A.X, T.A.X - PX),
				new Vector3(T.C.Y - T.A.Y, T.B.Y - T.A.Y, T.A.Y - PY));

			if (Math.Abs(U.Z) < 1)
			{
				Val.X = -1;
				return;
			}

			Val.X = 1.0f - (U.X + U.Y) / U.Z;
			Val.Y = U.Y / U.Z;
			Val.Z = U.X / U.Z;
		}
	}

	public abstract class FGLShader
	{
		public abstract void Vertex(ref Vector3 Vert);
		public abstract void Pixel(ref FGLColor OutColor, float U, float V, int ScrX, int ScrY, float Depth, ref bool Discard);
	}

	public static unsafe class FishGL
	{
		public static FGLFramebuffer ColorBuffer;
		public static FGLFramebuffer DepthBuffer;
		public static FGLFramebuffer TEX0Buffer;

		public static bool EnableTexturing = false;
		public static bool EnableShading = false;
		public static bool EnableBackfaceCulling = false;
		public static bool EnableDepthTesting = false;
		public static bool EnableWireframe = false;

		public static FGLColor DrawColor = FGLColor.White;
		public static FGLShader ShaderProgram;

		public static void Fill(ref FGLFramebuffer FB, FGLColor Clr)
		{
			for (int i = 0; i < FB.ColorLen; i++)
				FB.DataPtr[i] = Clr;
		}

		public static void Rect(int X, int Y, int W, int H)
		{
			for (int j = Y; j < Y + H; j++)
				for (int i = X; i < X + W; i++)
					ColorBuffer.DataPtr[j * ColorBuffer.Width + i] = DrawColor;
		}

		public static void Line(int X0, int Y0, int X1, int Y1)
		{
			bool Steep = false;

			if (Math.Abs(X0 - X1) < Math.Abs(Y0 - Y1))
			{
				int Tmp = X0;
				X0 = Y0;
				Y0 = Tmp;

				Tmp = X1;
				X1 = Y1;
				Y1 = Tmp;

				Steep = true;
			}

			if (X0 > X1)
			{
				int Tmp = X0;
				X0 = X1;
				X1 = Tmp;

				Tmp = Y0;
				Y0 = Y1;
				Y1 = Tmp;
			}

			int DeltaX = X1 - X0;
			int DeltaY = Y1 - Y0;
			int DeltaError2 = Math.Abs(DeltaY) * 2;
			int Error2 = 0;
			int Y = Y0;

			for (int X = X0; X <= X1; X++)
			{
				if (Steep)
					ColorBuffer.DataPtr[X * ColorBuffer.Width + Y] = DrawColor;
				else
					ColorBuffer.DataPtr[Y * ColorBuffer.Width + X] = DrawColor;


				Error2 += DeltaError2;

				if (Error2 > DeltaX)
				{
					Y += (Y1 > Y0 ? 1 : -1);
					Error2 -= DeltaX * 2;
				}
			}
		}

		public static void Triangle(Tri Tri)
		{
			if (ShaderProgram != null)
			{
				ShaderProgram.Vertex(ref Tri.A);
				ShaderProgram.Vertex(ref Tri.B);
				ShaderProgram.Vertex(ref Tri.C);
			}

			Vector3 Cross = Vector3.Normalize(Vector3.Cross(Tri.C - Tri.A, Tri.B - Tri.A));

			// Backface culling
			if (EnableBackfaceCulling && Cross.Z < 0)
				return;

			FGLColor PixColor = DrawColor;
			Vector3 Min, Max, BCnt = Vector3.Zero;
			Helpers.BoundingBox(ref Tri, out Min, out Max);

			for (int Y = (int)Min.Y; Y < Max.Y; Y++)
				for (int X = (int)Min.X; X < Max.X; X++)
				{
					if (X < 0 || Y < 0 || X >= ColorBuffer.Width || Y >= ColorBuffer.Height)
						continue;

					Helpers.Barycentric(ref Tri, X, Y, ref BCnt);
					if (BCnt.X < 0 || BCnt.Y < 0 || BCnt.Z < 0)
						continue;

					int Idx = Y * ColorBuffer.Width + X;
					float D = ((Tri.A.Z * BCnt.X) + (Tri.B.Z * BCnt.Y) + (Tri.C.Z * BCnt.Z));

					if (!EnableDepthTesting || (DepthBuffer.DataPtr[Idx].Float > D))
					{
						// Calculate UV coordinates
						float TexU = (Tri.A_UV.X * BCnt.X) + (Tri.B_UV.X * BCnt.Y) + (Tri.C_UV.X * BCnt.Z);
						float TexV = (Tri.A_UV.Y * BCnt.X) + (Tri.B_UV.Y * BCnt.Y) + (Tri.C_UV.Y * BCnt.Z);

						if (ShaderProgram != null)
						{
							bool Discard = false;
							ShaderProgram.Pixel(ref PixColor, TexU, TexV, X, Y, D, ref Discard);
							if (Discard)
								continue;
						}

						if (EnableShading)
							FGLColor.ScaleColor(ref PixColor, Math.Abs(Cross.Z));

						if (EnableTexturing)
						{
							// Scale UVs to texture size
							TexU *= TEX0Buffer.Width;
							TexV *= TEX0Buffer.Height;

							FGLColor TexClr = TEX0Buffer.DataPtr[(int)TexV * TEX0Buffer.Width + (int)TexU];
							FGLColor.ScaleColor(ref PixColor, ref TexClr);
						}

						DepthBuffer.DataPtr[Idx].Float = D;

						if (PixColor.A == 255)
							ColorBuffer.DataPtr[Y * ColorBuffer.Width + X] = PixColor;
						else if (PixColor.A != 0)
							FGLColor.Blend(ref ColorBuffer.DataPtr[Y * ColorBuffer.Width + X], ref PixColor);
					}
				}

			if (EnableWireframe)
			{
				Line((int)Tri.A.X, (int)Tri.A.Y, (int)Tri.B.X, (int)Tri.B.Y);
				Line((int)Tri.B.X, (int)Tri.B.Y, (int)Tri.C.X, (int)Tri.C.Y);
				Line((int)Tri.C.X, (int)Tri.C.Y, (int)Tri.A.X, (int)Tri.A.Y);
			}
		}
	}
}
