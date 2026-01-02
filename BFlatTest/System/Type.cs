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

using Internal.Runtime;

namespace System
{
	internal class TypeDesc
	{
		public RuntimeTypeHandle runtimeTypeHandle;
	}

	public unsafe class Type
	{
		internal MethodTable* _mt;

		public static Type GetTypeFromHandle(RuntimeTypeHandle handle)
		{
			MethodTable* mt = (MethodTable*)(handle.Value);

			//return TypeInternal.GetOrCreateType(mt);
			return new RuntimeType(mt);
		}

		public override string ToString()
		{
			//Console.Print("    flags: ", Utils.PtrToHexString(_mt->_usFlags));

			//if (this == typeof(object))
			//	return "object";

			return "Type_" + Utils.PtrToHexString((nint)_mt);
		}

		public override bool Equals(object obj)
		{
			//Console.WriteLine("Equals!");
			//Console.Print("this - ", GetType().ToString());
			//Console.Print("obj - ", GetType().ToString());

			if (obj is Type other)
			{
				return this._mt == other._mt;
			}
			else if (obj is RuntimeType rt)
			{
				return this._mt == rt._mt;
			}

			return base.Equals(obj);
		}
	}

	public unsafe class RuntimeType : Type
	{
		internal RuntimeType(MethodTable* mt)
		{
			if (mt != null)
				this._mt = mt->_uBaseType;
		}
	}

	internal static unsafe class TypeInternal
	{
		private static RuntimeType[] typeCache = new RuntimeType[1024]; // grow later

		public static RuntimeType GetOrCreateType(MethodTable* mt)
		{
			// trivial hash for now — align/pad later
			int index = ((int)(nuint)mt) & (typeCache.Length - 1);

			var existing = typeCache[index];
			if (existing != null && existing._mt == mt)
				return existing;

			var t = new RuntimeType(mt);
			typeCache[index] = t;
			return t;
		}
	}
}
