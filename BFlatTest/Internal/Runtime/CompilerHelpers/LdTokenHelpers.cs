using System;
using System.Runtime;

namespace Internal.Runtime.CompilerHelpers
{
	internal unsafe class LdTokenHelpers
	{
		public static RuntimeTypeHandle GetRuntimeTypeHandle(MethodTable* mt)
		{
			return new RuntimeTypeHandle((nint)mt);
		}
	}
}
