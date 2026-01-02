// bflat minimal runtime library
// Copyright (C) 2021-2022 Michal Strehovsky
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

#if UEFI

using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Internal.Runtime.CompilerHelpers
{
	unsafe partial class StartupCodeHelpers
	{
		//[RuntimeImport("*", "__managed__Main")]
		//[MethodImpl(MethodImplOptions.InternalCall)]
		//static extern int ManagedMain(int argc, char** argv);

		[RuntimeImport("*", "EfiMain2")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		static extern int ManagedMain(IntPtr imageHandle, EFI_SYSTEM_TABLE* systemTable);

		[RuntimeExport("EfiMain")]
		static long EfiMain(IntPtr imageHandle, EFI_SYSTEM_TABLE* systemTable)
		{
			SetEfiSystemTable(systemTable);
			ManagedMain(imageHandle, systemTable);

			while (true) ;
		}

		internal static unsafe void InitializeCommandLineArgsW(int argc, char** argv)
		{
			// argc and argv are garbage because EfiMain didn't pass any
		}

		internal static string[] GetMainMethodArguments()
		{
			return new string[0];
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EFI_HANDLE
	{
		private IntPtr _handle;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe readonly struct EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL
	{
		private readonly IntPtr _pad0;
		public readonly delegate* unmanaged<void*, char*, void*> OutputString;
		private readonly IntPtr _pad1;
		private readonly IntPtr _pad2;
		private readonly IntPtr _pad3;
		public readonly delegate* unmanaged<void*, uint, void> SetAttribute;
		private readonly IntPtr _pad4;
		public readonly delegate* unmanaged<void*, uint, uint, void> SetCursorPosition;
	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct EFI_INPUT_KEY
	{
		public readonly ushort ScanCode;
		public readonly ushort UnicodeChar;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe readonly struct EFI_SIMPLE_TEXT_INPUT_PROTOCOL
	{
		private readonly IntPtr _pad0;
		public readonly delegate* unmanaged<void*, EFI_INPUT_KEY*, ulong> ReadKeyStroke;
	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct EFI_TABLE_HEADER
	{
		public readonly ulong Signature;
		public readonly uint Revision;
		public readonly uint HeaderSize;
		public readonly uint Crc32;
		public readonly uint Reserved;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe readonly struct EFI_SYSTEM_TABLE
	{
		public readonly EFI_TABLE_HEADER Hdr;
		public readonly char* FirmwareVendor;
		public readonly uint FirmwareRevision;
		public readonly EFI_HANDLE ConsoleInHandle;
		public readonly EFI_SIMPLE_TEXT_INPUT_PROTOCOL* ConIn;
		public readonly EFI_HANDLE ConsoleOutHandle;
		public readonly EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* ConOut;
		public readonly EFI_HANDLE StandardErrorHandle;
		public readonly EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* StdErr;
		public readonly EFI_RUNTIME_SERVICES* RuntimeServices;
		public readonly EFI_BOOT_SERVICES* BootServices;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EFI_TIME
	{
		public ushort Year;
		public byte Month;
		public byte Day;
		public byte Hour;
		public byte Minute;
		public byte Second;
		public byte Pad1;
		public uint Nanosecond;
		public short TimeZone;
		public byte Daylight;
		public byte PAD2;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EFI_TIME_CAPABILITIES
	{
		public uint Resolution;
		public uint Accuracy;
		public byte SetsToZero;
	}

	public enum EFI_RESET_TYPE
	{
		EfiResetCold,
		EfiResetWarm,
		EfiResetShutdown,
		EfiResetPlatformSpecific
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EFI_CAPSULE_HEADER
	{
		public EFI_GUID CapsuleGuid;
		public uint HeaderSize;
		public uint Flags;
		public uint CapsuleImageSize;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe readonly struct EFI_RUNTIME_SERVICES
	{
		public readonly EFI_TABLE_HEADER Hdr;
		public readonly delegate* unmanaged<EFI_TIME*, EFI_TIME_CAPABILITIES*, ulong> GetTime;
		public readonly delegate* unmanaged<EFI_TIME*, ulong> SetTime;
		public readonly delegate* unmanaged<byte*, byte*, EFI_TIME*, ulong> GetWakeupTime;
		public readonly delegate* unmanaged<byte, EFI_TIME*, ulong> SetWakeupTime;
		public readonly delegate* unmanaged<nuint, nuint, uint, EFI_MEMORY_DESCRIPTOR*, ulong> SetVirtualAddressMap;
		public readonly delegate* unmanaged<nuint, void**, ulong> ConvertPointer;
		public readonly delegate* unmanaged<char*, EFI_GUID*, uint*, nuint*, void*, ulong> GetVariable;
		public readonly delegate* unmanaged<nuint*, char*, EFI_GUID*, ulong> GetNextVariableName;
		public readonly delegate* unmanaged<char*, EFI_GUID*, uint, nuint, void*, ulong> SetVariable;
		public readonly delegate* unmanaged<uint*, ulong> GetNextHighMonotonicCount;
		public readonly delegate* unmanaged<EFI_RESET_TYPE, EFI_STATUS, nuint, void*, void> ResetSystem;
		public readonly delegate* unmanaged<EFI_CAPSULE_HEADER**, nuint, EFI_PHYSICAL_ADDRESS, ulong> UpdateCapsule;
		public readonly delegate* unmanaged<EFI_CAPSULE_HEADER**, nuint, ulong*, EFI_RESET_TYPE*, ulong> QueryCapsuleCapabilities;
		public readonly delegate* unmanaged<uint, ulong*, ulong*, ulong*, ulong> QueryVariableInfo;
	}

	public enum EFI_INTERFACE_TYPE
	{
		EFI_NATIVE_INTERFACE
	}

	public enum EFI_LOCATE_SEARCH_TYPE
	{
		AllHandles,
		ByRegisterNotify,
		ByProtocol
	}

	public enum EFI_ALLOCATE_TYPE
	{
		AllocateAnyPages,
		AllocateMaxAddress,
		AllocateAddress,
		MaxAllocateType
	}

	public enum EFI_MEMORY_TYPE
	{
		EfiReservedMemoryType,
		EfiLoaderCode,
		EfiLoaderData,
		EfiBootServicesCode,
		EfiBootServicesData,
		EfiRuntimeServicesCode,
		EfiRuntimeServicesData,
		EfiConventionalMemory,
		EfiUnusableMemory,
		EfiACPIReclaimMemory,
		EfiACPIMemoryNVS,
		EfiMemoryMappedIO,
		EfiMemoryMappedIOPortSpace,
		EfiPalCode,
		EfiPersistentMemory,
		EfiMaxMemoryType
	}

	public enum EFI_TIMER_DELAY
	{
		TimerCancel,
		TimerPeriodic,
		TimerRelative
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EFI_DEVICE_PATH_PROTOCOL
	{
		public byte Type;
		public byte SubType;
		public unsafe fixed byte Length[2];
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EFI_OPEN_PROTOCOL_INFORMATION_ENTRY
	{
		public EFI_HANDLE AgentHandle;
		public EFI_HANDLE ControllerHandle;
		public uint Attributes;
		public uint OpenCount;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EFI_MEMORY_DESCRIPTOR
	{
		public uint Type;
		public ulong PhysicalStart;
		public ulong VirtualStart;
		public ulong NumberOfPages;
		public ulong Attribute;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EFI_GUID
	{
		public uint Data1;
		public ushort Data2;
		public ushort Data3;
		public unsafe fixed byte Data4[8];

		public EFI_GUID(uint d1, ushort d2, ushort d3, byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7)
		{
			Data1 = d1;
			Data2 = d2;
			Data3 = d3;
			unsafe
			{
				fixed (byte* pData4 = Data4)
				{
					pData4[0] = b0;
					pData4[1] = b1;
					pData4[2] = b2;
					pData4[3] = b3;
					pData4[4] = b4;
					pData4[5] = b5;
					pData4[6] = b6;
					pData4[7] = b7;
				}
			}
		}
	}

	public struct EFI_EVENT
	{
		private IntPtr _event;
	}

	public struct EFI_TPL
	{
		private nuint _tpl;
	}

	public struct EFI_STATUS
	{
		private nuint _status;
	}

	public struct EFI_PHYSICAL_ADDRESS
	{
		public ulong _address;
	}

	/* // TODO: translate to C#
	 * typedef struct {
  UINT8 Type;       ///< 0x01 Hardware Device Path.
                    ///< 0x02 ACPI Device Path.
                    ///< 0x03 Messaging Device Path.
                    ///< 0x04 Media Device Path.
                    ///< 0x05 BIOS Boot Specification Device Path.
                    ///< 0x7F End of Hardware Device Path.

  UINT8 SubType;    ///< Varies by Type
                    ///< 0xFF End Entire Device Path, or
                    ///< 0x01 End This Instance of a Device Path and start a new
                    ///< Device Path.

  UINT8 Length[2];  ///< Specific Device Path data. Type and Sub-Type define
                    ///< type of data. Size of data is included in Length.

} EFI_DEVICE_PATH_PROTOCOL;

	typedef struct {
		UINT32 Revision;     ///< Defines the revision of the EFI_LOADED_IMAGE_PROTOCOL structure.
							 ///< All future revisions will be backward compatible to the current revision.
		EFI_HANDLE ParentHandle; ///< Parent image's image handle. NULL if the image is loaded directly from
								 ///< the firmware's boot manager.
		EFI_SYSTEM_TABLE* SystemTable; ///< the image's EFI system table pointer.

		//
		// Source location of image
		//
		EFI_HANDLE DeviceHandle; ///< The device handle that the EFI Image was loaded from.
		EFI_DEVICE_PATH_PROTOCOL* FilePath;    ///< A pointer to the file path portion specific to DeviceHandle
											   ///< that the EFI Image was loaded from.
		VOID* Reserved;    ///< Reserved. DO NOT USE.

		//
		// Images load options
		//
		UINT32 LoadOptionsSize; ///< The size in bytes of LoadOptions.
		VOID* LoadOptions;    ///< A pointer to the image's binary load options.

		//
		// Location of where image was loaded
		//
		VOID* ImageBase;    ///< The base address at which the image was loaded.
		UINT64 ImageSize;     ///< The size in bytes of the loaded image.
		EFI_MEMORY_TYPE ImageCodeType; ///< The memory type that the code sections were loaded as.
		EFI_MEMORY_TYPE ImageDataType; ///< The memory type that the data sections were loaded as.
		EFI_IMAGE_UNLOAD Unload;
	}
	EFI_LOADED_IMAGE_PROTOCOL;
	*/

	[StructLayout(LayoutKind.Sequential)]
	public unsafe readonly struct EFI_BOOT_SERVICES
	{
		public readonly EFI_TABLE_HEADER Hdr;
		public readonly delegate* unmanaged<EFI_TPL, EFI_TPL> RaiseTPL;
		public readonly delegate* unmanaged<EFI_TPL, void> RestoreTPL;
		public readonly delegate* unmanaged<EFI_ALLOCATE_TYPE, EFI_MEMORY_TYPE, nuint, EFI_PHYSICAL_ADDRESS*, ulong> AllocatePages;
		public readonly delegate* unmanaged<EFI_PHYSICAL_ADDRESS, nuint, ulong> FreePages;
		public readonly delegate* unmanaged<nuint*, EFI_MEMORY_DESCRIPTOR*, nuint*, nuint*, uint*, ulong> GetMemoryMap;
		public readonly delegate* unmanaged<EFI_MEMORY_TYPE, nuint, void**, ulong> AllocatePool;
		public readonly delegate* unmanaged<void*, ulong> FreePool;
		public readonly delegate* unmanaged<uint, EFI_TPL, delegate* unmanaged<EFI_EVENT, void*, void>, void*, EFI_EVENT*, ulong> CreateEvent;
		public readonly delegate* unmanaged<EFI_EVENT, EFI_TIMER_DELAY, ulong, ulong> SetTimer;
		public readonly delegate* unmanaged<nuint, EFI_EVENT*, nuint*, ulong> WaitForEvent;
		public readonly delegate* unmanaged<EFI_EVENT, ulong> SignalEvent;
		public readonly delegate* unmanaged<EFI_EVENT, ulong> CloseEvent;
		public readonly delegate* unmanaged<EFI_EVENT, ulong> CheckEvent;
		public readonly delegate* unmanaged<EFI_HANDLE*, EFI_GUID*, EFI_INTERFACE_TYPE, void*, ulong> InstallProtocolInterface;
		public readonly delegate* unmanaged<EFI_HANDLE, EFI_GUID*, void*, void*, ulong> ReinstallProtocolInterface;
		public readonly delegate* unmanaged<EFI_HANDLE, EFI_GUID*, void*, ulong> UninstallProtocolInterface;
		public readonly delegate* unmanaged<EFI_HANDLE, EFI_GUID*, void**, ulong> HandleProtocol;
		private readonly void* Reserved;
		public readonly delegate* unmanaged<EFI_GUID*, EFI_EVENT, void**, ulong> RegisterProtocolNotify;
		public readonly delegate* unmanaged<EFI_LOCATE_SEARCH_TYPE, EFI_GUID*, void*, nuint*, EFI_HANDLE*, ulong> LocateHandle;
		public readonly delegate* unmanaged<EFI_GUID*, EFI_DEVICE_PATH_PROTOCOL**, EFI_HANDLE*, ulong> LocateDevicePath;
		public readonly delegate* unmanaged<EFI_GUID*, void*, ulong> InstallConfigurationTable;
		public readonly delegate* unmanaged<byte, EFI_HANDLE, EFI_DEVICE_PATH_PROTOCOL*, void*, nuint, EFI_HANDLE*, ulong> LoadImage;
		public readonly delegate* unmanaged<EFI_HANDLE, nuint*, ushort**, ulong> StartImage;
		public readonly delegate* unmanaged<EFI_HANDLE, EFI_STATUS, nuint, ushort*, ulong> Exit;
		public readonly delegate* unmanaged<EFI_HANDLE, ulong> UnloadImage;
		public readonly delegate* unmanaged<EFI_HANDLE, nuint, ulong> ExitBootServices;
		public readonly delegate* unmanaged<ulong*, ulong> GetNextMonotonicCount;
		public readonly delegate* unmanaged<nuint, ulong> Stall;
		public readonly delegate* unmanaged<nuint, ulong, nuint, ushort*, ulong> SetWatchdogTimer;
		public readonly delegate* unmanaged<EFI_HANDLE, EFI_HANDLE*, EFI_DEVICE_PATH_PROTOCOL*, byte, ulong> ConnectController;
		public readonly delegate* unmanaged<EFI_HANDLE, EFI_HANDLE, EFI_HANDLE, ulong> DisconnectController;
		public readonly delegate* unmanaged<EFI_HANDLE, EFI_GUID*, void**, EFI_HANDLE, EFI_HANDLE, uint, ulong> OpenProtocol;
		public readonly delegate* unmanaged<EFI_HANDLE, EFI_GUID*, EFI_HANDLE, EFI_HANDLE, ulong> CloseProtocol;
		public readonly delegate* unmanaged<EFI_HANDLE, EFI_GUID*, EFI_OPEN_PROTOCOL_INFORMATION_ENTRY**, nuint*, ulong> OpenProtocolInformation;
		public readonly delegate* unmanaged<EFI_HANDLE, EFI_GUID***, nuint*, ulong> ProtocolsPerHandle;
		public readonly delegate* unmanaged<EFI_LOCATE_SEARCH_TYPE, EFI_GUID*, void*, nuint*, EFI_HANDLE**, ulong> LocateHandleBuffer;
		public readonly delegate* unmanaged<EFI_GUID*, void*, void**, ulong> LocateProtocol;
		private readonly void* InstallMultipleProtocolInterfaces;
		private readonly void* UninstallMultipleProtocolInterfaces;
		public readonly delegate* unmanaged<void*, nuint, uint*, ulong> CalculateCrc32;
		public readonly delegate* unmanaged<void*, void*, nuint, void> CopyMem;
		public readonly delegate* unmanaged<void*, nuint, byte, void> SetMem;
		public readonly delegate* unmanaged<uint, EFI_TPL, delegate* unmanaged<EFI_EVENT, void*, void>, void*, EFI_GUID*, EFI_EVENT*, ulong> CreateEventEx;
	}

	public enum EFI_GRAPHICS_PIXEL_FORMAT
	{
		PixelRedGreenBlueReserved8BitPerColor,
		PixelBlueGreenRedReserved8BitPerColor,
		PixelBitMask,
		PixelBltOnly,
		PixelFormatMax
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EFI_PIXEL_BITMASK
	{
		public uint RedMask;
		public uint GreenMask;
		public uint BlueMask;
		public uint ReservedMask;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EFI_GRAPHICS_OUTPUT_MODE_INFORMATION
	{
		public uint Version;
		public uint HorizontalResolution;
		public uint VerticalResolution;
		public EFI_GRAPHICS_PIXEL_FORMAT PixelFormat;
		public EFI_PIXEL_BITMASK PixelInformation;
		public uint PixelsPerScanLine;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EFI_GRAPHICS_OUTPUT_BLT_PIXEL
	{
		public byte Blue;
		public byte Green;
		public byte Red;
		public byte Reserved;

		public EFI_GRAPHICS_OUTPUT_BLT_PIXEL(byte r, byte g, byte b)
		{
			Red = r;
			Green = g;
			Blue = b;
			Reserved = 0;
		}
	}

	public enum EFI_GRAPHICS_OUTPUT_BLT_OPERATION
	{
		EfiBltVideoFill,
		EfiBltVideoToBltBuffer,
		EfiBltBufferToVideo,
		EfiBltVideoToVideo,
		EfiGraphicsOutputBltOperationMax
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct EFI_GRAPHICS_OUTPUT_PROTOCOL_MODE
	{
		public uint MaxMode;
		public uint Mode;
		public EFI_GRAPHICS_OUTPUT_MODE_INFORMATION* Info;
		public nuint SizeOfInfo;
		public EFI_PHYSICAL_ADDRESS FrameBufferBase;
		public nuint FrameBufferSize;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe readonly struct EFI_GRAPHICS_OUTPUT_PROTOCOL
	{
		public readonly delegate* unmanaged<EFI_GRAPHICS_OUTPUT_PROTOCOL*, uint, nuint*, EFI_GRAPHICS_OUTPUT_MODE_INFORMATION**, ulong> QueryMode;
		public readonly delegate* unmanaged<EFI_GRAPHICS_OUTPUT_PROTOCOL*, uint, ulong> SetMode;
		public readonly delegate* unmanaged<EFI_GRAPHICS_OUTPUT_PROTOCOL*, EFI_GRAPHICS_OUTPUT_BLT_PIXEL*, EFI_GRAPHICS_OUTPUT_BLT_OPERATION, nuint, nuint, nuint, nuint, nuint, nuint, nuint, ulong> Blt;
		public readonly EFI_GRAPHICS_OUTPUT_PROTOCOL_MODE* Mode;
	}
}

#endif
