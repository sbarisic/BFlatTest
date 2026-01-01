using Fish;
using Internal.Runtime.CompilerHelpers;
using System;
using System.Runtime.CompilerServices;

namespace Kernel
{
	public unsafe class Framebuffer
	{
		EFI_GRAPHICS_OUTPUT_PROTOCOL* gop;

		EFI_GRAPHICS_PIXEL_FORMAT PixelFormat;
		byte* FramebufferPtr;
		nuint FramebufferSize;
		int PixelsPerScanLine;
		int Bpp;
		uint Width;
		uint Height;

		public Framebuffer(EFI_GRAPHICS_OUTPUT_PROTOCOL* gop)
		{
			this.gop = gop;
		}

		public unsafe void SetPixel(int x, int y, Color c)
		{
			ulong index = ((ulong)y * (ulong)PixelsPerScanLine + (ulong)x);
			byte* p = ((byte*)FramebufferPtr) + index * 4;

			p[0] = c.B;
			p[1] = c.G;
			p[2] = c.R;
			p[3] = c.A;
		}


		public void Init(int DesW, int DesH)
		{
			nuint SizeOfInfo;
			EFI_GRAPHICS_OUTPUT_MODE_INFORMATION* info;
			gop->QueryMode(gop, 0, &SizeOfInfo, &info);

			EFI_GRAPHICS_OUTPUT_PROTOCOL_MODE* mode = null;

			uint numModes = gop->Mode->MaxMode;
			for (uint i = 0; i < numModes; i++)
			{
				gop->QueryMode(gop, i, &SizeOfInfo, &info);

				int W = (int)info->HorizontalResolution;
				int H = (int)info->VerticalResolution;
				Console.Print("Mode ", i, ", W = ", W, ", H = ", H);

				if (W == DesW && H == DesH)
				{
					Console.Print("Setting mode to ", DesW, "x", DesH);
					gop->SetMode(gop, i);
					gop->QueryMode(gop, 0, &SizeOfInfo, &info);

					mode = gop->Mode;
					break;
				}
			}

			Console.SetCursorPosition(0, 0);

			FramebufferPtr = (byte*)mode->FrameBufferBase._address;
			FramebufferSize = (nuint)mode->FrameBufferSize;
			PixelsPerScanLine = (int)mode->Info->PixelsPerScanLine;
			Width = mode->Info->HorizontalResolution;
			Height = mode->Info->VerticalResolution;
			PixelFormat = mode->Info->PixelFormat;
			Bpp = 1;

			switch (PixelFormat)
			{
				case EFI_GRAPHICS_PIXEL_FORMAT.PixelBlueGreenRedReserved8BitPerColor:
				case EFI_GRAPHICS_PIXEL_FORMAT.PixelRedGreenBlueReserved8BitPerColor:
					Bpp = 4;
					break;
			}


			if (FramebufferPtr == null)
				Console.WriteLine("FramebufferPtr is null");
			else
				Console.Print("Framebuffer ", Utils.PtrToHexString((nint)FramebufferPtr), ", Size ", (ulong)FramebufferSize, ", PixelsPerScanLine ", PixelsPerScanLine, ", Bpp ", Bpp);

			//Unsafe.InitBlockUnaligned(ref Unsafe.AsRef<byte>(FramebufferPtr), 0xFF, 1920);
			DrawRect(50, 50, 200, 200, new Color(255, 0, 0, 0));

			Console.WriteLine("Hello Worlde!");
			MathF.Sin(2);
		}

		public void DrawRect(int X, int Y, int W, int H, Color color)
		{
			/*nuint sourceX = 0;
			nuint sourceY = 0;
			nuint destX = (nuint)X;
			nuint destY = (nuint)Y;
			nuint ww = (nuint)W;
			nuint hh = (nuint)H;
			nuint delta = 0;
			gop->Blt(gop, &color, EFI_GRAPHICS_OUTPUT_BLT_OPERATION.EfiBltVideoFill, sourceX, sourceY, destX, destY, ww, hh, delta);*/

			for (int y = 0; y < H; y++)
			{
				for (int x = 0; x < W; x++)
				{
					SetPixel(X + x, Y + y, color);
				}
			}
		}
	}
}
