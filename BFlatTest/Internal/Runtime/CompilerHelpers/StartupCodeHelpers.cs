using System;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace Internal.Runtime.CompilerHelpers
{
	// A class that the compiler looks for that has helpers to initialize the
	// process. The compiler can gracefully handle the helpers not being present,
	// but the class itself being absent is unhandled. Let's add an empty class.
	internal unsafe partial class StartupCodeHelpers
	{
		// A couple symbols the generated code will need we park them in this class
		// for no particular reason. These aid in transitioning to/from managed code.
		// Since we don't have a GC, the transition is a no-op.
		[RuntimeExport("RhpReversePInvoke")]
		static void RhpReversePInvoke(IntPtr frame)
		{
		}

		[RuntimeExport("RhpReversePInvokeReturn")]
		static void RhpReversePInvokeReturn(IntPtr frame)
		{
		}

		[RuntimeExport("RhpPInvoke")]
		static void RhpPInvoke(IntPtr frame)
		{
		}

		[RuntimeExport("RhpPInvokeReturn")]
		static void RhpPInvokeReturn(IntPtr frame)
		{
		}

		[RuntimeExport("RhpGcPoll")]
		static void RhpGcPoll()
		{
		}

		[RuntimeExport("RhpFallbackFailFast")]
		static void RhpFallbackFailFast()
		{
			Environment.FailFast(null);
		}

		[RuntimeExport("RhpGetClasslibType")]
		static RuntimeTypeHandle RhpGetClasslibType(MethodTable* mt)
		{
			Console.WriteLine("RhpGetClasslibType");
			return new RuntimeTypeHandle((nint)mt);
		}

		[RuntimeExport("RhpNewFinalizable")]
		static unsafe void* RhpNewFinalizable(Internal.Runtime.MethodTable* pMT)
		{
			Internal.Runtime.MethodTable** result = AllocObject(pMT->_uBaseSize);
			*result = pMT;
			return result;
		}

		[RuntimeExport("RhpNewFast")]
		static unsafe void* RhpNewFast(Internal.Runtime.MethodTable* pMT)
		{
			Internal.Runtime.MethodTable** result = AllocObject(pMT->_uBaseSize);
			*result = pMT;
			return result;
		}

		[RuntimeExport("RhpNewArray")]
		static unsafe void* RhpNewArray(Internal.Runtime.MethodTable* pMT, int numElements)
		{

			if (numElements < 0)
				Environment.FailFast(null);

			Internal.Runtime.MethodTable** result = AllocObject((uint)(pMT->_uBaseSize + numElements * pMT->_usComponentSize));
			*result = pMT;
			*(int*)(result + 1) = numElements;
			return result;
		}

		internal struct ArrayElement
		{
			public object Value;
		}

		[RuntimeExport("RhpStelemRef")]
		public static unsafe void StelemRef(Array array, nint index, object obj)
		{
			ref object element = ref Unsafe.As<ArrayElement[]>(array)[index].Value;

			MethodTable* elementType = (MethodTable*)array.m_pMethodTable->_uBaseType;

			if (obj == null)
				goto assigningNull;

			if (elementType != obj.m_pMethodTable)
				Environment.FailFast("Covariance"); /* covariance */

			doWrite:
			element = obj;
			return;

		assigningNull:
			element = null;
			return;
		}

		[RuntimeExport("RhpCheckedAssignRef")]
		public static unsafe void RhpCheckedAssignRef(void** dst, void* r)
		{
			*dst = r;
		}

		[RuntimeExport("RhpAssignRef")]
		public static unsafe void RhpAssignRef(void** dst, void* r)
		{
			*dst = r;
		}

		[RuntimeExport("RhpByRefAssignRef")]
		public static unsafe void RhpByRefAssignRef(void** dst, void** src)
		{
			void* obj = *src;      // load reference from source slot

			RhpAssignRef(dst, obj);

			dst++;
			src++;
		}

		static unsafe Internal.Runtime.MethodTable** AllocObject(uint size)
		{

			Internal.Runtime.MethodTable** result = (Internal.Runtime.MethodTable**)Libc.Calloc(1, size);

			if (result == null)
				Environment.FailFast(null);

			return result;
		}

		static unsafe void FreeObject(MethodTable** mt)
		{
			Libc.Free(mt);
		}

		[RuntimeExport("RhSuppressFinalize")]
		static unsafe void RhSuppressFinalize(object obj)
		{
			fixed (Internal.Runtime.MethodTable** mt = &obj.m_pMethodTable)
			{
				//FreeObject(mt);
			}
		}

		[RuntimeExport("RhTypeCast_IsInstanceOfClass")]
		public static unsafe bool RhTypeCast_IsInstanceOfClass(Object obj, Internal.Runtime.MethodTable* targetType)
		{
			Internal.Runtime.MethodTable* mt = *((Internal.Runtime.MethodTable**)Unsafe.AsPointer(ref obj));

			//Console.Print("   targetType - ", Utils.PtrToHexString((nint)targetType));
			//Console.Print("   m_pMethodTable - ", Utils.PtrToHexString((nint)mt));

			if (mt == targetType)
			{
				//Console.Print("RhTypeCast_IsInstanceOfClass - true");
				return true;
			}

			//Console.Print("RhTypeCast_IsInstanceOfClass - false");
			return false;
		}

		[RuntimeExport("RhTypeCast_CheckCastClassSpecial")]
		public static unsafe void RhTypeCast_CheckCastClassSpecial(Object obj, Internal.Runtime.MethodTable* target)
		{
			Console.WriteLine("RhTypeCast_CheckCastClassSpecial");

			if (obj == null)
				return; // null casts to any reference type

			Internal.Runtime.MethodTable* mt = *((Internal.Runtime.MethodTable**)Unsafe.AsPointer(ref obj));

			// For minimal runtime, just check normal class inheritance
			while (mt != null)
			{
				if (mt == target)
					return; // cast succeeds

				mt = mt->_uBaseType;
			}

			Environment.FailFast("Invalid cast");
		}
	}
}
