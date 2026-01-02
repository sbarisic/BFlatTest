using Fish;
using Internal.Runtime.CompilerHelpers;
using System;
using System.Runtime.CompilerServices;

namespace Fish
{
    public unsafe class Framebuffer
    {
        EFI_GRAPHICS_OUTPUT_PROTOCOL* gop;

        EFI_GRAPHICS_PIXEL_FORMAT PixelFormat;
        byte* FramebufferPtr;
        nuint FramebufferSize;
        int PixelsPerScanLine;
        int Bpp;
        public uint Width;
        public uint Height;

        public Framebuffer(EFI_GRAPHICS_OUTPUT_PROTOCOL* gop)
        {
            this.gop = gop;
        }

        public static Framebuffer InitFramebuffer(EFI_HANDLE imageHandle, EFI_SYSTEM_TABLE* systemTable)
        {
            EFI_GUID gopGuid = new EFI_GUID(0x9042a9de, 0x23dc, 0x4a38, 0x96, 0xfb, 0x7a, 0xde, 0xd0, 0x80, 0x51, 0x6a);
            EFI_GRAPHICS_OUTPUT_PROTOCOL* gop;
            systemTable->BootServices->LocateProtocol(&gopGuid, null, (void**)&gop);

            if (gop == null)
                return null;

            return new Framebuffer(gop);
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
            {
                Console.Print("Framebuffer ", Utils.PtrToHexString((nint)FramebufferPtr), ", Size ", (ulong)FramebufferSize, ", Bpp ", Bpp);
                Console.Print("Width: ", Width, ", Height: ", Height);
            }

            //Unsafe.InitBlockUnaligned(ref Unsafe.AsRef<byte>(FramebufferPtr), 0xFF, 1920);
            //DrawRect(50, 50, 60, 60, new Color(255, 0, 0, 0));

            Console.Write("FishGL init ... ");
            FishGL.ColorBuffer = new FGLFramebuffer(Width, Height);
            FishGL.DepthBuffer = new FGLFramebuffer(Width, Height);
            FishGL.Fill(ref FishGL.ColorBuffer, new FGLColor(0, 0, 0));

            /*FishGL.Fill(ref FishGL.ColorBuffer, new FGLColor(88, 104, 115));

            FishGL.DrawColor = new FGLColor(0, 255, 0);
            FishGL.Rect(350, 220, 100, 100);

            FishGL.DrawColor = FGLColor.Black;
            FishGL.Line(300, 250, 800, 400);*/

            Console.WriteLine("OK");

        }

        public void SwapBuffer()
        {
            Unsafe.Copy(ref Unsafe.AsRef<byte>(FishGL.ColorBuffer.DataPtr), ref Unsafe.AsRef<byte>(FramebufferPtr), 1920 * 1080 * 4);
        }

        public void DrawRect(int X, int Y, int W, int H, Color color)
        {
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
