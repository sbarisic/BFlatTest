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

using System.Runtime;
using System.Runtime.CompilerServices;

namespace System
{
	public unsafe sealed class String
	{
		// The layout of the string type is a contract with the compiler.
		private readonly int _length;
		private char _firstChar;

		public int Length => _length;

		[IndexerName("Chars")]
		public unsafe char this[int index]
		{
			[System.Runtime.CompilerServices.Intrinsic]
			get
			{
				return System.Runtime.CompilerServices.Unsafe.Add(ref _firstChar, index);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern unsafe String(char* value, int startindex, int length);

		private static unsafe string Ctor(char* ptr, int startindex, int length)
		{
			string result = FastNewString(length);

			for (int i = 0; i < length; i++)
				Unsafe.Add(ref result._firstChar, i) = ptr[startindex + i];

			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern unsafe String(char* value);

		private static unsafe string Ctor(char* ptr)
		{
			char* cur = ptr;
			while (*cur++ != 0) ;

			string result = FastNewString((int)(cur - ptr - 1));
			for (int i = 0; i < cur - ptr - 1; i++)
				Unsafe.Add(ref result._firstChar, i) = ptr[i];
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern unsafe String(sbyte* value);

		private static unsafe string Ctor(sbyte* ptr)
		{
			sbyte* cur = ptr;
			while (*cur++ != 0) ;

			string result = FastNewString((int)(cur - ptr - 1));
			for (int i = 0; i < cur - ptr - 1; i++)
			{
				if (ptr[i] > 0x7F)
					Environment.FailFast(null);
				Unsafe.Add(ref result._firstChar, i) = (char)ptr[i];
			}
			return result;
		}

		static unsafe string FastNewString(int numChars)
		{
			return NewString("".m_pMethodTable, numChars);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[RuntimeImport("*", "RhpNewArray")]
			static extern string NewString(MethodTable* pMT, int numElements);
		}

		public override String ToString()
		{
			fixed (char* pThis = &this._firstChar)
				return new string(pThis, 0, _length);
		}

		public static string Concat(string StrA, string StrB)
		{
			int lenA = StrA.Length;
			int lenB = StrB.Length;
			string result = FastNewString(lenA + lenB);

			for (int i = 0; i < lenA; i++)
				Unsafe.Add(ref result._firstChar, i) = StrA[i];

			for (int i = 0; i < lenB; i++)
				Unsafe.Add(ref result._firstChar, lenA + i) = StrB[i];

			return result;
		}

		public static string Concat(string StrA, string StrB, string StrC)
		{
			return Concat(Concat(StrA, StrB), StrC);
		}

		public static string Concat(string StrA, string StrB, string StrC, string StrD)
		{
			return Concat(Concat(StrA, StrB), Concat(StrC, StrD));
		}

		public static string Concat(string StrA, string StrB, string StrC, string StrD, string StrE)
		{
			return Concat(Concat(Concat(StrA, StrB), StrC), Concat(StrD, StrE));
		}

		public static string Concat(string StrA, string StrB, string StrC, string StrD, string StrE, string StrF)
		{
			return Concat(Concat(Concat(StrA, StrB), StrC), Concat(Concat(StrD, StrE), StrF));
		}

		public static string Concat(string[] Strings)
		{
			int totalLength = 0;
			for (int i = 0; i < Strings.Length; i++)
			{
				totalLength += Strings[i].Length;
			}

			string result = FastNewString(totalLength);
			int currentPos = 0;

			for (int i = 0; i < Strings.Length; i++)
			{
				string src = Strings[i];
				for (int j = 0; j < src.Length; j++)
				{
					Unsafe.Add(ref result._firstChar, currentPos++) = src[j];
				}
			}

			return result;
		}
	}
}
