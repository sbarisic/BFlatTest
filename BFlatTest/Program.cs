using System;
using System.Runtime.InteropServices;
using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using Internal.Runtime.CompilerHelpers;
using Kernel;
using Fish;

public unsafe static class Program
{
	static string ToStr(nint ptr)
	{
		//return ptr.ToString("X16");
		return Utils.PtrToHexString(ptr);
	}


	[UnmanagedCallersOnly(EntryPoint = "EfiMain2")]
	public static int EfiMain2(IntPtr imageHandle, EFI_SYSTEM_TABLE* systemTable)
	{
		Console.WriteLine("efi_main Hello World");

		Console.WriteLine(ToStr((nint)imageHandle));
		Console.WriteLine(ToStr((nint)systemTable));

		//systemTable->BootServices->Stall();

		string message = "Hello from UEFI via Console Output!\r\n\0";
		fixed (char* messagePtr = message)
			systemTable->ConOut->OutputString(systemTable->ConOut, messagePtr);


		byte[] Data4Bytes = new byte[] { 0x96, 0xfb, 0x7a, 0xde, 0xd0, 0x80, 0x51, 0x6a };

		EFI_GUID gopGuid = new EFI_GUID();
		gopGuid.Data1 = 0x9042a9de;
		gopGuid.Data2 = 0x23dc;
		gopGuid.Data3 = 0x4a38;
		for (int i = 0; i < 8; i++)
		{
			gopGuid.Data4[i] = Data4Bytes[i];
		}

		EFI_GRAPHICS_OUTPUT_PROTOCOL* gop;
		systemTable->BootServices->LocateProtocol(&gopGuid, null, (void**)&gop);

		if (gop == null)
		{
			Console.WriteLine("Unable to locate GOP");
		}
		else
		{
			Console.WriteLine("GOP Found");
			Console.WriteLine(ToStr((nint)gop));

			Framebuffer FB = new Framebuffer(gop);
			FB.Init(1920, 1080);

			Console.WriteLine("Hello 1920x1080 World!");

			FishGL.DrawColor = new Color(255, 0, 0);
			FishGL.Rect(100, 100, 100, 100);

			FishGL.DrawColor = new Color(0, 255, 0);
			FishGL.Rect(120, 120, 100, 100);

			FishGL.DrawColor = new Color(0, 0, 255);
			FishGL.Rect(140, 140, 100, 100);

			FB.SwapBuffer();
			Console.WriteLine("Rectangles drawn!");
		}


		// Use it to access UEFI services  
		//systemTable->ConOut->OutputString(systemTable->ConOut, "Hello from UEFI!\0");

		return 0;
	}
}

