using Internal.Runtime.CompilerHelpers;
using System;

namespace System.IO
{
	public unsafe class File
	{
		public static EFI_BOOT_SERVICES* bootServices;
		public static IntPtr imageHandle;
		public static EFI_HANDLE deviceHandle;

		public static byte[] ReadAllBytes(string path)
		{
			nuint fileSize = 0;

			// 1. Locate the filesystem of the loaded image
			EFI_HANDLE fsHandle = deviceHandle;
			EFI_SIMPLE_FILE_SYSTEM_PROTOCOL* fs;
			EFI_GUID sfsguid = new EFI_GUID();

			sfsguid.Data1 = 0x0964e5b22;
			sfsguid.Data2 = 0x6459;
			sfsguid.Data3 = 0x11e4;
			byte[] Data4Bytes = new byte[] { 0x9a, 0x5b, 0x00, 0x90, 0x27, 0x3f, 0xc1, 0x4d };
			for (int i = 0; i < 8; i++)
			{
				sfsguid.Data4[i] = Data4Bytes[i];
			}

			bootServices->OpenProtocol(fsHandle, sfsguid, &fs);

			// 2. Open the root volume
			EFI_FILE_PROTOCOL* root;
			fs->OpenVolume(fs, &root);

			// 3. Open the file
			EFI_FILE_PROTOCOL* file;
			root->Open(root, &file, path, EFI_FILE_MODE_READ, 0);

			// 4. Get file size
			EFI_FILE_INFO* info;
			ulong infoSize = 0;
			file->GetInfo(file, EFI_GUID.FileInfo, &infoSize, null); // first call gets required size
			info = (EFI_FILE_INFO*)BootServices.AllocatePool(EFI_ALLOCATE_TYPE.LoaderData, infoSize);
			file->GetInfo(file, EFI_GUID.FileInfo, &infoSize, info);
			fileSize = info->FileSize;

			// 5. Allocate buffer
			byte* buffer = null;
			bootServices->AllocatePool(EFI_ALLOCATE_TYPE.LoaderData, fileSize, (void**)&buffer);

			// 6. Read file into buffer
			ulong readSize = fileSize;
			file->Read(file, &readSize, buffer);

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
