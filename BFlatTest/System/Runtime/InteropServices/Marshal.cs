using System;


namespace System.Runtime.InteropServices
{
	public unsafe static class Marshal
	{
		public static IntPtr GetFunctionPointerForDelegate(Delegate d)
		{
			return (IntPtr)d.m_functionPointer;
		}

		public static IntPtr GetFunctionPointerForDelegate<TDelegate>(TDelegate d)
		{
			return (IntPtr)(d as Delegate).m_functionPointer;
		}
	}
}
