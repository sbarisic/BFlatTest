using System;
using System.Runtime.CompilerServices;


namespace System.Runtime.InteropServices
{
	public unsafe static class Marshal
	{
		public static void* GetFunctionPointerForDelegate(Delegate d)
		{
			Console.Print("GetFunctionPointerForDelegate - ", Utils.PtrToHexString(d.m_functionPointer));
			return (void*)d.m_functionPointer;
		}

		public static void* GetFunctionPointerForDelegate<TDelegate>(TDelegate d) where TDelegate : Delegate
		{
			Console.WriteLine("GetFunctionPointerForDelegate<TDelegate>");

			void* vptr = Unsafe.AsPointer(ref d);
			//Console.Print("AsPointer - ", Utils.PtrToHexString((nint)vptr));
			//Console.Print("GetFunctionPointerForDelegate - ", Utils.PtrToHexString(d.m_functionPointer));
			//return (IntPtr)d.m_functionPointer;
			//return (IntPtr)vptr;

			return (void*)d.m_functionPointer;
		}
	}
}
