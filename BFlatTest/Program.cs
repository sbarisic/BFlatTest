using System;
using System.Runtime.InteropServices;
using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using Internal.Runtime.CompilerHelpers;

public unsafe static class Program
{
	public static string PtrToHexString(nint value)
	{
		const string hex = "0123456789ABCDEF";

		// 2 for "0x" + 16 nybbles for 64-bit + null
		char* buf = stackalloc char[2 + 16];

		buf[0] = '0';
		buf[1] = 'x';

		ulong v = (ulong)value;

		for (int i = 0; i < 16; i++)
		{
			int shift = (15 - i) * 4;
			buf[2 + i] = hex[(int)((v >> shift) & 0xF)];
		}

		return new string(buf);
	}

	static string ToStr(nint ptr)
	{
		//return ptr.ToString("X16");
		return PtrToHexString(ptr);
	}


	[UnmanagedCallersOnly(EntryPoint = "EfiMain2")]
	public static int EfiMain2(IntPtr imageHandle, EFI_SYSTEM_TABLE* systemTable)
	{
		Console.WriteLine("efi_main Hello World");

		Console.WriteLine(ToStr((nint)imageHandle));
		Console.WriteLine(ToStr((nint)systemTable));

		
		systemTable->RuntimeServices->

		// Use it to access UEFI services  
		//systemTable->ConOut->OutputString(systemTable->ConOut, "Hello from UEFI!\0");

		return 0;
	}
}

