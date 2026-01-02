using System;

namespace System.Runtime.InteropServices
{
	public unsafe struct NativeFunctionPointerWrapper
	{
		public IntPtr NativeFunctionPointer { get; }

		public NativeFunctionPointerWrapper(IntPtr nativeFunctionPointer)
		{
			NativeFunctionPointer = nativeFunctionPointer;
		}
	}
}
