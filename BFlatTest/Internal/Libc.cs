using Internal.Runtime.CompilerHelpers;
using System;
using System.Runtime;

namespace Internal
{
	public unsafe class Libc
	{
		[RuntimeExport("memset")]
		public static void* Memset(void* dest, int c, nuint count)
		{
			byte* ptr = (byte*)dest;

			for (nuint i = 0; i < count; i++)
			{
				ptr[i] = (byte)c;
			}

			return dest;
		}

		[RuntimeExport("memcpy")]
		public static void* Memcpy(void* dest, void* src, nuint count)
		{
			byte* d = (byte*)dest;
			byte* s = (byte*)src;
			
			for (nuint i = 0; i < count; i++)
			{
				d[i] = s[i];
			}
		
			return dest;
		}

		[RuntimeExport("malloc")]
		public static void* Malloc(nuint size)
		{
			if (EfiSystemTable != null) {
				void* result = null;

				if (EfiSystemTable->BootServices->AllocatePool(EFI_MEMORY_TYPE.EfiLoaderData, size, &result) != 0)
					return null;

				return result;
			}

			return null;
		}

		[RuntimeExport("calloc")]
		public static void* Calloc(nuint num, nuint size)
		{
			nuint totalSize = num * size;
			void* ptr = Malloc(totalSize);

			if (ptr != null)
			{
				Memset(ptr, 0, totalSize);
			}

			return ptr;
		}

		[RuntimeExport("free")]
		public static void Free(void* ptr)
		{
			if (EfiSystemTable != null) {
				EfiSystemTable->BootServices->FreePool(ptr);
			}
		}
	}
}
