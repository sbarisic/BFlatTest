using System;
using System.Runtime.InteropServices;

namespace Internal.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct MethodTable
	{
		public const ushort MTFlag_HasPointers = 0x0001;
		public const ushort MTFlag_ValueType = 0x0002;
		public const ushort MTFlag_Array = 0x0004;
		public const ushort MTFlag_String = 0x0008;

		public ushort _usComponentSize;
		public ushort _usFlags;
		public uint _uBaseSize;

		public MethodTable* _uBaseType;

		public ushort _usNumVtableSlots;
		public ushort _usNumInterfaces;
		public uint _uHashCode;

		public MethodTable** _interfaceMap;  // Interface map after vtable  

		public void* _elementType;          // Element type or PerInstInfo  

		public MethodTable*** _perInstInfo; // Generic instantiation info  

		public void* _writableData;         // NativeAOT specific - caches RuntimeType 

		public fixed byte bytes[64];
		
	}
}
