using System;

namespace Internal.Runtime.CompilerHelpers
{
	partial class ThrowHelpers
	{
		public static void ThrowIndexOutOfRangeException() => Environment.FailFast("ThrowIndexOutOfRangeException");

		public static void ThrowDivideByZeroException() => Environment.FailFast("ThrowDivideByZeroException");

		public static void ThrowPlatformNotSupportedException() => Environment.FailFast("ThrowPlatformNotSupportedException");

		public static void ThrowInvalidProgramException() => Environment.FailFast("ThrowInvalidProgramException");

		public static void ThrowOverflowException() => Environment.FailFast("ThrowOverflowException");

		public static void ThrowInvalidProgramExceptionWithArgument(object id, string methodName) => Environment.FailFast("ThrowInvalidProgramExceptionWithArgument");
	}
}
