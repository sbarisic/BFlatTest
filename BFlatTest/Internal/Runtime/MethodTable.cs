using System;
using System.Runtime.InteropServices;

namespace Internal.Runtime
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal unsafe struct MethodTable
	{
		public ushort _usComponentSize;
		public ushort _usFlags;
		public uint _uBaseSize;

		public MethodTable* _uBaseType;

		public ushort _usNumVtableSlots;
		public ushort _usNumInterfaces;
		public uint _uHashCode;

		public void** m_VTable;  
	}
}
