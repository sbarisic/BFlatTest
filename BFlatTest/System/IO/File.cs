using Internal.Runtime.CompilerHelpers;
using System;

namespace System.IO
{
	public unsafe class File
	{
		public static EFI_BOOT_SERVICES* bootServices;
		public static EFI_HANDLE imageHandle;
		public static EFI_HANDLE deviceHandle;

		public static byte[] ReadAllBytes(string path)
		{
			nuint fileSize = 0;

			// 1. Locate the filesystem of the loaded image
			EFI_HANDLE fsHandle = deviceHandle;
			EFI_SIMPLE_FILE_SYSTEM_PROTOCOL* fs;
			EFI_GUID sfsguid = new EFI_GUID(0x964e5b22, 0x6459, 0x11d2, 0x8e, 0x39, 0x0, 0xa0, 0xc9, 0x69, 0x72, 0x3b);

			EFI_HANDLE nullHandle = new EFI_HANDLE(IntPtr.Zero);
			bootServices->OpenProtocol(fsHandle, &sfsguid, (void**)&fs, imageHandle, nullHandle, EFI_OPEN_PROTOCOL.BY_HANDLE_PROTOCOL);

			// 2. Open the root volume
			EFI_FILE_PROTOCOL* root;
			fs->OpenVolume(fs, &root);

			// 3. Open the file
			EFI_FILE_PROTOCOL* file;

			fixed (char* pathPtr = path)
			{
				root->Open(root, &file, pathPtr, EFI_FILE_MODE_READ, 0);
			}

			// 4. Get file size
			EFI_FILE_INFO* info;
			ulong infoSize = 0;
			EFI_GUID fileInfoGUID = new EFI_GUID(0x9576e92, 0x6d3f, 0x11d2, 0x8e, 0x39, 0x0, 0xa0, 0xc9, 0x69, 0x72, 0x3b);

			file->GetInfo(file, &fileInfoGUID, (nuint*)&infoSize, null); // first call gets required size
			bootServices->AllocatePool(EFI_MEMORY_TYPE.EfiLoaderData, (nuint)infoSize, (void**)&info);
			file->GetInfo(file, &fileInfoGUID, (nuint*)&infoSize, info);
			fileSize = info->FileSize;

			// 5. Allocate buffer
			byte* buffer = null;
			bootServices->AllocatePool(EFI_MEMORY_TYPE.EfiLoaderData, fileSize, (void**)&buffer);

			// 6. Read file into buffer
			ulong readSize = fileSize;
			file->Read(file, (nuint*)&readSize, buffer);

			// 7. Close file
			file->Close(file);

			byte[] byteArr = new byte[readSize];
			for (ulong i = 0; i < readSize; i++)
			{

				byteArr[i] = buffer[i];
			}
			bootServices->FreePool(buffer);

			return byteArr;
		}
	}
}
